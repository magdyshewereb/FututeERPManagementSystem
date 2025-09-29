using ERPManagement.UI.Components.Base;
using ERPManagement.UI.DataAccess;
using ERPManagement.UI.DataModels.Accounting;
using ERPManagement.UI.DataModels.General;
using ERPManagement.UI.DataModels.HR;
using ERPManagement.UI.DataModels.ProtectedLocalStorage;
using ERPManagement.UI.DataModels.StockControl;
using ERPManagement.UI.GeneralClasses;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Data;
using static ERPManagement.UI.Components.Global.ModalDialog;
//using System.Data.Linq.SqlClient;

namespace ERPManagement.UI.Pages.Accounting
{
    public partial class FrmSubAccountsTree : ComponentBase
    {
        public string ConnectionString { get; set; }

        SubAccounts subAccObj = new SubAccounts();
        SubAccounts subAccSelectedObj;
        SubAccountsClientSupplier clientSupplierObj = new SubAccountsClientSupplier();
        //SubAccountsClientSupplier clientSupplierSelectedObj;
        Employees employeeObj = new Employees();
        //Employees employeeSelectedObj;

        DataTable dtSubAccDetails = new DataTable();
        DataTable dtLines = new DataTable();
        DataTable dtStores = new DataTable();
        DataTable dtPositions = new DataTable();
        DataTable dtSalesMan = new DataTable();
        DataTable dtEINVSubAccountsTypes = new DataTable();

        List<SuAccountsTypes> lstSubAccountTypes { get; set; } = new();
        List<PricesTypes> lstPriceTypes { get; set; } = new();
        List<Titles> lstTitles { get; set; } = new();
        List<SubAccounts_Classifications> lstSubAccountClassifications { get; set; } = new();
        List<PaymentMethods> lstPaymentMethods { get; set; } = new();
        List<Branch> lstBranchs { get; set; } = new();
        List<Account> lstAccounts { get; set; } = new();
        List<City> lstCities { get; set; } = new();
        List<Area> lstAreas { get; set; } = new();
        List<SubAccountsClientSupplierContacts> lstContacts { get; set; } = new();
        List<SubAccountsClientSupplierBanks> lstBanks { get; set; } = new();
        List<SubAccountsClientCards> lstCards { get; set; } = new();



        List<string> SubAccInvisibleColumns;
        List<string> ContactsInvisibleColumns;
        List<string> BanksInvisibleColumns;
        List<string> CardsInvisibleColumns;
        List<int> lstAccountIDs { get; set; } = new();
        private DataGrid<SubAccountsClientSupplierContacts> gridComponentContacts;
        private DataGrid<SubAccountsClientSupplierBanks> gridComponentBanks;
        private DataGrid<SubAccountsClientCards> gridComponentCards;
        Dictionary<string, Dictionary<string, string>> valueLists = new Dictionary<string, Dictionary<string, string>>();
        bool isNavMode = true;
        bool IsInfoMBVisible = false;
        bool IsQuesMBVisible = false;
        //public int NodeLevel;
        int confirmUpdatechildAcc = -1;
        GlobalVariables.States currentState = GlobalVariables.States.NavMode;
        UserLoginData userData { get; set; }
        private SystemSettings systemSettings { get; set; } = new();
        HiddenButtonsConfig hiddenButtons = new HiddenButtonsConfig();
        ModalDialogType MBType = ModalDialogType.Ok;
        string InfoMessage = "";

        bool IsAsset, IsSupplier, IsClient, IsEmployee;
        // confirmUpdatechildAcc is int to be able to return more than two state (1=true, 0=false,-1 is not confermed)
        string QuesMessage = "";
        //Tree Table column
        string IDCol = "SubAccountID";
        string NoCol = "SubAccountNumber";
        string NameCol = "SubAccountNameAr";
        string NameEnCol = "SubAccountNameEn";
        string ParentIDCol = "ParentID";
        string IsMainCol = "IsMain";
        string ItemLevelCol = "LevelID";
        string AdditionalCol1 = "SubAccountTypeID";
        string AdditionalCol2 = "BranchID";
        string AdditionalCol3 = "ForAllBranches";
        string TableName = "A_SubAccounts";
        //Levels Table
        string LevelsTable = "A_SubAccounts_Levels";
        string LevelsCol = "LevelID";
        string LevelsWidthCol = "Width";
        Main main;
        protected override async Task OnInitializedAsync()
        {
            SubAccInvisibleColumns = new List<string> { "SubAccountID", "ParentID", "IsMain", "LevelID", "Deleted", "ForAllBranches" };
            ContactsInvisibleColumns = new List<string> { "ClientSupplierContactID", "SubAccountID", "BranchID", "Deleted", "PositionSerializable" };
            BanksInvisibleColumns = new List<string> { "ClientSupplierBankID", "SubAccountID", "BranchID", "Deleted" };
            CardsInvisibleColumns = new List<string> { "SubAccountClientCardID", "SubAccountID", "BranchID", "Deleted" };
            hiddenButtons.HideCopy = hiddenButtons.HideNext = hiddenButtons.HidePrevious = hiddenButtons.HideSearch = true;


            if (ConnectionString == null || ConnectionString == "")
            {
                ConnectionString = await protectedLocalStorageService.GetConnectionStringAsync();
            }
            userData = await protectedLocalStorageService.GetUserDataAsync();
            systemSettings = await protectedLocalStorageService.GetSystemSettingsAsync();

            main = new Main(ConnectionString);
            lstAccounts = GlobalFunctions.GetListFromDataTable<Account>("TreeData_Select 'AccountID', 'AccountNumber', 'AccountNameAr', 'AccountNameEn', 'ParentID', 'IsMain', 'LevelID', '', '', '', '', 'A_Accounts', -1, 0", main);
            lstSubAccountTypes = subAccountsTypesService.Select(-1, "-1", systemSettings.IsArabic, main);
            lstBranchs = branchService.Select(-1, "-1", systemSettings.IsArabic ? 1 : 0, main);
            lstCities = cityService.Select(-1, "-1", systemSettings.IsArabic, main);
            lstAreas = areaService.Select(-1, "-1", systemSettings.IsArabic, main);
            lstPriceTypes = priceTypeService.Select(-1, "-1", systemSettings.IsArabic, main);
            lstSubAccountClassifications = subAccountClassificationService.Select(-1, "-1", systemSettings.IsArabic, main);
            lstPaymentMethods = paymentMethodService.Select(-1, "-1", systemSettings.IsArabic, main);
            lstTitles = titlesService.Select(-1, "-1", systemSettings.IsArabic, main);
            dtLines = linesService.FillCombo(systemSettings.IsArabic, main);
            dtStores = storeService.FillCombo(-1, -1, GetBranchesIDs(), systemSettings.IsArabic, main);
            dtPositions = positionService.FillCombo(systemSettings.IsArabic, main);
            dtSalesMan = subAccountsService.SelectBySubAccountTypeIDs(GlobalVariables.EmployeeSubAccountTypeIDs, systemSettings.IsArabic, main);
            dtEINVSubAccountsTypes = eInvoiceService.FillCombo(-1, systemSettings.IsArabic, main);
            valueLists.Add("SubAccountTypeID", lstSubAccountTypes.ToDictionary(c => c.SubAccountTypeID.ToString(), c => systemSettings.IsArabic ? c.SubAccountTypeNameAr : c.SubAccountTypeNameEn));
            valueLists.Add("BranchID", lstBranchs.ToDictionary(c => c.BranchID.ToString(), c => systemSettings.IsArabic ? c.BranchNameAr : c.BranchNameEn));
            valueLists.Add("PositionID", dtPositions.AsEnumerable().ToDictionary(row => row.Field<int>("PositionID").ToString(), row => row.Field<string>("PositionName")));
            valueLists.Add("SubAccountID", dtSalesMan.AsEnumerable().ToDictionary(row => row.Field<int>("SubAccountID").ToString(), row => row.Field<string>("SubAccountName")));
            valueLists.Add("EINVSubAccountTypeID", dtEINVSubAccountsTypes.AsEnumerable().ToDictionary(row => row.Field<int>("EINVSubAccountTypeID").ToString(), row => row.Field<string>("EINVSubAccountTypeName")));
        }
        private void ClearControls(SubAccounts item)
        {
            subAccObj = item;
            //if (currentState== GlobalVariables.States.Adding)
            //{
            subAccObj.SubAccountNumber = subAccountsService.GetCode(subAccSelectedObj == null || subAccSelectedObj.SubAccountID == 0 ? "Null" : subAccSelectedObj.SubAccountID.ToString(), main);
            clientSupplierObj.ClientSupplierNo = subAccountsClientSupplierService.GetCode(main);
            employeeObj.IsSalesMan = false;
            //}
            //if (isNavMode)
            //{
            //    subAccSelectedObj = null;
            //    clientSupplierSelectedObj = null;
            //    employeeSelectedObj = null;
            //}
            lstCards.Clear();
            lstBanks.Clear();
            lstContacts.Clear();
            confirmUpdatechildAcc = -1;
        }
        public void SetControls(GlobalVariables.States state)
        {
            currentState = state;
            isNavMode = currentState == GlobalVariables.States.NavMode;
            if (isNavMode)
            {
                subAccObj = subAccSelectedObj == null ? new SubAccounts() : subAccSelectedObj.Clone();
            }
        }
        private void DisplayData(SubAccounts item)
        {
            if (item != null)
            {
                item = subAccountsService.Select(item.SubAccountID, "-1", systemSettings.IsArabic, main).FirstOrDefault();
                subAccObj = item.Clone();
                subAccSelectedObj = item;
                cboSubAccountTypeValueChanges(); // to set invisible control 
                // NodeLevel = subAccSelectedObj.LevelID;
                dtSubAccDetails = subAccountsService.SelectBySubAccountIDWithAccountName(item.SubAccountID, systemSettings.IsArabic, main);
                if (IsEmployee)
                {
                    employeeObj = employeeService.SelectBySubAccountID(item.SubAccountID, "-1", systemSettings.IsArabic ? 1 : 0, main).FirstOrDefault();
                    //employeeObj = employeeSelectedObj != null ? employeeSelectedObj.Clone() : new Employees();
                }
                if (IsSupplier || IsClient)
                {
                    clientSupplierObj = subAccountsClientSupplierService.SelectBySubAccountID(item.SubAccountID, systemSettings.IsArabic ? 1 : 0, main).FirstOrDefault();
                    //clientSupplierObj = clientSupplierSelectedObj != null ? clientSupplierSelectedObj.Clone() : new SubAccountsClientSupplier();
                    lstContacts = contactService.SelectBySubAccountID(item.SubAccountID, true, main);
                    lstCards = cardService.SelectBySubAccountID(item.SubAccountID, true, main);
                    lstBanks = bankService.SelectBySubAccountID(item.SubAccountID, true, main);
                }
            }
            // Clear Account Tree And Add Parent Accounts
            lstAccountIDs.Clear();
            for (int i = 0; i < dtSubAccDetails.Rows.Count; i++)
            {
                lstAccountIDs.Add(int.Parse(dtSubAccDetails.Rows[i]["AccountID"].ToString()));
            }
        }

        private string HasTransactionValidations()
        {
            string Msg = subAccountsService.SelectRelations(subAccSelectedObj.SubAccountID.ToString(), systemSettings.IsArabic ? "1" : "0", main).Replace("-", "\n");
            if (Msg != "")
            {
                return Msg;
            }
            else
            {
                return "";
            }
        }
        public async Task<List<string>> ValidateData()
        {
            // lstMessages[0] mean the information message and lstMessages[1] mean the question message;
            List<string> lstMessages = new List<string> { "", "" };

            if (IsSupplier && clientSupplierObj.DefaultSupplierAccountID == null)
            {
                lstMessages[0] = systemSettings.IsArabic ? "من فضلك ادخل الحساب الإفتراضي للمورد" : "Enter Default Supplier Account";
                return lstMessages;
            }
            if (IsClient && clientSupplierObj.DefaultClientAccountID == null)
            {
                lstMessages[0] = systemSettings.IsArabic ? "من فضلك ادخل الحساب الإفتراضي للعميل" : "Enter Default Client Account";
                return lstMessages;
            }

            if (subAccObj.SubAccountTypeID == null)
            {
                lstMessages[0] = systemSettings.IsArabic ? "من فضلك نوع الحساب " : "Enter SubAccount Type";
                return lstMessages;
            }
            if (!(IsClient || IsSupplier || IsEmployee) && subAccObj.BranchID == null)
            {
                lstMessages[0] = systemSettings.IsArabic ? "من فضلك الفرع " : "Enter Branch";
                return lstMessages;
            }
            if (!clientSupplierObj.IsActive && clientSupplierObj.StopDate == null && (IsSupplier || IsClient))
            {
                lstMessages[0] = systemSettings.IsArabic ? "من فضلك ادخل تاريخ التوقف " : "Please Enter Stop Date";
                return lstMessages;
            }
            if (lstContacts.Count > 0 && (IsSupplier || IsClient))
            {
                for (int i = 0; i < lstContacts.Count; i++)
                {
                    if (lstContacts[i].ContactNo == null || lstContacts[i].ContactNo == "")
                    {
                        lstMessages[0] = systemSettings.IsArabic ? "من فضلك ادخل كود جهة الاتصال  " : "Please Enter Contact No of Contacts";
                        return lstMessages;
                    }
                    if (lstContacts[i].ContactName == null || lstContacts[i].ContactName == "")
                    {
                        lstMessages[0] = systemSettings.IsArabic ? "من فضلك ادخل اسم جهة الاتصال " : "Please Enter Contact Name of Contacts";
                        return lstMessages;
                    }
                }
            }
            if (currentState == GlobalVariables.States.Updating && subAccObj.IsMain && confirmUpdatechildAcc == -1)
            {
                lstMessages[1] = systemSettings.IsArabic ? "هل تريد ربط كل محتويات المجموعه بنفس الحسابات؟" : "Would you like to Bind All Group Contents With The Same Accounts?";
                return lstMessages;
            }
            return lstMessages;
        }
        private async Task ConfirmAction(bool isConfirm)
        {
            confirmUpdatechildAcc = isConfirm ? 1 : 0;
        }
        private async Task<int> TreeAddData(SubAccounts item)
        {
            int ID = 0;
            Main mainAdd = new Main(ConnectionString);
            mainAdd.StartBulkTrans(true);
            try
            {
                item.SubAccountID = -1;
                item.SubAccountNameEn = subAccObj.SubAccountNameEn == null ? subAccObj.SubAccountNameAr : subAccObj.SubAccountNameEn;
                item.ParentID = subAccSelectedObj == null || subAccSelectedObj.SubAccountID == null ? null : subAccSelectedObj.SubAccountID;
                item.LevelID = (subAccSelectedObj == null || subAccSelectedObj.LevelID == null ? 0 : subAccSelectedObj.LevelID) + 1;
                item.IsMain = false;
                //Save SubAccount
                ID = subAccountsService.Insert_Update(item, int.Parse(userData.UserID.ToString()), mainAdd);

                //Save AccountTree
                subAccountsDetailsService.Insert_UpdateByAccIDs(ID, GetAccountIDs(lstAccountIDs), "0", userData.BranchID, mainAdd);

                //Save SubAccountsClientSupplier
                if (IsSupplier || IsClient)
                {
                    clientSupplierObj.ClientSupplierID = -1;
                    clientSupplierObj.ClientSupplierNo = clientSupplierObj.ClientSupplierNo == null ? item.SubAccountNumber : clientSupplierObj.ClientSupplierNo;
                    clientSupplierObj.SubAccountID = ID;

                    subAccountsClientSupplierService.Insert_Update(clientSupplierObj, int.Parse(userData.UserID.ToString()), mainAdd);

                    //Save Contacts
                    if (lstContacts.Count > 0)
                    {
                        //for (int i = 0; i < lstContacts.Count; i++)
                        //{
                        //    lstContacts[i].ClientSupplierContactID = i;
                        //}
                        contactService.Insert_UpdateByTableXML(lstContacts, ID, int.Parse(userData.UserID.ToString()), int.Parse(userData.UserID.ToString()), mainAdd);
                    }
                    //Save Bank
                    if (lstBanks.Count > 0)
                    {
                        //for (int i = 0; i < lstBanks.Count; i++)
                        //{
                        //    lstBanks[i].ClientSupplierBankID = i;
                        //}
                        bankService.Insert_UpdateByTableXML(lstBanks, ID, int.Parse(userData.UserID.ToString()), int.Parse(userData.UserID.ToString()), mainAdd);
                    }

                    //Save Cards
                    if (lstCards.Count > 0)
                    {
                        //for (int i = 0; i < lstCards.Count; i++)
                        //{
                        //    lstCards[i].SubAccountClientCardID = i;
                        //}
                        cardService.Insert_UpdateByTableXML(lstCards, ID, int.Parse(userData.UserID.ToString()), int.Parse(userData.UserID.ToString()), mainAdd);
                    }
                }
                //Save Employee
                if (IsEmployee)
                {
                    employeeObj.EmployeeNo = subAccountsService.GetCode(subAccSelectedObj.SubAccountID == 0 ? "Null" : subAccSelectedObj.SubAccountID.ToString(), mainAdd);
                    employeeObj.SubAccountID = ID;
                    employeeObj.StateID = 1;
                    employeeObj.BranchID = employeeObj.BranchID == null ? int.Parse(userData.BranchID.ToString()) : employeeObj.BranchID;
                    employeeObj.IsMonthlySalary = true;
                    employeeService.Insert_Update(employeeObj, int.Parse(userData.UserID.ToString()), mainAdd);
                }
                mainAdd.EndBulkTrans(true);
            }
            catch (Exception ex)
            {
                InfoMessage = main.strMessageDetail + ex.Message;
                IsInfoMBVisible = true;
                mainAdd.RollbackBulkTrans(true);
                StateHasChanged();
                return 0;
            }
            return ID;
        }
        private async Task<int> TreeUpdateData(SubAccounts item)
        {

            Main mainUpdate = new Main(ConnectionString);
            mainUpdate.StartBulkTrans(true);
            try
            {
                item.SubAccountNameEn = subAccObj.SubAccountNameEn == "" ? subAccObj.SubAccountNameAr : subAccObj.SubAccountNameEn;
                item.ParentID = subAccSelectedObj.ParentID;
                item.LevelID = subAccSelectedObj.LevelID;
                item.BranchID = int.Parse(userData.BranchID.ToString());
                subAccountsService.Insert_Update(item, int.Parse(userData.UserID.ToString()), mainUpdate);

                //update AccountTree for SubAccount and its Childs if needed
                string AccountIDs = GetAccountIDs(lstAccountIDs);
                SaveAccountsToSubAccAndChildNades(item.SubAccountID, AccountIDs, confirmUpdatechildAcc, mainUpdate);

                //Save SubAccountsClientSupplier
                if (IsSupplier || IsClient)
                {
                    clientSupplierObj.ClientSupplierID = clientSupplierObj == null ? -1 : clientSupplierObj.ClientSupplierID;
                    clientSupplierObj.ClientSupplierNo = clientSupplierObj.ClientSupplierNo == null ? item.SubAccountNumber : clientSupplierObj.ClientSupplierNo;
                    clientSupplierObj.SubAccountID = subAccSelectedObj.SubAccountID;

                    subAccountsClientSupplierService.Insert_Update(clientSupplierObj, int.Parse(userData.UserID.ToString()), mainUpdate);

                    //Delete Contacts before save (old way)
                    //contactService.DeleteBySubAccountID(item.SubAccountID, int.Parse(userData.UserID.ToString()), mainUpdate);
                    //Save Contacts
                    if (lstContacts.Count > 0)
                    {
                        //for (int i = 0; i < lstContacts.Count; i++)
                        //{
                        //    lstContacts[i].ClientSupplierContactID = i;
                        //}
                        contactService.Insert_UpdateByTableXML(lstContacts, item.SubAccountID, int.Parse(userData.BranchID.ToString()), int.Parse(userData.UserID.ToString()), mainUpdate);
                    }
                    //Save Banks
                    if (lstBanks.Count > 0)
                    {
                        bankService.Insert_UpdateByTableXML(lstBanks, item.SubAccountID, int.Parse(userData.BranchID.ToString()), int.Parse(userData.UserID.ToString()), mainUpdate);
                    }
                    //Save Cards
                    if (lstCards.Count > 0)
                    {
                        cardService.Insert_UpdateByTableXML(lstCards, item.SubAccountID, int.Parse(userData.BranchID.ToString()), int.Parse(userData.UserID.ToString()), mainUpdate);
                    }
                }
                //Save Employee
                if (IsEmployee)
                {
                    employeeObj.EmployeeNo = subAccountsService.GetCode(subAccSelectedObj.SubAccountID == 0 ? "Null" : subAccSelectedObj.SubAccountID.ToString(), mainUpdate);
                    employeeObj.SubAccountID = subAccSelectedObj.SubAccountID;
                    employeeObj.BranchID = employeeObj.BranchID == null ? int.Parse(userData.BranchID.ToString()) : employeeObj.BranchID;
                    employeeService.Insert_Update2(employeeObj, int.Parse(userData.UserID.ToString()), mainUpdate);
                }
                mainUpdate.EndBulkTrans(true);
                return subAccSelectedObj.SubAccountID;
            }
            catch (Exception ex)
            {
                InfoMessage = ex.Message;
                IsInfoMBVisible = true;
                mainUpdate.RollbackBulkTrans(true);
                StateHasChanged();
                return 0;
            }
        }
        private void SaveAccountsToSubAccAndChildNades(int SubAccountID, string AccountIDs, int ConfirmUpdatechildAcc, Main mainParam)
        {
            subAccountsService.SaveAccountsToChilds(SubAccountID, AccountIDs, ConfirmUpdatechildAcc, int.Parse(userData.BranchID.ToString()), int.Parse(userData.UserID.ToString()), mainParam);
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    subAccountsDetailsService.DeleteBySubAccountID(int.Parse(dt.Rows[i]["SubAccountID"].ToString()), int.Parse(userData.UserID.ToString()), main);
            //    subAccountsDetailsService.Insert_UpdateByAccIDs(int.Parse(dt.Rows[i]["SubAccountID"].ToString()), AccountIDs, "0", userData.BranchID, main);
            //}
        }
        private void TreeDeleteData()
        {
            Main mainDelete = new Main(ConnectionString);
            mainDelete.StartBulkTrans(true);
            try
            {
                cardService.DeleteBySubAccountID(subAccSelectedObj.SubAccountID, int.Parse(userData.UserID.ToString()), mainDelete);
                bankService.DeleteBySubAccountID(subAccSelectedObj.SubAccountID, int.Parse(userData.UserID.ToString()), mainDelete);
                contactService.DeleteBySubAccountID(subAccSelectedObj.SubAccountID, int.Parse(userData.UserID.ToString()), mainDelete);
                subAccountsClientSupplierService.DeleteBySubAccountID(subAccSelectedObj.SubAccountID, int.Parse(userData.UserID.ToString()), mainDelete);
                subAccountsDetailsService.DeleteBySubAccountID(subAccSelectedObj.SubAccountID, int.Parse(userData.UserID.ToString()), mainDelete);
                subAccountsService.Delete(subAccSelectedObj.SubAccountID, int.Parse(userData.UserID.ToString()), mainDelete);
                mainDelete.EndBulkTrans(true);
                js.InvokeVoidAsync("TreeView.setHighlightDefault"); // for delete any node Highlight
            }
            catch (Exception ex)
            {
                InfoMessage = ex.Message;
                IsInfoMBVisible = true;
                mainDelete.RollbackBulkTrans(true);
                StateHasChanged();
            }
        }
        private void Cancel()
        {
            subAccObj = subAccSelectedObj == null ? new SubAccounts() : subAccSelectedObj.Clone();
            DisplayData(subAccObj);
        }
        private void cboSubAccountTypeValueChanges()
        {
            if (subAccObj.SubAccountTypeID == 1 || subAccObj.SubAccountTypeID == 6)
            {
                IsAsset = true;
                IsClient = IsSupplier = IsEmployee = false;
                if (!SubAccInvisibleColumns.Contains("BranchID")) SubAccInvisibleColumns.Add("BranchID");
            }
            else if (subAccObj.SubAccountTypeID == 7)
            {
                IsEmployee = IsClient = true;
                IsSupplier = IsAsset = false;
                SubAccInvisibleColumns.Remove("BranchID");
            }
            else if (subAccObj.SubAccountTypeID == 5)
            {
                IsClient = IsSupplier = true;
                IsAsset = IsEmployee = false;
                SubAccInvisibleColumns.Remove("BranchID");
            }
            else if (subAccObj.SubAccountTypeID == 2)
            {
                IsEmployee = true;
                IsClient = IsSupplier = IsAsset = false;
                SubAccInvisibleColumns.Remove("BranchID");
            }
            else if (subAccObj.SubAccountTypeID == 3)
            {
                IsSupplier = true;
                IsClient = IsEmployee = IsAsset = false;
                SubAccInvisibleColumns.Remove("BranchID");
            }
            else if (subAccObj.SubAccountTypeID == 4)
            {
                IsClient = true;
                IsSupplier = IsEmployee = IsAsset = false;
                SubAccInvisibleColumns.Remove("BranchID");
            }
        }

        private string GetBranchesIDs()
        {
            string IDs = "";
            for (int i = 0; i < lstBranchs.Count; i++)
            {
                IDs = IDs + "," + lstBranchs[i].BranchID;
            }
            return IDs;
        }
        private void ModalDialogClose()
        {
            IsInfoMBVisible = false;
        }
        ///////// *** Account Tab *** ////////////
        #region Account Tab 
        private string GetAccountIDs(List<int> IDs)
        {
            string accIDs = "";
            for (int i = 0; i < IDs.Count; i++)
            {
                if (!lstAccounts.Single(a => a.AccountID == IDs[i]).IsMain) // this condition for exclude Main Accounts
                {
                    accIDs += IDs[i].ToString() + ",";
                }
            }
            return "," + accIDs;
        }
        #endregion

        ///////// *** Details Tab *** ////////////
        #region Details Tab 
        private void TabClick(TabPage tab)
        {
            if (tab.Text == localizer["TabDetails"] && !isNavMode)
            {
                dtSubAccDetails.Rows.Clear();
                for (int i = 0; i < lstAccountIDs.Count; i++)
                {
                    DataRow dr = dtSubAccDetails.NewRow();

                    Account Acc = lstAccounts.Single(a => a.AccountID == lstAccountIDs[i]);
                    if (!Acc.IsMain)
                    {
                        dr["AccountID"] = Acc.AccountID;
                        dr["AccountName"] = systemSettings.IsArabic ? Acc.AccountNameAr : Acc.AccountNameEn;
                        dtSubAccDetails.Rows.Add(dr);
                    }
                }
                //valueLists.Remove("DefaultClientAccountID");
                //valueLists.Remove("DefaultSupplierAccountID");
                //valueLists.Add("DefaultClientAccountID", dtSubAccDetails.AsEnumerable().ToDictionary(row => row.Field<int>("AccountID").ToString(), row => row.Field<string>("AccountName")));
                //valueLists.Add("DefaultSupplierAccountID", dtSubAccDetails.AsEnumerable().ToDictionary(row => row.Field<int>("AccountID").ToString(), row => row.Field<string>("AccountName")));
            }
        }
        private void cboCityValueChanged(int id)
        {
            clientSupplierObj.CityID = id;
            lstAreas = lstAreas.Where(c => c.CityID == clientSupplierObj.CityID).ToList();
            //valueLists["AreaID"] = lstAreas.Where(c => c.CityID == clientSupplierObj.CityID).ToDictionary(c => c.AreaID.ToString(), c => systemSettings.IsArabic ? c.AreaNameAr : c.AreaNameEn);
        }
        #endregion
    }
}

