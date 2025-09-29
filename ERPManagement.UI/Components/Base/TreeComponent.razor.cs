using ERPManagement.UI.DataModels.ProtectedLocalStorage;
using ERPManagement.UI.GeneralClasses;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System.Data;

//using System.Data.Linq.SqlClient;

namespace ERPManagement.UI.Components.Base
{
    public partial class TreeComponent<TItem, Tlevel> : ComponentBase where TItem : new() where Tlevel : new()
    {
        [CascadingParameter(Name = "currentObj")]
        public TItem currentObject { get; set; }
        [CascadingParameter(Name = "selectedObj")]
        public TItem selectedNode { get; set; }
        // Next line is needed so we are able to add <TabControl> components inside
        [Parameter]
        public RenderFragment Tabs { get; set; }
        [Parameter]
        public IStringLocalizer localizer { get; set; }
        //Tree column parameters
        [Parameter]
        public string IDCol { get; set; }
        [Parameter]
        public string NoCol { get; set; }
        [Parameter]
        public string NameCol { get; set; }
        [Parameter]
        public string NameEnCol { get; set; }
        [Parameter]
        public string ParentIDCol { get; set; }
        [Parameter]
        public string IsMainCol { get; set; }
        [Parameter]
        public string ItemLevelCol { get; set; }
        [Parameter]
        public string AdditionalCol1 { get; set; }
        [Parameter]
        public string AdditionalCol2 { get; set; }
        [Parameter]
        public string AdditionalCol3 { get; set; }
        [Parameter]
        public string AdditionalCol4 { get; set; }
        [Parameter]
        public string TableName { get; set; }
        //Levels column parameters
        [Parameter]
        public string LevelsTable { get; set; }
        [Parameter]
        public string LevelsCol { get; set; }
        [Parameter]
        public string LevelsWidthCol { get; set; }

        // other Parameter
        [Parameter]
        public List<string> invisibleColumns { get; set; }
        [Parameter]
        public Dictionary<string, Dictionary<string, string>> valueLists { get; set; }//= new Dictionary<string, Dictionary<string, string>>();
        [Parameter]
        public HiddenButtonsConfig hiddenButtons { get; set; }//= new ButtonsVisibilty();

        //Vertual Methods
        [Parameter]
        public Func<TItem, Task<int>> TreeAddDataFun { get; set; }
        [Parameter]
        public Func<TItem, Task<int>> TreeUpdateDataFun { get; set; }
        [Parameter]
        public Func<string> HasTransactionValidationsFun { get; set; }
        [Parameter]
        public Func<Task<List<string>>> ValidateDataFun { get; set; } 
        [Parameter]
        public EventCallback<TItem> OnDeleteCallBack { get; set; }
        [Parameter]
        public EventCallback OnPrepareDataCallback { get; set; }
        [Parameter]
        public EventCallback<TItem> OnClearControlsCallback { get; set; }
        [Parameter]
        public EventCallback<GlobalVariables.States> OnSetControlsCallback { get; set; }
        [Parameter]
        public EventCallback<TItem> OnDisplayDataCallback { get; set; }
        [Parameter]
        public EventCallback<TItem> OnCancelCallBack { get; set; }
        [Parameter]
        public EventCallback<string> OnCboValueChangedCallBack { get; set; }
        [Parameter]
        public EventCallback<bool> OnConfirmCallback { get; set; }
        //public string isArabic;
        public string ConnectionString { get; set; }

        public List<TItem> lstItems { get; set; } = new();
        public List<Tlevel> lstLevels { get; set; } = new();
        string SearchCode = "";
        List<string> lstMessages = new List<string> { "", "" };
 
        bool isNavMode = true;
        public int NodeLevel;
         int selectedIndex = -1;
        GlobalVariables.States currentState = GlobalVariables.States.NavMode;
        UserLoginData userData { get; set; }
        private SystemSettings systemSettings { get; set; } = new();
        bool isModalVisible;


        protected override async Task OnInitializedAsync()
        {
            if (ConnectionString == null || ConnectionString == "")
            {
                ConnectionString = await protectedLocalStorageService.GetConnectionStringAsync();
            }
            await OnPrepareDataCallback.InvokeAsync();
            lstItems = GlobalFunctions.GetListFromDataTable<TItem>("TreeData_Select '" + IDCol
                                       + "','" + NoCol + "','" + NameCol + "','" + NameEnCol + "','" + ParentIDCol
                                       + "','" + IsMainCol + "','" + ItemLevelCol
                                       + "','" + AdditionalCol1 + "','" + AdditionalCol2 + "','" + AdditionalCol3
                                       + "','" + AdditionalCol4 + "','" + TableName + "'" + ",-1,0", new DataAccess.Main(ConnectionString));
            lstLevels = GlobalFunctions.GetListFromDataTable<Tlevel>("TreeLevels_Select '" + LevelsCol
                                + "','" + LevelsWidthCol + "','" + LevelsTable + "'", new DataAccess.Main(ConnectionString));
        }

        private void ClearControls()
        {
            currentObject = new TItem ();
            if (!isNavMode)
            {
                OnClearControlsCallback.InvokeAsync(currentObject);
            }
        }
        public void ChangeNavMode(GlobalVariables.States state)
        {
            OnSetControlsCallback.InvokeAsync(state);
            if (currentState == GlobalVariables.States.Updating)
            {
                OnDisplayDataCallback.InvokeAsync(selectedNode);
            }
            currentState = state;
            isNavMode = currentState == GlobalVariables.States.NavMode;
        }                                               
        private void DisplayData(TItem item)
        {
            if (item != null)
            {
                selectedIndex = lstItems.IndexOf(item);
                OnDisplayDataCallback.InvokeAsync(item);
                //currentObject = item;
                selectedNode = item;
                NodeLevel = int.Parse(GlobalFunctions.GetPropertyValue(item, ItemLevelCol).ToString());
            }
        }
        public int LevelStart(int Level)
        {
            Tlevel item = lstLevels.Where(i => GlobalFunctions.GetPropertyValue(i, "Level") == Level.ToString()).FirstOrDefault();
            return int.Parse(GlobalFunctions.GetPropertyValue(item, "Start").ToString());
        }
        public int LevelEnd(int Level)
        {
            Tlevel item = lstLevels.Where(i => GlobalFunctions.GetPropertyValue(i, "Level") == Level.ToString()).FirstOrDefault();
            return int.Parse(GlobalFunctions.GetPropertyValue(item, "End").ToString());
        }
        public int LevelWidth(int Level)
        {
            Tlevel item = lstLevels.Where(i => GlobalFunctions.GetPropertyValue(i, "Level") == Level.ToString()).FirstOrDefault();
            return int.Parse(GlobalFunctions.GetPropertyValue(item, "Width").ToString());
        }
        public string GetCode()
        {
            if (NodeLevel != 0)
            {
                string Realcode = "";
                int Start = LevelStart(NodeLevel + (GlobalVariables.States.Adding == currentState ? 1 : 0));
                Realcode = GlobalFunctions.GetPropertyValue(selectedNode, NoCol);
                if (GlobalVariables.States.Updating == currentState) Realcode = Realcode.Remove(Start);
                Realcode = Realcode.Insert(Start, GlobalFunctions.GetPropertyValue(currentObject, NoCol).ToString());
                return Realcode;
            }
            else
                return GlobalFunctions.GetPropertyValue(currentObject, NoCol);
        }
        public string HasTransactionValidation()
        {
            if (HasTransactionValidationsFun != null)
            {
                string Msg = HasTransactionValidationsFun.Invoke();
                if (Msg != "")
                {
                    return Msg;
                }
            }
            return "";
        }
        private async Task<List<string>> ValidateData()
        {
            if (GlobalFunctions.GetPropertyValue(currentObject, NameCol) == "" || GlobalFunctions.GetPropertyValue(currentObject, NameCol) == null)
            {
                lstMessages[0] = localizer["CheckEmptyNameMS"];
                return lstMessages;
            }
            if (GlobalFunctions.GetPropertyValue(currentObject, NoCol) == "" || GlobalFunctions.GetPropertyValue(currentObject, NoCol) == null)
            {
                lstMessages[0] = localizer["CheckEmptyCodeMS"];
                return lstMessages;
            }
            TItem[] account = lstItems.Where(a => GlobalFunctions.GetPropertyValue(a, NoCol) == GetCode()).ToArray();
            if (account.Length > 0 && (currentState == GlobalVariables.States.Adding || GlobalFunctions.GetPropertyValue(account[0], IDCol) != GlobalFunctions.GetPropertyValue(selectedNode, IDCol)))
            {
                lstMessages[0] = localizer["CheckCodeExistMS"];
                return lstMessages;
            }
            account = lstItems.Where(a => GlobalFunctions.GetPropertyValue(a, NameCol) == GlobalFunctions.GetPropertyValue(currentObject, NameCol)).ToArray();
            if (account.Length > 0 && (currentState == GlobalVariables.States.Adding || GlobalFunctions.GetPropertyValue(account[0], IDCol) != GlobalFunctions.GetPropertyValue(selectedNode, IDCol)))
            {
                lstMessages[0] = localizer["CheckNameExistMS"];
                return lstMessages;
            }
            if (ValidateDataFun != null)
            {
                lstMessages = await ValidateDataFun.Invoke();
                if (lstMessages[0] != "")
                {
                    return lstMessages;
                }
            }

            return lstMessages;
        }
        private string BtnAddRootClick()
        {
            NodeLevel = 0;
            
            return "";
        }
        private string BtnAddClick()
        {
            if (lstItems.Count == 0 || selectedNode == null)
            {
                return localizer["SelectNodeMS"];
            }
            if (lstLevels.Count <= NodeLevel)
            {
                return localizer["ExceedMaxLevelMS"];
            }
            return HasTransactionValidation();
        }
        private string BtnUpdateClick()
        {
            if (lstItems.Count == 0 || selectedNode == null)
            {
                return localizer["SelectNodeMS"];
            }

            object FullCode = GlobalFunctions.GetPropertyValue(selectedNode, NoCol).Substring(LevelStart(NodeLevel), LevelWidth(NodeLevel)).ToString();
            globalFunctions.SetPropertyValue(currentObject, NoCol, FullCode);
            //js.InvokeVoidAsync("TreeView.inputMaxLength", "AccountNumber", 2); //LevelWidth(selectedObject.LevelID)
            return "";
        }
        public string BtnDeleteClick()
        {
            if (lstItems.Count == 0 || GlobalFunctions.GetPropertyValue(selectedNode, IDCol) == "-1" || selectedNode == null)
            {
                return localizer["SelectNodeMS"];
            }
            if (GlobalFunctions.GetPropertyValue(selectedNode, IsMainCol) == "True")
            {
                return localizer["DeleteMainNodeMS"];
            }
            return HasTransactionValidation();
        }

        private void TreeDeleteData()
        {
            OnDeleteCallBack.InvokeAsync(selectedNode);
            lstItems.RemoveAll(i => GlobalFunctions.GetPropertyValue(i, IDCol) == GlobalFunctions.GetPropertyValue(selectedNode, IDCol));
            js.InvokeVoidAsync("TreeView.setHighlightDedault");
            currentObject = new TItem();
            OnClearControlsCallback.InvokeAsync(currentObject);
        }
        private async Task ConfirmData(bool isConfirm)
        {
           await OnConfirmCallback.InvokeAsync(isConfirm);
        }
        public async Task<bool> SaveData()
        {
            int ID;
            if (currentState == GlobalVariables.States.Adding)
            {
                globalFunctions.SetPropertyValue(currentObject, NoCol, GetCode());
                ID = await TreeAddDataFun.Invoke(currentObject);
                if (ID != 0)
                {
                    //Update parent

                    TItem account = GlobalFunctions.GetListFromDataTable<TItem>("TreeData_Select '" + IDCol
                                        + "','" + NoCol + "','" + NameCol + "','" + NameEnCol + "','" + ParentIDCol
                                        + "','" + IsMainCol + "','" + ItemLevelCol
                                        + "','" + AdditionalCol1 + "','" + AdditionalCol2 + "','" + AdditionalCol3
                                        + "','" + AdditionalCol4 + "','" + TableName + "'," + ID, new DataAccess.Main(ConnectionString)).FirstOrDefault()!;
                    lstItems.Insert(lstItems.Count(), account);
                    if (NodeLevel != 0 && lstItems.Where(a => globalFunctions.GetPropertyValue(a, ParentIDCol) == globalFunctions.GetPropertyValue(selectedNode, IDCol)).Count() == 1)
                    {
                        GlobalFunctions.UpdateTable("Update " + TableName + " set " + IsMainCol + " = 1 where " + IDCol + " = " + globalFunctions.GetPropertyValue(selectedNode, IDCol), new DataAccess.Main(ConnectionString), false);
                        globalFunctions.SetPropertyValue(selectedNode, IsMainCol, true);
                    }
                    return true;
                }
            }
            else
            {
                globalFunctions.SetPropertyValue(currentObject, NoCol, GetCode());
                //TItem oldItem = lstItems.Where(a => globalFunctions.GetPropertyValue(a, IDCol) == globalFunctions.GetPropertyValue(selectedNode, IDCol)).FirstOrDefault()!;
                ID =await TreeUpdateDataFun.Invoke(currentObject);
                if (ID != 0)
                {

                    TItem newItem = GlobalFunctions.GetListFromDataTable<TItem>("TreeData_Select '" + IDCol
                                        + "','" + NoCol + "','" + NameCol + "','" + NameEnCol + "','" + ParentIDCol
                                        + "','" + IsMainCol + "','" + ItemLevelCol
                                        + "','" + AdditionalCol1 + "','" + AdditionalCol2 + "','" + AdditionalCol3
                                        + "','" + AdditionalCol4 + "','" + TableName + "'," + ID, new DataAccess.Main(ConnectionString)).FirstOrDefault()!;
                    globalFunctions.SetPropertyValue(currentObject, IDCol,int.Parse(globalFunctions.GetPropertyValue(currentObject, IDCol).ToString()));
                    if (globalFunctions.GetPropertyValue(selectedNode, NoCol) != globalFunctions.GetPropertyValue(currentObject, NoCol))
                    {
                        GlobalFunctions.UpdateTable("update"+ TableName + " set " + NoCol + " = " + globalFunctions.GetPropertyValue(currentObject, NoCol)
                                                        + "' + SUBSTRING("+ NoCol + ", " + globalFunctions.GetPropertyValue(selectedNode, NoCol).ToString().Length + 1
                                                        + ", LEN("+ NoCol + ")) Where " + NoCol + " Like '" + globalFunctions.GetPropertyValue(selectedNode, NoCol) + "%'", new DataAccess.Main(ConnectionString), false);
                        List<TItem> lstChilds = lstItems.Where(i => globalFunctions.GetPropertyValue(i, NoCol).StartsWith(globalFunctions.GetPropertyValue(selectedNode, NoCol))).ToList();

                        for (int i = 0; i < lstChilds.Count(); i++)
                        {
                            globalFunctions.SetPropertyValue(lstChilds[i], NoCol, globalFunctions.GetPropertyValue(currentObject, NoCol) + globalFunctions.GetPropertyValue(lstChilds[i], NoCol).ToString().Remove(0, globalFunctions.GetPropertyValue(selectedNode, NoCol).ToString().Length));
                        }
                    }
                    lstItems[selectedIndex] = newItem;
                   // lstItems.Add(currentObject);
                    selectedNode = currentObject;
                    return true;
                }
            }
            return false;
        }
        private void BtnCancel()
        {
            OnCancelCallBack.InvokeAsync();
        }
        private void cboValueChange(string value)
        {
            OnCboValueChangedCallBack.InvokeAsync(value);
        }
        
    }
}

