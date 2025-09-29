using ERPManagement.UI.Components.Base;
using ERPManagement.UI.DataAccess;
using ERPManagement.UI.DataModels.Accounting;
using ERPManagement.UI.DataModels.ProtectedLocalStorage;
using ERPManagement.UI.GeneralClasses;
using ERPManagement.UI.DataModels.Privilege;
using ERPManagement.UI.DataModels.StockControl;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic;
using System.Data;
using System.Security.Principal;
using static ERPManagement.UI.Components.Global.ModalDialog;
using static System.Runtime.CompilerServices.RuntimeHelpers;


namespace ERPManagement.UI.Pages.Accounting.Transactions
{
    public partial class FrmJV : ComponentBase
    {
        public string ConnectionString { get; set; }

        JV jvObj = new JV();
        JV jvSelectedObj;
        JV jvRepeatedObj;
        JVDetails jvDetailsDefultObj =new JVDetails();
        DataTable dtCurrency = new DataTable();
        DataTable dtJVDefaults = new DataTable();
        List<JVDetails> lstJVDetails = new List<JVDetails>();
        DataTable dtSubAccounts = new DataTable(); //for sub accounts>
        DataTable dtCostCenters = new DataTable(); //for sub accounts>
        DataTable dtJVs = new DataTable(); //for sub accounts>
        DataTable dtAccounts = new DataTable(); //for sub accounts>
        DataSet ds = new DataSet();
        List<string> JVDetailsInvisibleColumns;
        private DataGrid<JVDetails> gridComponentJVDetails;

        bool isNavMode = true;
        GlobalVariables.States currentState = GlobalVariables.States.NavMode;
        UserLoginData userData { get; set; }
        private SystemSettings systemSettings { get; set; } = new();
        HiddenButtonsConfig hiddenButtons = new HiddenButtonsConfig();
        Dictionary<string, Dictionary<string, string>> valueLists = new Dictionary<string, Dictionary<string, string>>();
        Dictionary<string, string> lstFilters = new Dictionary<string, string>();
        Dictionary<string, int> lstSearchWidth = new Dictionary<string, int>();
        Dictionary<string, int> lstDetailGridWidth = new Dictionary<string, int>();

        string RowID = "";
        //var of controles not saved
        bool chkCurrency = false;
        decimal diff = 0;
        bool IsRepeatedJV = false;
        bool UseSubCostCenters, UseSubAccounts , UseCostCenters , UseCurrency ;

        //modal box
        ModalDialogType MBType = ModalDialogType.Ok;
        int confirmUpdatechildAcc = -1;
        bool IsInfoMBVisible = false;
        bool IsSearchMDVisible = false;
        string InfoMessage = "";
        // confirmUpdatechildAcc is int to be able to return more than two state (1=true, 0=false,-1 is not confermed)
        string QuesMessage = "";
        DateTime jvDate
        {
            get { return jvObj.JVDate; }
            set
            {
                jvObj.JVDate = value;
                DateChanged();
            }
        }
        Main main;
        protected override async Task OnInitializedAsync()
        {
            if (ConnectionString == null || ConnectionString == "")
            {
                ConnectionString = await protectedLocalStorageService.GetConnectionStringAsync();
            }
            userData = await protectedLocalStorageService.GetUserDataAsync();
            systemSettings = await protectedLocalStorageService.GetSystemSettingsAsync();
            main = new Main(ConnectionString);
            UseSubAccounts = int.Parse(main.ExecuteQuery_DataTable("Select Count(*) as SA from A_SubAccounts Where IsMain = 0", false, null).Rows[0][0].ToString()) > 0;
            UseCostCenters = int.Parse(main.ExecuteQuery_DataTable("Select Count(*) as CC From A_CostCenters Where IsMain = 0", false, null).Rows[0][0].ToString()) > 0;
            UseSubCostCenters = int.Parse(main.ExecuteQuery_DataTable("Select Count(*) as CC From A_SubCostCenter Where IsMain = 0", false, null).Rows[0][0].ToString()) > 0;
            UseCurrency = int.Parse(main.ExecuteQuery_DataTable("Select Count(*) as SA from Currency ", false, null).Rows[0][0].ToString()) > 1;

            JVDetailsInvisibleColumns = new List<string> { "JVDetailID", "JVID", "BranchID", "Deleted", UseSubAccounts?"": "SubAccountID", UseSubCostCenters?"": "SubCostCenterID",
                                             UseCostCenters?"":"CostCenterID" , UseCurrency?"":"CurrencyID" , UseCurrency ? "" : "ExchangeRate", UseCurrency ? "" : "LocalDebit", UseCurrency ? "" : "LocalCredit" };
            hiddenButtons.HideCopy = hiddenButtons.HideAddRoot = true;
            lstFilters.Add("SubAccountID", "AccountID");
            jvObj.JVDate = DateTime.Now;
            lstJVDetails = jvDetailService.SelectByJVID(0, systemSettings.IsArabic, new Main(ConnectionString));
            dtJVDefaults = jvDefaultService.Select(-1, systemSettings.IsArabic, new Main(ConnectionString));
            //Filling the Sub Accounts DropDown
            if (UseSubAccounts)
            {
                dtSubAccounts = subAccountsService.SelectByAccountID(-1, systemSettings.IsArabic, new Main(ConnectionString));
                valueLists.Add("SubAccountID", dtSubAccounts.AsEnumerable().Select(row => new { attribute1_name = row.Field<int>("SubAccountID").ToString(), attribute2_name = row.Field<string>("Name") })
                                                                        .Distinct().ToDictionary(s => s.attribute1_name, s => s.attribute2_name));
                dtSubAccounts.TableName = "SubAccountID";
                dtSubAccounts.Prefix = "Name"; // to be able to get column name from query not from table
                ds.Tables.Add(dtSubAccounts);
            }
            dtAccounts = accountService.FillCombo(GlobalVariables.SeeingInvisibleAcounts ? "1" : "0", systemSettings.IsArabic, new Main(ConnectionString));
            valueLists.Add("AccountID", dtAccounts.AsEnumerable().ToDictionary(row => row.Field<int>("AccountID").ToString(), row => row.Field<string>("Name")));
            //Filling the Cost Centers Dropdown
            if (UseCostCenters)
            {
                dtCostCenters = costCenterService.FillCombo(systemSettings.IsArabic, new Main(ConnectionString));
                valueLists.Add("CostCenterID", dtCostCenters.AsEnumerable().ToDictionary(row => row.Field<int>("CostCenterID").ToString(), row => row.Field<string>("Name")));
            }
            FillCurrencyDropDown();
            // set jvDetails Default Values
            jvDetailsDefultObj.Debit=0;
            jvDetailsDefultObj.Credit=0;
            jvDetailsDefultObj.CurrencyID=1;
            jvDetailsDefultObj.ExchangeRate= (decimal)dtCurrency.Select("CurrencyID=" + jvDetailsDefultObj.CurrencyID.ToString())[0]["ExchangeRate"];
            jvDetailsDefultObj.Notes="";
            /////// for search  columns width *---*-*-*--------******
            lstSearchWidth.Add("JVNo", 10);
            lstSearchWidth.Add("JVDate", 10);
            lstSearchWidth.Add("TotalDebit", 10);
            lstSearchWidth.Add("TransTypeName", 12);
            lstSearchWidth.Add("JournalName", 10);
            lstSearchWidth.Add("JVTypeName", 10);
            lstSearchWidth.Add("ReceiptNo", 10);
            lstSearchWidth.Add("Notes", 20);
            lstSearchWidth.Add("BranchName", 5);
            lstSearchWidth.Add("IsOpenningJv", 5);
            lstSearchWidth.Add("IsInternalJV", 5);
            /////// for Detail Grid columns width *---*-*-*--------******
            lstDetailGridWidth.Add("AccountID", 16);
            lstDetailGridWidth.Add("SubAccountID", 15);
            lstDetailGridWidth.Add("CostCenterID", 15);
            lstDetailGridWidth.Add("SubCostCenterID", 15);
            lstDetailGridWidth.Add("Debit", 8);
            lstDetailGridWidth.Add("Credit", 8);
            lstDetailGridWidth.Add("CurrencyID", 6);
            lstDetailGridWidth.Add("ExchangeRate", 7);
            lstDetailGridWidth.Add("LocalDebit", 7);
            lstDetailGridWidth.Add("LocalCredit", 7);
            lstDetailGridWidth.Add("IsDocumented", 5);
            lstDetailGridWidth.Add("Notes",0);
        }
        private void ClearControls()
        {
            jvObj = new JV();
            jvObj.JVDate = DateTime.Now;
            if (currentState == GlobalVariables.States.Adding)
                jvObj.TotalDebit = 0;
            jvObj.TotalCredit = 0;
            diff = 0;
            jvObj.JVNo = currentState == GlobalVariables.States.Adding ? jvService.GetCode(jvObj.JVDate.ToString("MM'/'dd'/'yyyy"), userData.BranchID, jvObj.TransTypeID, new Main(ConnectionString)) : "";
            chkCurrency = false;
            lstJVDetails.Clear();
            //cboCurrency.SelectedIndex = -1;
            //cboCurrency.Clear();
        }
        public void SetControls(GlobalVariables.States state)
        {
            currentState = state;
            isNavMode = currentState == GlobalVariables.States.NavMode;
            if (currentState == GlobalVariables.States.Updating && UseSubAccounts)
            {
                for (int i = 0; i < lstJVDetails.Count; i++)
                {
                    if (lstJVDetails[i].AccountID != null)
                    {
                        //Filling the Sub Accounts UDD
                        int AccountID = lstJVDetails[i].AccountID;
                        if (UseSubAccounts)
                        {
                            valueLists.Remove("SubAccountID");
                            valueLists.Add("SubAccountID", dtSubAccounts.Select("AccountID=" + AccountID).ToDictionary(row => row.Field<int>("SubAccountID").ToString(), row => row.Field<string>("Name")));

                            if (valueLists["SubAccountID"].Count == 0)
                            {
                                lstJVDetails[i].SubAccountID = null;
                            }
                        }
                    }
                }
            }
        }
        private void DisplayData()
        {
            if (jvSelectedObj != null)
            {
                jvSelectedObj = jvService.Select(jvSelectedObj.JVID, "-1", systemSettings.IsArabic, new Main(ConnectionString)).FirstOrDefault();
                jvObj = jvSelectedObj;
                lstJVDetails = jvDetailService.SelectByJVID(jvObj.JVID, systemSettings.IsArabic, new Main(ConnectionString));
                if (jvObj.Approved || jvObj.IsInternalJV)
                {
                    hiddenButtons.HideDelete = false;
                    hiddenButtons.HideUpdate = false;
                }
                else
                {
                    hiddenButtons.HideDelete = true;
                    hiddenButtons.HideUpdate = true;
                }
            }
            else
            {
                ClearControls();
            }
        }
        private void FillData()
        {
            if (IsRepeatedJV)
            {
                //btnAddClick();
                DisplayDataRepeatedJV();
                return;
            }

            if (RowID == "")
            {
                jvSelectedObj = null;
            }
            else
            {
                List<JV> lst = jvService.Select(int.Parse(RowID), userData.BranchID, systemSettings.IsArabic, new Main(ConnectionString));
                if (lst.Count > 0) jvSelectedObj = lst[0];
                else jvSelectedObj = null;
            }
            DisplayData();

        }
        private async Task FillData(int ID)
        {
            RowID = ID.ToString();
            List<JV> lst = jvService.Select(int.Parse(RowID), userData.BranchID, systemSettings.IsArabic, new Main(ConnectionString));
            if (lst.Count > 0) jvSelectedObj = lst[0];
            else jvSelectedObj = null;

            DisplayData();
            IsSearchMDVisible = false;
        }
        private async Task FillData(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                dtJVs = dt;
                RowID = dt.Rows[0]["JVID"].ToString();
                List<JV> lst = jvService.Select(int.Parse(RowID), userData.BranchID, systemSettings.IsArabic, new Main(ConnectionString));
                if (lst.Count > 0) jvSelectedObj = lst[0];
                else jvSelectedObj = null;

                DisplayData();
                IsSearchMDVisible = false;
            }
        }
        private void FillCurrencyDropDown()
        {
            dtCurrency = currencyService.FillCurrencyByDate(jvObj.JVDate.ToString("MM'/'dd'/'yyyy"), systemSettings.IsArabic, new Main(ConnectionString));

            if (UseCurrency)
            {
                valueLists.Remove("CurrencyID");
                valueLists.Add("CurrencyID", dtCurrency.AsEnumerable().ToDictionary(row => row.Field<int>("CurrencyID").ToString(), row => row.Field<string>("CurrencyName")));
            }
        }
        public void DisplayDataRepeatedJV()
        {
            jvObj.JVDate = DateTime.Now;
            jvObj.TransTypeID = jvRepeatedObj.TransTypeID;
            jvObj.TotalDebit = jvRepeatedObj.TotalDebit;
            jvObj.TotalCredit = jvRepeatedObj.TotalCredit;
            diff = jvRepeatedObj.TotalDebit - jvRepeatedObj.TotalCredit;
            jvObj.Notes = jvRepeatedObj.Notes;
            jvObj.IsOpenningJv = false;
            jvObj.JVNo = currentState == GlobalVariables.States.Adding ? jvService.GetCode(jvObj.JVDate.ToString("MM'/'dd'/'yyyy"), userData.BranchID, jvObj.TransTypeID, new Main(ConnectionString)) : "";

            ////Details
            //dtDetails = BusinessLayer.Accounting.RepeatedJVDetails.SelectByRepeatedJVID_ForJV(drRepeatedJV["RepeatedJVID"].ToString(),
            //    GlobalVariables.IsArabic ? "1" : "0");
            //ULGData.DataSource = dtDetails;
            //InitGrid();

            //if (UseSubAccounts)
            //{
            //    for (int i = 0; i < ULGData.Rows.Count; i++)
            //    {
            //        if (ULGData.Rows[i].Cells["AccountID"].Value != DBNull.Value)
            //        {
            //            //Filling the Sub Accounts UDD
            //            int AccountID = int.Parse(ULGData.Rows[i].Cells["AccountID"].Value.ToString());
            //            Infragistics.Win.ValueList lst = getSubAccountValueList(AccountID);
            //            ULGData.Rows[i].Cells["SubAccountID"].ValueList = lst;
            //            if (lst.ValueListItems.Count == 0)
            //            {
            //                ULGData.Rows[i].Cells["SubAccountID"].Value = DBNull.Value;
            //            }
            //        }
            //    }
            //}


        }
        private async Task<List<string>> ValidateData()
        {
            List<string> lstMessages = new List<string> { "", "" };

            if (!fiscalYearService.ChkForConfirmedFiscalYear(jvObj.JVDate.ToString("MM'/'dd'/'yyyy"), new Main(ConnectionString)))
            {
                lstMessages[0] = systemSettings.IsArabic ? "السنة المالية غير معتمدة" : "The Fiscal Year is not confirmed..";
                return lstMessages;
            }
            else if (fiscalYearService.ChkForClosingFsicalPeriod(jvObj.JVDate.ToString("MM'/'dd'/'yyyy"), userData.BranchID, new Main(ConnectionString)))
            {
                lstMessages[0] = systemSettings.IsArabic ? "لقد قمت بأختيار تاريخ يقع في فتره ماليه مغلقة" : "The Date you choosed\n\r exists in closed fisical period";
                return lstMessages;
            }
            else if (jvObj.TransTypeID == null || jvObj.TransTypeID == 0)
            {
                lstMessages[0] = systemSettings.IsArabic ? "برجاء أن تختار نوع الحركه" : "Please select a Tans. type for this JV";
                return lstMessages;
            }
            else if (jvObj.JVNo == null || jvObj.JVNo == "")
            {
                lstMessages[0] = systemSettings.IsArabic ? "برجاء إدخال كود القيد" : "Please Enter JV Code";
                return lstMessages;
            }
            else if (lstJVDetails.Count < 2)
            {
                lstMessages[0] = systemSettings.IsArabic ? "برجاء إدخــال تفاصيل لهذا القيـــد" : "Please insert details for this JV";
                return lstMessages;
            }
            else if (jvObj.TotalDebit != jvObj.TotalCredit)
            {
                lstMessages[0] = systemSettings.IsArabic ? "القيد غير متوازن. \r\n برجاء مراجعة تفاصيل القيد" : "The J.V. is Not Balanced. Please Recheck the J.V. Details";
                return lstMessages;
            }
            else if (jvObj.TotalDebit == 0)
            {
                lstMessages[0] = systemSettings.IsArabic ? "اجمالى المدين لابد ان يكون اكبر من الصفر" : "Total Debit Must Be Greater Than Zero";
                return lstMessages;
            }
            for (int i = 0; i < lstJVDetails.Count; i++)
            {
                if (lstJVDetails[i].AccountID == null)
                {
                    lstMessages[0] = systemSettings.IsArabic ? "برجاء ادخال اسم الحساب أو حذف السطر  " : "Please Enter Account Name or Delete Row ";
                    return lstMessages;
                }
                if (lstJVDetails[i].ExchangeRate <= 0)
                {
                    lstMessages[0] = systemSettings.IsArabic ? "سعر التحويل لابد ان يكون اكبر من الصفر" : "Exchange Rate Must Be Greater Than Zero";
                    return lstMessages;
                }

                //Enforcing SubAccounts Choice
                if (UseSubAccounts) // && GlobalFunctions.GetOption("EnforceSubAccountsUse")
                {
                    if (
                        valueLists["SubAccountID"] != null
                        && dtSubAccounts.Select("AccountID=" + lstJVDetails[i].AccountID).Length > 0
                        && (lstJVDetails[i].SubAccountID == null || dtSubAccounts.Select(" SubAccountID = " + lstJVDetails[i].SubAccountID).Length == 0)
                        )
                    {
                        lstMessages[0] = systemSettings.IsArabic ? "برجاء اختيار حساب تحليلي" : "Please choose SubAccount";
                        return lstMessages;
                    }
                }

                //Enforcing CostCenters Choice
                if (UseCostCenters)// && GlobalFunctions.GetOption("EnforceCostCentersUse")
                {
                    if (
                        dtAccounts.Select("AccountID = " + lstJVDetails[i].AccountID)[0]["AccountTypeID"].ToString() == "3"
                    ||
                        dtAccounts.Select("AccountID = " + lstJVDetails[i].AccountID)[0]["AccountTypeID"].ToString() == "4"
                        )
                    {
                        if (lstJVDetails[i].CostCenterID == null)
                        {
                            lstMessages[0] = systemSettings.IsArabic ? "برجاء اختيار مركز تكلفة" : "Please choose Cost-Center";
                            return lstMessages;
                        }
                    }
                }
            }
            if (jvService.Check_Code(currentState == GlobalVariables.States.Adding ? 0 : jvObj.JVID, jvObj.JVDate.ToString("MM'/'dd'/'yyyy"), jvObj.JVNo, new Main(ConnectionString)))
            {
                string NewCode = jvService.GetCode(jvObj.JVDate.ToString("MM'/'dd'/'yyyy"), userData.BranchID, jvObj.TransTypeID, new Main(ConnectionString));

                if (confirmUpdatechildAcc == -1)
                {
                    lstMessages[1] = systemSettings.IsArabic ?
                    "رقم هذا القيد متواجد من قبل \n "
                        + "سوف يتم الحفظ برقم " + NewCode
                        : "The JV Number Already Exists "
                        + "It Will Be Saved With No. : " + NewCode;
                }
                else
                {
                    if (confirmUpdatechildAcc == 1)
                    {
                        jvObj.JVNo = NewCode;
                    }
                    else
                    {
                        lstMessages[0] = systemSettings.IsArabic ? "برجاء ادخال رقم لهذا القيد" : "Please enter JV Number";
                        return lstMessages;
                    }
                }

            }
            //else if( currentObject.EINVCurrencyID)
            //else if (CheckForValue("Currency", "CurrencyNameAr", txtCurrencyArabicName.Text, (Updating ? this.ULGData.ActiveRow.Cells["CurrencyNameAr"].Value.ToString() : ""), true) > 0)
            //{

            //    errorMessage = isArabic ? " إسم العملة بالعربية متواجد من قبل ": "Currency Arabic Name Already Exist";
            //    txtCurrencyArabicName.Focus();
            //    return false;
            //}
            return lstMessages;
        }
        private async Task<bool> Add()
        {
            Main main = new Main(ConnectionString);
            main.StartBulkTrans(true);
            try
            {
                int ID = jvService.GenerateJV_Insert(jvObj, lstJVDetails, userData.BranchID, userData.UserID, new Main(ConnectionString));

                if (main.strMessageDetail != null && main.strMessageDetail.Length > 0)
                {
                    InfoMessage = main.strMessageDetail;
                    IsInfoMBVisible = true;
                    main.RollbackBulkTrans(true);
                    StateHasChanged();
                    return false;
                }
                else
                {
                    main.EndBulkTrans(true);
                    RowID = ID.ToString();
                    // if (IsRepeatedJV) Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                InfoMessage = main.strMessageDetail + ex.Message;
                IsInfoMBVisible = true;
                main.RollbackBulkTrans(true);
                StateHasChanged();
                return false;
            }
        }
        private async Task<bool> Update()
        {
            Main main = new Main(ConnectionString);
            main.StartBulkTrans(true);
            try
            {
                int ID = jvService.GenerateJV_Update(jvObj, lstJVDetails, userData.BranchID, userData.UserID, new Main(ConnectionString));

                if (main.strMessageDetail != null && main.strMessageDetail.Length > 0)
                {
                    InfoMessage = main.strMessageDetail;
                    IsInfoMBVisible = true;
                    main.RollbackBulkTrans(true);
                    StateHasChanged();
                    return false;
                }
                else
                {
                    main.EndBulkTrans(true);
                    return true;
                }
            }
            catch (Exception ex)
            {
                InfoMessage = main.strMessageDetail + ex.Message;
                IsInfoMBVisible = true;
                main.RollbackBulkTrans(true);
                StateHasChanged();
                return false;
            }
        }
        private void Copy()
        {
        }
        private void Delete()
        {

        }
        private void btnViewClick()
        {
        }
        private void btnSearchClick()
        {
            Main main = new Main(ConnectionString);
            DataTable dtPeriod =new DataTable();
            dtPeriod = searchPeriodService.SelectByFormName(userData.UserID, "ERP.Accounting.Search.frmJVSearchReport", main);
            DateTime MinDate = DateTime.Now.AddDays(-(dtPeriod.Rows.Count > 0 ? Convert.ToInt32(dtPeriod.Rows[0]["SearchPeriodDays"]) : 10000));
            DateTime MaxDate = DateTime.Now.AddYears(50);

            dtJVs = jvService.Search(userData.BranchID, MinDate.ToString("MM'/'dd'/'yyyy"), MaxDate.ToString("MM'/'dd'/'yyyy"), -1, false, systemSettings.IsArabic, false, new Main(ConnectionString));
            
            IsSearchMDVisible = true;
        }
        private void Next()
        {
            if (dtJVs != null && dtJVs.Rows.Count > 1)
            {
                if (jvObj != null)
                {
                    for (int i = 1; i < dtJVs.Rows.Count; i++)
                    {
                        if (dtJVs.Rows[i]["JVID"].ToString() == jvObj.JVID.ToString())
                        {
                            RowID = dtJVs.Rows[i - 1]["JVID"].ToString();
                            FillData();
                            break;
                        }
                    }
                }
                else
                {
                    RowID = dtJVs.Rows[0]["JVID"].ToString();
                    FillData();
                }
            }
            else
            {
                Main main = new Main(ConnectionString);
                DataTable dt = main.SelectNext("," + userData.BranchID + ",", "A_JV", "JVID", "JVNo", "JVDate", RowID, "", "1");
                if (dt.Rows.Count > 0)
                {
                    RowID = dt.Rows[0]["JVID"].ToString();
                }
                else
                {
                    RowID = "";
                }
                FillData();
            }

        }
        private void Previous()
        {
            if (dtJVs != null && dtJVs.Rows.Count > 1)
            {
                if (jvObj != null)
                {
                    for (int i = 0; i < dtJVs.Rows.Count - 1; i++)
                    {
                        if (dtJVs.Rows[i]["JVID"].ToString() == jvObj.JVID.ToString())
                        {
                            RowID = dtJVs.Rows[i + 1]["JVID"].ToString();
                            FillData();
                            break;
                        }
                    }
                }
                else
                {
                    RowID = dtJVs.Rows[dtJVs.Rows.Count - 1]["JVID"].ToString();
                    FillData();

                }
            }
            else
            {
                Main main = new Main(ConnectionString);
                DataTable dt = main.SelectNext("," + userData.BranchID + ",", "A_JV", "JVID", "JVNo", "JVDate", RowID, "", "0");
                if (dt.Rows.Count > 0)
                {
                    RowID = dt.Rows[0]["JVID"].ToString();
                }
                else
                {
                    RowID = "";
                }
                FillData();
            }
        }

        private string CheckBeforeDelete()
        {
            //if (GlobalVariables.LocalCurrencyID == currentObject.CurrencyID)
            //{
            //    return isArabic ? "لا يمكن حذف العملة المحلية" : "You cannot delete the local currency.";
            //}
            return "";
        }
        private void Cancel()
        {
            FillData();
            //currentObject = oldObject.Clone();
            //currencies[currencies.FindIndex(cur => cur.CurrencyID == currentObject.CurrencyID)] = currentObject;
        }
        private void Close()
        {
            dtJVs = null;
            IsInfoMBVisible = false;
            IsSearchMDVisible = false;
        }
        private void ChangeNavMode(GlobalVariables.States state)
        {
            currentState = state;
            isNavMode = currentState == GlobalVariables.States.NavMode;
        }
        private JVDetails AddNewDefaultItem( JVDetails _item)
        {
            _item = new JVDetails();
            _item = jvDetailsDefultObj.Clone();
            return _item;
        }
        private JVDetails CellUpdate(JVDetails item, string col)
        {
            if (col == "Debit")
            {
                item.LocalDebit = item.Debit == null ? 0 : (decimal)item.Debit * item.ExchangeRate;
            }
            else if (col == "Credit")
            {
                item.LocalCredit = item.Credit == null ? 0 : (decimal)item.Credit * item.ExchangeRate;
            }
            else if (col == "ExchangeRate" || col == "CurrencyID")
            {
                item.LocalDebit = item.Debit == null ? 0 : (decimal)item.Debit * item.ExchangeRate;
                item.LocalCredit = item.Credit == null ? 0 : (decimal)item.Credit * item.ExchangeRate;
            }
            else if (col == "LocalDebit")
            {
                item.Debit = item.LocalDebit == null ? 0 : item.LocalDebit / item.ExchangeRate;
            }
            else if (col == "LocalCredit")
            {
                item.Credit = item.LocalCredit == null ? 0 : item.LocalCredit / item.ExchangeRate;
            }
            if (col == "Credit" && globalFunctions.GetPropertyValue(item, col) == null)
            {
                globalFunctions.SetPropertyValue(item, col, 0);
            }
            if (col == "Debit" && globalFunctions.GetPropertyValue(item, col) == null)
            {
                globalFunctions.SetPropertyValue(item, col, 0);
            }
            if (col == "LocalCredit" && globalFunctions.GetPropertyValue(item, col) == null)
            {
                globalFunctions.SetPropertyValue(item, col, 0);
            }
            if (col == "LocalDebit" && globalFunctions.GetPropertyValue(item, col) == null)
            {
                globalFunctions.SetPropertyValue(item, col, 0);
            }
            if (col == "ExchangeRate" && globalFunctions.GetPropertyValue(item, col) == null)
            {
                globalFunctions.SetPropertyValue(item, col, 1.00m);
            }
            CalculateTotals();
            return item;
        }
        private JVDetails CellListChanged(JVDetails item, string col)
        {
            if (col == "CurrencyID")
            {
                item.ExchangeRate = (decimal)dtCurrency.Select("CurrencyID=" + item.CurrencyID.ToString())[0]["ExchangeRate"];
                CellUpdate(item, col);
            }
            else if (UseSubAccounts && col == "AccountID")
            {
                //int AccountID = item.AccountID == 0 ? (int)dtAccounts.Select("AccountID=" + item.AccountID.ToString())[0]["AccountID"] : 0;
                //if (AccountID != 0)
                //{
                item.SubAccountID = null;
                valueLists.Remove("SubAccountID");
                valueLists.Add("SubAccountID", dtSubAccounts.Select("AccountID=" + item.AccountID).ToDictionary(row => row.Field<int>("SubAccountID").ToString(), row => row.Field<string>("Name")));
                //}
                //else
                //{
                //    e.Cell.Row.Cells["SubAccountID"].ValueList = null;
                //    e.Cell.Row.Cells["SubAccountID"].Value = DBNull.Value;
                //    e.Cell.Row.Cells["CostCenterID"].Value = DBNull.Value;
                //}
            }
            StateHasChanged();
            return item;
        }
        private void CalculateTotals()
        {
            decimal TotalDebit = 0;
            decimal TotalCredit = 0;
            for (int i = 0; i < lstJVDetails.Count; i++)
            {
                //Summing Local Debit and Local Credit
                TotalDebit += lstJVDetails[i].LocalDebit;
                TotalCredit += lstJVDetails[i].LocalCredit;
            }
            jvObj.TotalDebit = TotalDebit;
            jvObj.TotalCredit = TotalCredit;
            diff = TotalDebit - TotalCredit;
            StateHasChanged();
        }
        async Task chkCurrencyChecked(ChangeEventArgs args)
        {
            chkCurrency = (bool)args.Value;
        }
        private void cboCurrencyValueChanged(int id)
        {
            if (id != -1)
            {
                decimal _ExchangeRate = decimal.Parse(((decimal)dtCurrency.Select("CurrencyID=" + id)[0]["ExchangeRate"]).ToString("0.##"));
                for (int i = 0; i < lstJVDetails.Count; i++)
                {
                    lstJVDetails[i].CurrencyID = id;
                    lstJVDetails[i].ExchangeRate = _ExchangeRate;
                    CellUpdate(lstJVDetails[i], "ExchangeRate");
                }
                jvDetailsDefultObj.CurrencyID = id;
                jvDetailsDefultObj.ExchangeRate = _ExchangeRate;
            }
        }
        private void DateChanged()
        {
            FillCurrencyDropDown();

            if (currentState == GlobalVariables.States.Adding)
                jvObj.JVNo = jvService.GetCode(jvObj.JVDate.ToString("MM'/'dd'/'yyyy"), userData.BranchID, jvObj.TransTypeID, new Main(ConnectionString));
        }
        //private void SaveFilterModal()
        //{
        //    Main main = new Main(ConnectionString);
        //    main.StartBulkTrans(true);
        //    try
        //    {
        //        searchPeriodService.DeleteByFormName(userData.UserID, "ERP.Accounting.Search.frmJVSearchReport", main);
        //        searchColumnService.DeleteByFormName(userData.UserID, "ERP.Accounting.Search.frmJVSearchReport", main);
        //        searchPeriodService.Insert_Update(
        //            new SearchPeriod (),userData.UserID,main);
        //        for (int i = 0; i < 5; i++)
        //        {
        //           searchColumnService.Insert_Update(
        //                new SearchColumns(), userData.UserID, main);
        //        }
        //        main.RollbackBulkTrans(true);
        //    }

        //    catch (Exception ex)
        //    {
        //        InfoMessage = main.strMessageDetail + ex.Message;
        //        IsInfoMBVisible = true;
        //        main.RollbackBulkTrans(true);
        //    }
        //}
    }
}
