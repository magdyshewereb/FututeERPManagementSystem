using System.Data;
using ERPManagement.UI.DataModels.Privilege;
using ERPManagement.UI.GeneralClasses;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using System.Data.Common;
using System.Security.Cryptography.Xml;
using static ERPManagement.UI.Components.Global.ModalDialog;
using ERPManagement.UI.DataModels.ProtectedLocalStorage;
using ERPManagement.UI.DataAccess;

namespace ERPManagement.UI.Components.Base
{
    public partial class Search : ComponentBase    //<TColumn, TPeriod> : ComponentBase where TColumn : new() where TPeriod : new()
    {
        class Column
        { 
            public string Name { get; set; } 
            public string Value { get; set; }
            public bool Available { get; set; }
            public int Index { get; set; }
        }
        public string ConnectionString { get; set; }
        UserLoginData userData { get; set; }
        private SystemSettings systemSettings { get; set; } = new();
        [Parameter]
        public DataTable dtItems { get; set; }
        public DataTable dtSelectedCol { get; set; }
        [Parameter]
        public List<string> InvisibleColumns { get; set; }
        [Parameter]
        public string ColID { get; set; }
        [Parameter]
        public string FormName { get; set; }
        [Parameter]
        public Dictionary<string, Dictionary<string, string>> valueLists { get; set; }
        [Parameter]
        public Dictionary<string, int> ColWidth { get; set; }
        [Parameter]
        public bool isNaveMode { get; set; } = false;
        [Parameter]
        public IStringLocalizer localizer { get; set; }
        [Parameter]
        public EventCallback<object> OnItemSelectedCallback { get; set; }
        [Parameter]
        public EventCallback<bool> OnClose { get; set; }
        [Parameter]
        public EventCallback<DataTable> OnOk { get; set; }
        [Parameter]
        public EventCallback<string> OnDoubleClickCallback { get; set; }
        //[Parameter]
        //public EventCallback OnSaveFilterModalCallback { get; set; }
        public DataTable dtVisCol { get; set; }
        int ItemID { get; set; }
        string selectedItemName { get; set; }
        //TItem selectedItem { get; set; }
        bool ModalFilterVisible = false;
        //Dictionary<string, string> visibleFilterColums = new Dictionary<string, string>();
        List<Column> lstCols=new List<Column>();
        List<Column> lstColsTemp ;
        //Dictionary<string, string> invisibleFilterColums = new Dictionary<string, string>();
        public int SearchPeriod {  get; set; }
       
        // information Dialog
        bool IsInfoMBVisible = false;
        string InfoMessage = "";
        ModalDialogType MBType = ModalDialogType.Ok;
        //Paging
        public int itemsPerPage { get; set; } = 10;
        int currentPage = 1;
        int totalPages { get; set; }
        int counter { get; set; }


        protected override async Task OnInitializedAsync()
        {
            if (ConnectionString == null || ConnectionString == "")
            {
                ConnectionString = await protectedLocalStorageService.GetConnectionStringAsync();
            }
            userData = await protectedLocalStorageService.GetUserDataAsync();
            systemSettings = await protectedLocalStorageService.GetSystemSettingsAsync();
            dtSelectedCol = searchColumnService.SelectByFormName(userData.UserID, FormName, new Main(ConnectionString));
            DataTable dtPeriod = searchPeriodService.SelectByFormName(userData.UserID, FormName, new Main(ConnectionString));
            if (dtPeriod.Rows.Count > 0)
            {
                SearchPeriod = int.Parse(dtPeriod.Rows[0]["SearchPeriodDays"].ToString());
            }
            bool _available = false;
            int _index ;
            foreach (DataColumn col in dtItems.Columns)
            {
                if (col.ColumnName != ColID)
                {
                    _available = dtSelectedCol.Select("ColKey='" + col.ColumnName + "'").Length > 0;
                    _index = _available ? int.Parse(dtSelectedCol.Select("ColKey='" + col.ColumnName + "'")[0]["ColOrder"].ToString()): dtItems.Columns.Count-lstCols.Count;
                    if (col.DataType.Name == "Boolean")
                    {
                        lstCols.Add(new Column { Name = "chk" + col.ColumnName, Value = "", Available = _available, Index = _index });
                        lstCols.Add(new Column { Name = "chkNot" + col.ColumnName, Value = "", Available = _available, Index = _index });
                    }
                    else if (col.DataType.Name == "DateTime")
                    {
                        lstCols.Add(new Column { Name = "dtpFrom" + col.ColumnName, Value = "", Available = _available, Index = _index });
                        lstCols.Add(new Column { Name = "dtpTo" + col.ColumnName, Value = "", Available = _available, Index = _index });
                    }
                    else
                    {
                        lstCols.Add(new Column { Name = "txt" + col.ColumnName, Value = "", Available = _available, Index = _index });
                    }
                }
            }
            //lstCols.OrderBy(o => o.Index asc).ToList();
            lstCols.Sort((x, y) => x.Index.CompareTo(y.Index));
            dtItems.Columns.Add(new DataColumn ("Choose", typeof(bool)) { DefaultValue = false });

            //paging
            totalPages = (int)Math.Ceiling((double)dtItems.Rows.Count / itemsPerPage);
            counter = (currentPage - 1) * itemsPerPage;
        }
        private Task ModalCancel()
        {
            return OnClose.InvokeAsync(false);
        }
        private Task ModalOk()
        {
            DataView dv = new DataView(dtItems);
            dv.RowFilter = "Choose = 1";           
            return OnOk.InvokeAsync(dv.ToTable());
        }
        private Task OnDoubleClick(string ID)
        {
            return OnDoubleClickCallback.InvokeAsync(ID);
        }
        private void CheckAll(bool Choose)
        {
            if (Choose)
            {
                foreach (DataRow dr in dtItems.Rows)
                {
                    dr["Choose"] = true;
                }
            }
            else
            {
                foreach (DataRow dr in dtItems.Rows)
                {
                    dr["Choose"] = false;
                }
            }
        }
        private void Choose(bool Choose, string ID)
        {
            dtItems.Select(ColID+"='" + ID + "'")[0]["Choose"] = Choose;
        }
        private void ModalFilterClose()
        {
            ModalFilterVisible = !ModalFilterVisible;
        }
        private void AddColFilter(bool IsAdded,string ColName)
        {
            int _index = lstCols.Any(x => x.Available == IsAdded) ? lstCols.Where(x => x.Available == IsAdded).LastOrDefault()!.Index + 1 :  1;
            if (ColName != null)
            {
                if (lstCols.Where(x => x.Name == "chk" + ColName).FirstOrDefault() != null)
                {
                    lstCols.Where(x => x.Name == "chk" + ColName).FirstOrDefault()!.Available = IsAdded;
                    lstCols.Where(x => x.Name == "chkNot" + ColName).FirstOrDefault()!.Available = IsAdded;
                    lstCols.Where(x => x.Name == "chk" + ColName).FirstOrDefault()!.Index = _index;
                    lstCols.Where(x => x.Name == "chkNot" + ColName).FirstOrDefault()!.Index = _index;
                }
                else if (lstCols.Where(x => x.Name == "dtpFrom" + ColName).FirstOrDefault() != null)
                {
                    lstCols.Where(x => x.Name == "dtpFrom" + ColName).FirstOrDefault()!.Available = IsAdded;
                    lstCols.Where(x => x.Name == "dtpTo" + ColName).FirstOrDefault()!.Available = IsAdded;
                    lstCols.Where(x => x.Name == "dtpFrom" + ColName).FirstOrDefault()!.Index = _index;
                    lstCols.Where(x => x.Name == "dtpTo" + ColName).FirstOrDefault()!.Index = _index;
                }
                else
                {
                    lstCols.Where(x => x.Name == "txt" + ColName).FirstOrDefault()!.Available = IsAdded;
                    lstCols.Where(x => x.Name == "txt" + ColName).FirstOrDefault()!.Index = _index;
                }
                lstCols.Sort((x, y) => x.Index.CompareTo(y.Index));
                //lstCols.OrderBy(o => o.Index).ToList();
            }
        }
        
        private void MoveUpDn(bool blnUp, string ColName)
        {
            Column col = lstCols.Where(x => x.Name == "txt"+ColName || x.Name == "chk" + ColName || x.Name == "dtpFrom" + ColName).FirstOrDefault()!;
            if (col == null) return;
            int idx = lstCols.IndexOf(col);
            if (blnUp)
            {
                if (idx <= 0) return;
                if (lstCols[idx - 1].Name.StartsWith("txt")) // then back 1 index
                {
                    if (col.Name.StartsWith("chk") || col.Name.StartsWith("dtp")) // then 2 item will up
                    {
                        lstCols.Insert(idx - 1, col);
                        lstCols.Insert(idx, lstCols[idx + 2]);
                        lstCols.RemoveRange(idx + 2, 2);
                    }
                    else // then 1 item will up 1 index
                    {
                        lstCols.Insert(idx - 1, col);
                        lstCols.RemoveAt(idx + 1);
                    }
                }
                else if (lstCols[idx - 1].Name.StartsWith("chk") || lstCols[idx - 1].Name.StartsWith("dtp")) // then back 2 index
                {
                    if (col.Name.StartsWith("chk") || col.Name.StartsWith("dtp"))  // then 2 item will up
                    {
                        lstCols.Insert(idx - 2, col);
                        lstCols.Insert(idx - 1, lstCols[idx + 2]);
                        lstCols.RemoveRange(idx + 2, 2);
                    }
                    else
                    {
                        lstCols.Insert(idx - 2, col);
                        lstCols.RemoveAt(idx + 1);
                    }
                }
            }
            else
            {
                if (idx >= lstCols.Count - 1) return;
                if (col.Name.StartsWith("chk") || col.Name.StartsWith("dtp")) // 2 item will down
                {
                    if (lstCols[idx + 2].Name.StartsWith("txt")) // then down 1 index
                    {
                        lstCols.Insert(idx + 3, col);
                        lstCols.Insert(idx + 4, lstCols[idx + 1]);
                        lstCols.RemoveRange(idx, 2);
                    }
                    else // then down 2 index
                    {
                        lstCols.Insert(idx + 4, col);
                        lstCols.Insert(idx + 5, lstCols[idx + 1]);
                        lstCols.RemoveRange(idx, 2);
                    }
                }
                else if (col.Name.StartsWith("txt")) // 1 item will down
                {
                    if (lstCols[idx + 1].Name.StartsWith("chk") || lstCols[idx + 1].Name.StartsWith("dtp"))// then down 2 index
                    {
                        lstCols.Insert(idx + 3, col);
                        //lstCols.Insert(idx + 4, lstCols[idx + 1]);
                        lstCols.RemoveAt(idx);
                    }
                    else // then down 1 index
                    {
                        lstCols.Insert(idx + 2, col);
                        lstCols.RemoveAt(idx);
                    }
                }
            }
            //lstCols.Sort((x, y) => x.Index.CompareTo(y.Index));
        }
        private DataTable MultiFilterString()
        {
            string Filter = "";
            dtItems.DefaultView.Sort="Choose ASC";
            DataView dv = new DataView(dtItems);
            Filter = "1=1";
            
            foreach (Column item in lstCols.Where(x => x.Value != "" && x.Available))
            {

                if (item.Name.StartsWith("chk"))
                {
                    if (item.Value == "True")
                    {
                        if (item.Name.StartsWith("chkNot"))
                        {
                            Filter += " AND " + item.Name.Remove(0, 6) + " = 0 " ;
                        }
                        else
                        {
                            Filter += " AND " + item.Name.Remove(0, 3) + " = 1 " ;
                        }
                    }

                }
                else if (item.Name.StartsWith("dtp"))
                {
                    if (item.Name.StartsWith("dtpFrom"))
                    {
                        Filter += " AND " + item.Name.Remove(0, 7) + " >= '" + item.Value + "'";
                    }
                    else
                    {
                        Filter += " AND " + item.Name.Remove(0, 5) + " <= '" + item.Value + "'";
                    }
                }
                else if (item.Name.StartsWith("txt") && dtItems.Columns[item.Name.Remove(0, 3)]!.DataType == typeof(int) || dtItems.Columns[item.Name.Remove(0, 3)]!.DataType == typeof(decimal))
                {
                    Filter += " AND " + item.Name.Remove(0, 3) + "=" + decimal.Parse(item.Value);
                }
                else
                {
                    Filter += " AND " + item.Name.Remove(0, 3) + " Like '%" + item.Value + "%'";
                }

            }
            Filter += "or Choose=1 " ;
            dv.RowFilter = Filter;
            totalPages = (int)Math.Ceiling((double)dv.ToTable().Rows.Count / itemsPerPage);
            return dv.ToTable();
        }
       
        private void UpdateFilter(string ColName, string value)
        {
            lstCols.Where(x => x.Name == ColName).First().Value = value;
        }
        private async Task SaveFilterModal()
        {
            Main main = new Main(ConnectionString);
            main.StartBulkTrans(false);
            try
            {
                List<Column> cols = new List<Column>();
                cols = lstCols.Where(x => x.Available).ToList();
                int _key = 0;
                searchPeriodService.DeleteByFormName(userData.UserID,FormName, main);
                searchColumnService.DeleteByFormName(userData.UserID, FormName, main);
                searchPeriodService.Insert_Update(
                    "-1",FormName, userData.UserID, SearchPeriod.ToString(),"0",userData.BranchID,userData.UserID, main);
                for (int i = 0; i < cols.Count; i++)
                { 
                    if (cols[i].Name.StartsWith("chkNot") || cols[i].Name.StartsWith("dtpTo")) continue;

                    searchColumnService.Insert_Update(
                         "-1",
                         FormName,
                         userData.UserID,
                         cols[i].Name.StartsWith("dtp") ? cols[i].Name.Remove(0,7):cols[i].Name.Remove(0,3),
                         _key.ToString(),
                         "0",
                         userData.BranchID,
                         userData.UserID, main);
                    _key++;
                }
                main.EndBulkTrans(false);
                ModalFilterClose();
            }

            catch (Exception ex)
            {
                InfoMessage = main.strMessageDetail + ex.Message;
                IsInfoMBVisible = true;
                main.RollbackBulkTrans(false);
            }
        }
        private void Close()
        {
            IsInfoMBVisible = false;
        }


        #region
        private void PreviousPage()
        {
            if (currentPage > 1)
            {
                currentPage--;
            }
        }
        private void FirstPage()
        {

            currentPage = 1;

        }
        private void NextPage()
        {
            if (currentPage < totalPages)
            {
                currentPage++;
            }
        }
        private void LastPage()
        {
            currentPage = totalPages;

        }
        private void GoToPage(int pageNumber)
        {
            currentPage = pageNumber;
        }
        private void ChangeItemsPerPage(ChangeEventArgs e)
        {
            itemsPerPage = int.Parse(e.Value.ToString());
            totalPages = (int)Math.Ceiling((double)MultiFilterString().Rows.Count / itemsPerPage);
            currentPage = 1;
        }
        #endregion
    }
}