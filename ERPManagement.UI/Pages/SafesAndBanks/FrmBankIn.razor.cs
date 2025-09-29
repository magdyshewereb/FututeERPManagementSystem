using ERPManagement.UI.Components.Base;
using ERPManagement.UI.DataAccess;
using ERPManagement.UI.DataModels.Accounting;
using ERPManagement.UI.DataModels.ProtectedLocalStorage;
using ERPManagement.UI.DataModels.SafesAndBanks;
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


namespace ERPManagement.UI.Pages.SafesAndBanks
{
    public partial class FrmBankIn : ComponentBase
    {
        public string ConnectionString { get; set; }
        DataRow drMaster ;
        BankIn bankInObj = new BankIn();
        BankIn bankInSelectedObj;
        JV jvRepeatedObj;
        //JVDetails jvDetailsObj =new JVDetails();
        DataTable dtCurrency = new DataTable();
        List<JVDetails> lstJVDetails = new List<JVDetails>();
        List<BankInDetails> lstBankInDetails = new List<BankInDetails>();
        DataTable dtSubAccounts = new DataTable(); //for sub accounts>
        DataTable dtCostCenters = new DataTable(); //for sub accounts>
        DataTable dtJVs = new DataTable(); //for sub accounts>
        DataTable dtbankIns = new DataTable(); //for sub accounts>
        DataTable dtAccounts = new DataTable(); //for sub accounts>
        DataTable dtbanks = new DataTable(); //for sub accounts>
        DataTable dtsafes = new DataTable(); //for sub accounts>

        DataSet ds = new DataSet();
        List<string> BankInDetailsInvisibleColumns;
        private DataGrid<BankInDetails> gridComponentBankInDetails;

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
        string JV,JV2,JV3, lblCheckStatus;
        //modal box
        ModalDialogType MBType = ModalDialogType.Ok;
        int confirmUpdatechildAcc = -1;
        bool IsInfoMBVisible = false;
        bool IsSearchMDVisible = false;
        string InfoMessage = "";
        // confirmUpdatechildAcc is int to be able to return more than two state (1=true, 0=false,-1 is not confermed)
        string QuesMessage = "";
        DateTime BankInDate
        {
            get { return bankInObj.BankInDate; }
            set
            {
                bankInObj.BankInDate = value;
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

            BankInDetailsInvisibleColumns = new List<string> {"IsDocumented","BranchName", "BankInDetailsID", "BankInID", "BranchID", "Deleted", UseSubAccounts?"": "SubAccountID", UseSubCostCenters?"": "SubCostCenterID",
                                             UseCostCenters?"":"CostCenterID" };
            hiddenButtons.HideCopy = hiddenButtons.HideAddRoot = true;
            lstFilters.Add("SubAccountID", "AccountID");
            bankInObj.BankInDate = DateTime.Now;
            lstBankInDetails = bankInDetailsService.SelectByBankInID(0, systemSettings.IsArabic, new Main(ConnectionString));
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
            dtbanks = banksService.FillCombo(systemSettings.IsArabic, new Main(ConnectionString));
            dtsafes =safesService.FillComboBySafeIDs(GlobalVariables.SafeIDs,systemSettings.IsArabic, new Main(ConnectionString));

            // set jvDetails Default Values
            //jvDetailsDefultObj.Debit=0;
            //jvDetailsDefultObj.Credit=0;
            //jvDetailsDefultObj.CurrencyID=1;
            //jvDetailsDefultObj.ExchangeRate= (decimal)dtCurrency.Select("CurrencyID=" + jvDetailsDefultObj.CurrencyID.ToString())[0]["ExchangeRate"];
            //jvDetailsDefultObj.Notes="";
            /////// for search  columns width *---*-*-*--------******
            lstSearchWidth.Add("BankInNo", 10);
            lstSearchWidth.Add("BankInDate", 10);
            lstSearchWidth.Add("bankName", 10);
            lstSearchWidth.Add("CheckNo", 12);
            lstSearchWidth.Add("CheckDate", 10);
            lstSearchWidth.Add("CurrencyCode", 10);
            lstSearchWidth.Add("Total", 10);
            lstSearchWidth.Add("ChargedPerson", 10);
            lstSearchWidth.Add("Notes", 20);
            lstSearchWidth.Add("JVNo", 5);

            /////// for Detail Grid columns width *---*-*-*--------******
            lstDetailGridWidth.Add("AccountID", 20);
            lstDetailGridWidth.Add("SubAccountID", 20);
            lstDetailGridWidth.Add("CostCenterID", 20);
            lstDetailGridWidth.Add("SubCostCenterID", 10);
            lstDetailGridWidth.Add("Value", 10);
            lstDetailGridWidth.Add("IsDocumented", 5);
            lstDetailGridWidth.Add("Notes",0);
        }
        private void ClearControls()
        {
            bankInObj = new BankIn();
            bankInObj.BankInDate = DateTime.Now;
            if (currentState == GlobalVariables.States.Adding)
                bankInObj.Total = 0;
            bankInObj.CollectionExpense = 0;
            bankInObj.ExchangeRate = 0;
            bankInObj.BankInNo = currentState == GlobalVariables.States.Adding ? bankInService.GetCodeByBranchID(bankInObj.BankInDate.ToString("MM'/'dd'/'yyyy"), userData.BranchID,  new Main(ConnectionString)) : "";
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
        private void DisplayData(DataRow dr)
        {
            if (dr != null)
            {
                drMaster = dr;
                //bankInObj = bankInSelectedObj;
                //CanModifyOtherBranch = drMaster["BranchID"].ToString() == GlobalVariables.CurrentBranchID;
                bankInObj.BankInNo = drMaster["BankInNo"].ToString();
                bankInObj.BankInDate = (DateTime)drMaster["BankInDate"];
                bankInObj.CurrencyID = int.Parse(drMaster["CurrencyID"].ToString());
                bankInObj.ExchangeRate = decimal.Parse(drMaster["ExchangeRate"].ToString());
                bankInObj.IsCheck = bool.Parse(drMaster["IsCheck"].ToString());

                bankInObj.CheckNo = drMaster["CheckNo"].ToString();
                bankInObj.CheckDate = drMaster["CheckDate"]==DBNull.Value ? null : (DateTime)drMaster["CheckDate"];
                bankInObj.BankID = int.Parse(drMaster["BankID"].ToString());
                bankInObj.ChargedPerson = drMaster["ChargedPerson"].ToString();
                bankInObj.Notes = drMaster["Notes"].ToString();
                bankInObj.Total = decimal.Parse(drMaster["Total"].ToString());
                bankInObj.SenderBankExpense = decimal.Parse(drMaster["SenderBankExpense"].ToString());
                bankInObj.ReceivingBank = drMaster["ReceivingBank"].ToString();
                bankInObj.BinderSafeID = drMaster["BinderSafeID"] == DBNull.Value ? null : int.Parse(drMaster["BinderSafeID"].ToString()); //drMaster["BinderSafeID"];
                JV =  drMaster["JVNo"].ToString() ;
                JV2 = drMaster["JVNo2"].ToString();
                JV3 = drMaster["JVNo3"].ToString();
                //JV.Visible = (drMaster["JVNo"] == DBNull.Value ? false : true) && CanViewJV;
                //JV2.Visible = (drMaster["JVNo2"] == DBNull.Value ? false : true) && CanViewJV;
                //JV3.Visible = (drMaster["JVNo3"] == DBNull.Value ? false : true) && CanViewJV;
                lblCheckStatus = drMaster["CheckStatus"].ToString();
                //lblCheckStatus.Visible = (drMaster["CheckStatus"] == DBNull.Value ? false : true);
                //btnChangeStatus.Visible = (drMaster["CheckStatus"] == DBNull.Value ? false : true) && (bool.Parse(drMaster["Collected"].ToString()) == false && bool.Parse(drMaster["Returned"].ToString()) == false);
                //ClosedPeriod = BusinessLayer.Accounting.FiscalYear.ChkForClosingFsicalPeriod(dtpDate.DateTime.ToString(GlobalVariables.DateShortFormate), GlobalVariables.CurrentBranchID, false);

            }
            else
            {
                ClearControls();
            }
        }
        private void FillData()
        {
            DataRow dr=null;
            if (IsRepeatedJV)
            {
                //btnAddClick();
                
                return;
            }

            if (RowID == "")
            {
                bankInSelectedObj = null;
            }
            else
            {
                dr = bankInService.Select(int.Parse(RowID), userData.BranchID, systemSettings.IsArabic, new Main(ConnectionString)).Rows[0];
                //if (dr != null) bankInSelectedObj = new BankIn(dr);
                //else bankInSelectedObj = null;
            }
            DisplayData(dr);

        }
        private async Task FillData(int ID)
        {
            RowID = ID.ToString();
            DataTable dtBankIns = bankInService.Select(int.Parse(RowID), userData.BranchID, systemSettings.IsArabic, new Main(ConnectionString));
            //if (lst.Count > 0) bankInSelectedObj = lst[0];
            //else bankInSelectedObj = null;

            DisplayData(dtbankIns != null ? dtbankIns.Rows[0] : null);
            IsSearchMDVisible = false;
        }
        private async Task FillData(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                dtbankIns = dt;
                RowID = dt.Rows[0]["BankInID"].ToString();
                DataTable dtBankIns= bankInService.Select(int.Parse(RowID), userData.BranchID, systemSettings.IsArabic, new Main(ConnectionString));
                //if (dtbankIns.Rows.Count > 0) bankInSelectedObj = lst[0];
                //else bankInSelectedObj = null;

                DisplayData(dtbankIns!=null?dtbankIns.Rows[0]:null);
                IsSearchMDVisible = false;
            }
        }
        private void FillCurrencyDropDown()
        {
            dtCurrency = currencyService.FillCurrencyByDate(bankInObj.BankInDate.ToString("MM'/'dd'/'yyyy"), systemSettings.IsArabic, new Main(ConnectionString));

            if (UseCurrency)
            {
                valueLists.Remove("CurrencyID");
                valueLists.Add("CurrencyID", dtCurrency.AsEnumerable().ToDictionary(row => row.Field<int>("CurrencyID").ToString(), row => row.Field<string>("CurrencyName")));
            }
        }
        
        private async Task<List<string>> ValidateData()
        {
            List<string> lstMessages = new List<string> { "", "" };

            if (!fiscalYearService.ChkForConfirmedFiscalYear(bankInObj.BankInDate.ToString("MM'/'dd'/'yyyy"), new Main(ConnectionString)))
            {
                lstMessages[0] = systemSettings.IsArabic ? "السنة المالية غير معتمدة" : "The Fiscal Year is not confirmed..";
                return lstMessages;
            }
            else if (fiscalYearService.ChkForClosingFsicalPeriod(bankInObj.BankInDate.ToString("MM'/'dd'/'yyyy"), userData.BranchID, new Main(ConnectionString)))
            {
                lstMessages[0] = systemSettings.IsArabic ? "لقد قمت بأختيار تاريخ يقع في فتره ماليه مغلقة" : "The Date you choosed\n\r exists in closed fisical period";
                return lstMessages;
            }
            
            else if (bankInObj.BankInNo == null || bankInObj.BankInNo == "")
            {
                lstMessages[0] = systemSettings.IsArabic ? "برجاء إدخال كود القيد" : "Please Enter JV Code";
                return lstMessages;
            }
            else if (lstJVDetails.Count < 2)
            {
                lstMessages[0] = systemSettings.IsArabic ? "برجاء إدخــال تفاصيل لهذا القيـــد" : "Please insert details for this JV";
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
            //Main main = new Main(ConnectionString);
            //main.StartBulkTrans(true);
            //try
            //{
            //    int ID = jvService.GenerateJV_Insert(jvObj, lstJVDetails, userData.BranchID, userData.UserID, new DataAccess.Main(ConnectionString));

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
            //        RowID = ID.ToString();
            //        // if (IsRepeatedJV) Close();
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
        private void btnSearchClick()
        {
            Main main = new Main(ConnectionString);
            DataTable dtPeriod =new DataTable();
            dtPeriod = searchPeriodService.SelectByFormName(userData.UserID, "ERP.SafesAndBanks.Search.frmBankInSearchReport", main);
            DateTime MinDate = DateTime.Now.AddDays(-(dtPeriod.Rows.Count > 0 ? Convert.ToInt32(dtPeriod.Rows[0]["SearchPeriodDays"]) : 10000));
            DateTime MaxDate = DateTime.Now.AddYears(50);

            dtbankIns = bankInService.Search(userData.BranchID, MinDate.ToString("MM'/'dd'/'yyyy"), MaxDate.ToString("MM'/'dd'/'yyyy"), -1,-1,-1,-1, -1, 0, systemSettings.IsArabic, new Main(ConnectionString));
            
            IsSearchMDVisible = true;
        }
        private void btnChangeStatus()
        {
            
        }
        private void Next()
        {
            if (dtbankIns != null && dtbankIns.Rows.Count > 1)
            {
                if (bankInObj != null)
                {
                    for (int i = 1; i < dtbankIns.Rows.Count; i++)
                    {
                        //if (dtbankIns.Rows[i]["BankInID"].ToString() == jvObj.BankInID.ToString())
                        //{
                        //    RowID = dtbankIns.Rows[i - 1]["BankInID"].ToString();
                        //    FillData();
                        //    break;
                        //}
                    }
                }
                else
                {
                    RowID = dtbankIns.Rows[0]["BankInID"].ToString();
                    FillData();
                }
            }
            else
            {
                Main main = new Main(ConnectionString);
                DataTable dt = main.SelectNext("," + userData.BranchID + ",", "SB_BankIn", "BankInID", "BankInNo", "BankInDate", RowID, "", "1");
                if (dt.Rows.Count > 0)
                {
                    RowID = dt.Rows[0]["BankInID"].ToString();
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
            if (dtbankIns != null && dtbankIns.Rows.Count > 1)
            {
                if (bankInObj != null)
                {
                    for (int i = 0; i < dtbankIns.Rows.Count - 1; i++)
                    {
                        //if (dtbankIns.Rows[i]["BankInID"].ToString() == jvObj.BankInID.ToString())
                        //{
                        //    RowID = dtbankIns.Rows[i + 1]["BankInID"].ToString();
                        //    FillData();
                        //    break;
                        //}
                    }
                }
                else
                {
                    RowID = dtbankIns.Rows[dtbankIns.Rows.Count - 1]["BankInID"].ToString();
                    FillData();

                }
            }
            else
            {
                Main main = new Main(ConnectionString);
                DataTable dt = main.SelectNext("," + userData.BranchID + ",", "SB_BankIn", "BankInID", "BankInNo", "BankInDate", RowID, "", "0");
                if (dt.Rows.Count > 0)
                {
                    RowID = dt.Rows[0]["BankInID"].ToString();
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
            CalculateTotals();
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
            //jvObj.TotalDebit = TotalDebit;
            //jvObj.TotalCredit = TotalCredit;
            diff = TotalDebit - TotalCredit;
            StateHasChanged();
        }
        async Task chkCurrencyChecked()
        {
            bankInObj.IsCheck = false;
        }
        private void cboCurrencyValueChanged(int id)
        {
            if (id != -1)
            {
                decimal _ExchangeRate = decimal.Parse(((decimal)dtCurrency.Select("CurrencyID=" + id)[0]["ExchangeRate"]).ToString("0.##"));
                for (int i = 0; i < lstBankInDetails.Count; i++)
                {
                    //lstBankInDetails[i].CurrencyID = id;
                    //lstBankInDetails[i].ExchangeRate = _ExchangeRate;
                    CellUpdate(lstBankInDetails[i], "ExchangeRate");
                }
               
            }
        }
        private void DateChanged()
        {
            FillCurrencyDropDown();

            if (currentState == GlobalVariables.States.Adding)
                bankInObj.BankInNo = currentState == GlobalVariables.States.Adding ? bankInService.GetCodeByBranchID(bankInObj.BankInDate.ToString("MM'/'dd'/'yyyy"), userData.BranchID, new Main(ConnectionString)) : "";
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
