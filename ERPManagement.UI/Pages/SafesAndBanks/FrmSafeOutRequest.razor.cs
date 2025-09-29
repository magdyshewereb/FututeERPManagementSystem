using ERPManagement.UI.Components.Base;
using ERPManagement.UI.DataAccess;
using ERPManagement.UI.DataModels.ProtectedLocalStorage;
using ERPManagement.UI.DataModels.SafesAndBanks;
using ERPManagement.UI.GeneralClasses;
using ERPManagement.UI.DataModels.Accounting;
using ERPManagement.UI.DataModels.EInvoices;
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


namespace ERPManagement.UI.Pages.SafesAndBanks
{
    public partial class FrmSafeOutRequest : ComponentBase
    {
        public string ConnectionString { get; set; }
        DataRow drMaster;
        SafeOutRequests safeOutRequestObj = new SafeOutRequests();
        SafeOutRequests safeOutRequestSelectedObj;
        DataTable dtCurrency = new DataTable();
        List<SafeOutRequestDetails> lstSafeOutRequestDetails = new List<SafeOutRequestDetails>();
        DataTable dtSubAccounts = new DataTable(); //for sub accounts>
        DataTable dtCostCenters = new DataTable(); //for sub accounts>
        DataTable dtSafeOutRequests = new DataTable(); //for sub accounts>
        DataTable dtAccounts = new DataTable(); //for sub accounts>
        DataTable dtsafes = new DataTable(); //for sub accounts>

        DataSet ds = new DataSet();
        List<string> SafeOutRequestDetailsInvisibleColumns;
        private DataGrid<BankInDetails> gridComponentSafeOutRequestDetails;
        public int _SafeOutRequestID = 0;
        bool isNavMode = true;
        GlobalVariables.States currentState = GlobalVariables.States.NavMode;
        UserLoginData userData { get; set; }
        private SystemSettings systemSettings { get; set; } = new();
        HiddenButtonsConfig hiddenButtons = new HiddenButtonsConfig();
        Dictionary<string, Dictionary<string, string>> valueLists = new Dictionary<string, Dictionary<string, string>>();
        Dictionary<string, string> lstFilters = new Dictionary<string, string>();
        Dictionary<string, int> lstSearchWidth = new Dictionary<string, int>();
        Dictionary<string, int> lstDetailGridWidth = new Dictionary<string, int>();
        int CurrencyID;
        string RowID = "";
        //var of controles not saved
        bool IsRepeatedJV = false;
        bool UseSubCostCenters, UseSubAccounts, UseCostCenters;
        //modal box
        ModalDialogType MBType = ModalDialogType.Ok;
        int confirmUpdatechildAcc = -1;
        bool IsInfoMBVisible = false;
        bool IsSearchMDVisible = false;
        string InfoMessage = "";
        // confirmUpdatechildAcc is int to be able to return more than two state (1=true, 0=false,-1 is not confermed)
        string QuesMessage = "";
        DateTime SafeOutDate
        {
            get { return safeOutRequestObj.SafeOutRequestDate; }
            set
            {
                safeOutRequestObj.SafeOutRequestDate = value;
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

            SafeOutRequestDetailsInvisibleColumns = new List<string> {"IsDocumented","BranchName", "SafeOutRequestDetailID", "SafeOutRequestID", "BranchID", "Deleted", UseSubAccounts?"": "SubAccountID", UseSubCostCenters?"": "SubCostCenterID",
                                             UseCostCenters?"":"CostCenterID" };
            hiddenButtons.HideCopy = hiddenButtons.HideAddRoot = true;
            lstFilters.Add("SubAccountID", "AccountID");
            safeOutRequestObj.SafeOutRequestDate = DateTime.Now;
            lstSafeOutRequestDetails = safeOuRequestDetailsService.SelectBySafeOutRequestID(0, systemSettings.IsArabic, new Main(ConnectionString));
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
            //jvDetailsDefultObj.Debit=0;
            //jvDetailsDefultObj.Credit=0;
            //jvDetailsDefultObj.CurrencyID=1;
            //jvDetailsDefultObj.ExchangeRate= (decimal)dtCurrency.Select("CurrencyID=" + jvDetailsDefultObj.CurrencyID.ToString())[0]["ExchangeRate"];
            //jvDetailsDefultObj.Notes="";
            /////// for search  columns width *---*-*-*--------******
            lstSearchWidth.Add("SafeOutRequestNo", 12);
            lstSearchWidth.Add("SafeOutRequestDate", 12);
            lstSearchWidth.Add("CurrencyName", 12);
            lstSearchWidth.Add("IsRefused", 10);
            lstSearchWidth.Add("Approved", 10);
            lstSearchWidth.Add("BranchName", 14);
            lstSearchWidth.Add("Notes", 0);


            /////// for Detail Grid columns width *---*-*-*--------******
            lstDetailGridWidth.Add("AccountID", 20);
            lstDetailGridWidth.Add("SubAccountID", 20);
            lstDetailGridWidth.Add("CostCenterID", 20);
            lstDetailGridWidth.Add("SubCostCenterID", 10);
            lstDetailGridWidth.Add("Value", 10);
            //lstDetailGridWidth.Add("IsDocumented", 5);
            lstDetailGridWidth.Add("Notes", 0);
        }
        private void ClearControls()
        {
            safeOutRequestObj = new SafeOutRequests();
            safeOutRequestObj.SafeOutRequestDate = DateTime.Now;
            //if (currentState == GlobalVariables.States.Adding)
            //    safeOutRequestObj.Total = 0;
            //safeOutRequestObj.CollectionExpense = 0;
            //safeOutRequestObj.ExchangeRate = 0;
            safeOutRequestObj.SafeOutRequestNo = currentState == GlobalVariables.States.Adding ? safeOuRequestService.GetCodeByBranchID(safeOutRequestObj.SafeOutRequestDate.ToString("MM'/'dd'/'yyyy"), userData.BranchID, new Main(ConnectionString)) : "";
            lstSafeOutRequestDetails.Clear();
            //cboCurrency.SelectedIndex = -1;
            //cboCurrency.Clear();
        }
        public void SetControls(GlobalVariables.States state)
        {
            currentState = state;
            isNavMode = currentState == GlobalVariables.States.NavMode;
            if (currentState == GlobalVariables.States.Updating && UseSubAccounts)
            {
                for (int i = 0; i < lstSafeOutRequestDetails.Count; i++)
                {
                    if (lstSafeOutRequestDetails[i].AccountID != null)
                    {
                        //Filling the Sub Accounts UDD
                        int AccountID = lstSafeOutRequestDetails[i].AccountID;
                        if (UseSubAccounts)
                        {
                            valueLists.Remove("SubAccountID");
                            valueLists.Add("SubAccountID", dtSubAccounts.Select("AccountID=" + AccountID).ToDictionary(row => row.Field<int>("SubAccountID").ToString(), row => row.Field<string>("Name")));

                            if (valueLists["SubAccountID"].Count == 0)
                            {
                                lstSafeOutRequestDetails[i].SubAccountID = null;
                            }
                        }
                    }
                }
            }
        }
        private void DisplayData()
        {
            if (drMaster != null)
            {
                
                //bankInObj = bankInSelectedObj;
                //CanModifyOtherBranch = drMaster["BranchID"].ToString() == GlobalVariables.CurrentBranchID;
                safeOutRequestObj.SafeOutRequestNo = drMaster["SafeOutRequestNo"].ToString();
                safeOutRequestObj.SafeOutRequestDate = (DateTime)drMaster["SafeOutRequestDate"];
                safeOutRequestObj.CurrencyID = int.Parse(drMaster["CurrencyID"].ToString());
                safeOutRequestObj.Notes = drMaster["Notes"].ToString();
                lstSafeOutRequestDetails = safeOuRequestDetailsService.SelectBySafeOutRequestID(int.Parse(drMaster["SafeOutRequestID"].ToString()), systemSettings.IsArabic, new Main(ConnectionString));

            }
            else
            {
                ClearControls();
            }
        }
        private void FillData()
        {
            if (_SafeOutRequestID == -1)
            {

                drMaster = null;
               // btnAddClick();

                //btnAdd.Visible = false;
                //btnOK.Visible = false;
            }
            else if (_SafeOutRequestID > 0 || _SafeOutRequestID < -1)
            {
                DataTable dt = safeOuRequestService.Select(_SafeOutRequestID, userData.BranchID, systemSettings.IsArabic, new Main(ConnectionString));
                if (dt.Rows.Count > 0) drMaster = dt.Rows[0];
                else drMaster = null;
                //btnUpdateClick();
                //btnOK.Visible = false;
                DataRow dr = null;
           }
          else if (RowID == "")
                {
                    drMaster = null;
                }
                else
                {
                    DataTable dt = safeOuRequestService.Select(int.Parse(RowID), userData.BranchID, systemSettings.IsArabic, new Main(ConnectionString));
                    if (dt.Rows.Count > 0) drMaster = dt.Rows[0];
                    else drMaster = null;
                }
                DisplayData();
                if (CurrencyID > 0) safeOutRequestObj.CurrencyID = CurrencyID;
            //    if (IsRepeatedJV)
            //{
            //    //btnAddClick();

            //    return;
            //}

            //if (RowID == "")
            //{
            //    bankInSelectedObj = null;
            //}
            //else
            //{
            //    dr = safeOuRequestService.Select(int.Parse(RowID), userData.BranchID, systemSettings.IsArabic, new DataAccess.Main(ConnectionString)).Rows[0];
            //    //if (dr != null) bankInSelectedObj = new BankIn(dr);
            //    //else bankInSelectedObj = null;
            //}
            //DisplayData(dr);

        }
        private async Task FillData(int ID)
        {
            RowID = ID.ToString();
            DataTable dtSafeOuRequests = safeOuRequestService.Select(int.Parse(RowID), userData.BranchID, systemSettings.IsArabic, new Main(ConnectionString));
            //if (lst.Count > 0) bankInSelectedObj = lst[0];
            //else bankInSelectedObj = null;
            drMaster = dtSafeOuRequests != null ? dtSafeOuRequests.Rows[0] : null;
            DisplayData();
            IsSearchMDVisible = false;
        }
        private async Task FillData(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                dtSafeOutRequests = dt;
                RowID = dt.Rows[0]["SafeOutRequestID"].ToString();
                DataTable _dtSafeOutRequests = safeOuRequestService.Select(int.Parse(RowID), userData.BranchID, systemSettings.IsArabic, new Main(ConnectionString));
                //if (dtbankIns.Rows.Count > 0) bankInSelectedObj = lst[0];
                //else bankInSelectedObj = null;
                drMaster = _dtSafeOutRequests != null ? _dtSafeOutRequests.Rows[0] : null;
                DisplayData();
                IsSearchMDVisible = false;
            }
        }
        private void FillCurrencyDropDown()
        {
            dtCurrency = currencyService.FillCurrencyByDate(safeOutRequestObj.SafeOutRequestDate.ToString("MM'/'dd'/'yyyy"), systemSettings.IsArabic, new Main(ConnectionString));

            //if (UseCurrency)
            //{
            //    valueLists.Remove("CurrencyID");
            //    valueLists.Add("CurrencyID", dtCurrency.AsEnumerable().ToDictionary(row => row.Field<int>("CurrencyID").ToString(), row => row.Field<string>("CurrencyName")));
            //}
        }

        private async Task<List<string>> ValidateData()
        {
            List<string> lstMessages = new List<string> { "", "" };

            //if (!fiscalYearService.ChkForConfirmedFiscalYear(safeOutRequestObj.SafeOutRequestDate.ToString("MM'/'dd'/'yyyy"), new DataAccess.Main(ConnectionString)))
            //{
            //    lstMessages[0] = systemSettings.IsArabic ? "السنة المالية غير معتمدة" : "The Fiscal Year is not confirmed..";
            //    return lstMessages;
            //}
            //else if (fiscalYearService.ChkForClosingFsicalPeriod(safeOutRequestObj.SafeOutRequestDate.ToString("MM'/'dd'/'yyyy"), userData.BranchID, new DataAccess.Main(ConnectionString)))
            //{
            //    lstMessages[0] = systemSettings.IsArabic ? "لقد قمت بأختيار تاريخ يقع في فتره ماليه مغلقة" : "The Date you choosed\n\r exists in closed fisical period";
            //    return lstMessages;
            //}

            //else if (bankInObj.BankInNo == null || bankInObj.BankInNo == "")
            //{
            //    lstMessages[0] = systemSettings.IsArabic ? "برجاء إدخال كود القيد" : "Please Enter JV Code";
            //    return lstMessages;
            //}
            //else if (lstJVDetails.Count < 2)
            //{
            //    lstMessages[0] = systemSettings.IsArabic ? "برجاء إدخــال تفاصيل لهذا القيـــد" : "Please insert details for this JV";
            //    return lstMessages;
            //}

            //for (int i = 0; i < lstJVDetails.Count; i++)
            //{
            //    if (lstJVDetails[i].AccountID == null)
            //    {
            //        lstMessages[0] = systemSettings.IsArabic ? "برجاء ادخال اسم الحساب أو حذف السطر  " : "Please Enter Account Name or Delete Row ";
            //        return lstMessages;
            //    }
            //    if (lstJVDetails[i].ExchangeRate <= 0)
            //    {
            //        lstMessages[0] = systemSettings.IsArabic ? "سعر التحويل لابد ان يكون اكبر من الصفر" : "Exchange Rate Must Be Greater Than Zero";
            //        return lstMessages;
            //    }

            //    //Enforcing SubAccounts Choice
            //    if (UseSubAccounts) // && GlobalFunctions.GetOption("EnforceSubAccountsUse")
            //    {
            //        if (
            //            valueLists["SubAccountID"] != null
            //            && dtSubAccounts.Select("AccountID=" + lstJVDetails[i].AccountID).Length > 0
            //            && (lstJVDetails[i].SubAccountID == null || dtSubAccounts.Select(" SubAccountID = " + lstJVDetails[i].SubAccountID).Length == 0)
            //            )
            //        {
            //            lstMessages[0] = systemSettings.IsArabic ? "برجاء اختيار حساب تحليلي" : "Please choose SubAccount";
            //            return lstMessages;
            //        }
            //    }

            //    //Enforcing CostCenters Choice
            //    if (UseCostCenters)// && GlobalFunctions.GetOption("EnforceCostCentersUse")
            //    {
            //        if (
            //            dtAccounts.Select("AccountID = " + lstJVDetails[i].AccountID)[0]["AccountTypeID"].ToString() == "3"
            //        ||
            //            dtAccounts.Select("AccountID = " + lstJVDetails[i].AccountID)[0]["AccountTypeID"].ToString() == "4"
            //            )
            //        {
            //            if (lstJVDetails[i].CostCenterID == null)
            //            {
            //                lstMessages[0] = systemSettings.IsArabic ? "برجاء اختيار مركز تكلفة" : "Please choose Cost-Center";
            //                return lstMessages;
            //            }
            //        }
            //    }
            //}
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
                safeOutRequestObj.SafeOutRequestID = -1;
                safeOutRequestObj.IsRefused = false;
                safeOutRequestObj.Approved = false;
                safeOutRequestObj.BranchID = int.Parse(userData.BranchID);

                int ID = safeOuRequestService.Insert_Update(
                    safeOutRequestObj,int.Parse(userData.UserID), main);
                
                lstSafeOutRequestDetails.ForEach(x => x.SafeOutRequestDetailID = -1);
                safeOuRequestDetailsService.Insert_UpdateByTable(lstSafeOutRequestDetails, ID,int.Parse(userData.BranchID), userData.UserID, main);
                
                    main.EndBulkTrans(true);
                    RowID = ID.ToString();
                    // if (IsRepeatedJV) Close();
                    return true;
                
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
            //main.StartBulkTrans(true);
            //try
            //{
            //    int ID = jvService.GenerateJV_Update(jvObj, lstJVDetails, userData.BranchID, userData.UserID, new DataAccess.Main(ConnectionString));

            //    if (main.strMessageDetail != null && main.strMessageDetail.Length > 0)
            //    {
            //        InfoMessage = main.strMessageDetail;
            //        IsInfoMBVisible = true;
            //        main.RollbackBulkTrans(true);
            //        StateHasChanged();
            //        return false;
            //    }
            //    else
            //    {
            //        main.EndBulkTrans(true);
            //        return true;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    InfoMessage = main.strMessageDetail + ex.Message;
            //    IsInfoMBVisible = true;
            //    main.RollbackBulkTrans(true);
            //    StateHasChanged();
            return false;
            //}
        }
        private void Copy()
        {
        }
        private void Delete()
        {

        }
        private void btnBankSearch()
        {
        }
        private void btnViewClick()
        {
        }
        private async Task<DataTable> btnSearchClick()
        {
            Main main = new Main(ConnectionString);
            DataTable dtPeriod = new DataTable();
            dtPeriod = searchPeriodService.SelectByFormName(userData.UserID, "ERP.SafesAndBanks.Search.frmSafeOutRequestsSearchReport", main);
            DateTime MinDate = DateTime.Now.AddDays(-(dtPeriod.Rows.Count > 0 ? Convert.ToInt32(dtPeriod.Rows[0]["SearchPeriodDays"]) : 10000));
            DateTime MaxDate = DateTime.Now.AddYears(50);
            dtSafeOutRequests = safeOuRequestService.Search(userData.BranchID, MinDate.ToString("MM'/'dd'/'yyyy"), MaxDate.ToString("MM'/'dd'/'yyyy"), -1, false, systemSettings.IsArabic, new Main(ConnectionString));

            return dtSafeOutRequests;
        }
        private void btnChangeStatus()
        {

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
            dtSafeOutRequests = null;
            IsInfoMBVisible = false;
            IsSearchMDVisible = false;
        }
        private void ChangeNavMode(GlobalVariables.States state)
        {
            currentState = state;
            isNavMode = currentState == GlobalVariables.States.NavMode;
        }

        private BankInDetails CellUpdate(BankInDetails item, string col)
        {
            //if (col == "Debit")
            //{
            //    item.LocalDebit = item.Debit == null ? 0 : (decimal)item.Debit * item.ExchangeRate;
            //}
            //else if (col == "Credit")
            //{
            //    item.LocalCredit = item.Credit == null ? 0 : (decimal)item.Credit * item.ExchangeRate;
            //}
            //else if (col == "ExchangeRate" || col == "CurrencyID")
            //{
            //    item.LocalDebit = item.Debit == null ? 0 : (decimal)item.Debit * item.ExchangeRate;
            //    item.LocalCredit = item.Credit == null ? 0 : (decimal)item.Credit * item.ExchangeRate;
            //}
            //else if (col == "LocalDebit")
            //{
            //    item.Debit = item.LocalDebit == null ? 0 : item.LocalDebit / item.ExchangeRate;
            //}
            //else if (col == "LocalCredit")
            //{
            //    item.Credit = item.LocalCredit == null ? 0 : item.LocalCredit / item.ExchangeRate;
            //}
            //if ((col == "Credit") && (globalFunctions.GetPropertyValue(item, col) == null))
            //{
            //    globalFunctions.SetPropertyValue(item, col, 0);
            //}
            //if ((col == "Debit") && (globalFunctions.GetPropertyValue(item, col) == null))
            //{
            //    globalFunctions.SetPropertyValue(item, col, 0);
            //}
            //if ((col == "LocalCredit") && (globalFunctions.GetPropertyValue(item, col) == null))
            //{
            //    globalFunctions.SetPropertyValue(item, col, 0);
            //}
            //if ((col == "LocalDebit") && (globalFunctions.GetPropertyValue(item, col) == null))
            //{
            //    globalFunctions.SetPropertyValue(item, col, 0);
            //}
            //if ((col == "ExchangeRate") && (globalFunctions.GetPropertyValue(item, col) == null))
            //{
            //    globalFunctions.SetPropertyValue(item, col, 1.00m);
            //}
            
            return item;
        }
        private BankInDetails CellListChanged(BankInDetails item, string col)
        {
            if (UseSubAccounts && col == "AccountID")
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
        
        
       
        private void DateChanged()
        {
            FillCurrencyDropDown();

            if (currentState == GlobalVariables.States.Adding)
                safeOutRequestObj.SafeOutRequestNo = currentState == GlobalVariables.States.Adding ? safeOuRequestService.GetCodeByBranchID(safeOutRequestObj.SafeOutRequestDate.ToString("MM'/'dd'/'yyyy"), userData.BranchID, new Main(ConnectionString)) : "";
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
