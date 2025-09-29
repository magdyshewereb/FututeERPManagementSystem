using ERPManagement.UI.DataModels.Accounting;
using ERPManagement.UI.DataModels.ProtectedLocalStorage;
using ERPManagement.UI.GeneralClasses;
using ERPManagement.UI.DataModels.CustomsClearence;
using ERPManagement.UI.DataModels.General;
using Microsoft.AspNetCore.Components;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using System.Data;
//using System.Data.Linq.SqlClient;

namespace ERPManagement.UI.Pages.Accounting
{
    public partial class FrmAccountsTree : ComponentBase
    {
        public string ConnectionString { get; set; }
        
        
        Account currentObject = new Account();
        Account selectedNode;
        List<AccountType> lstAccountTypes { get; set; } = new();

        List<string> invisibleColumns;
        Dictionary<string, Dictionary<string, string>> valueLists = new Dictionary<string, Dictionary<string, string>>();
        bool isNavMode = true;
        public int NodeLevel;
        GlobalVariables.States currentState = GlobalVariables.States.NavMode;
        UserLoginData userData { get; set; }
        private SystemSettings systemSettings { get; set; } = new();
        HiddenButtonsConfig hiddenButtons = new HiddenButtonsConfig();


        //Tree Table column
        string IDCol = "AccountID";
        string NoCol = "AccountNumber";
        string NameCol = "AccountNameAr";
        string NameEnCol = "AccountNameEn";
        string ParentIDCol = "ParentID";
        string IsMainCol = "IsMain";
        string ItemLevelCol = "LevelID";
        string AdditionalCol1 = "IsVisible";
        string AdditionalCol2 = "AccountTypeID";
        string TableName = "A_Accounts";
        //Levels Table
        string LevelsTable = "A_AccountLevels";
        string LevelsCol = "LevelID";
        string LevelsWidthCol = "Width";

        protected override async Task OnInitializedAsync()
        {
            invisibleColumns = new List<string> { "AccountID", "ParentID", "IsMain", "LevelID", "BranchID", "Deleted" };
            hiddenButtons.HideCopy = hiddenButtons.HideNext = hiddenButtons.HidePrevious = hiddenButtons.HideSearch = hiddenButtons.HideAddRoot = true;

            if (ConnectionString == null || ConnectionString == "")
            {
                ConnectionString = await protectedLocalStorageService.GetConnectionStringAsync();
            }
            userData = await protectedLocalStorageService.GetUserDataAsync();
            systemSettings = await protectedLocalStorageService.GetSystemSettingsAsync();
            lstAccountTypes = accountTypeService.Select(-1, "-1", systemSettings.IsArabic, new DataAccess.Main(ConnectionString));
            valueLists.Add("AccountTypeID", lstAccountTypes.ToDictionary(c => c.AccountTypeID.ToString(), c => systemSettings.IsArabic ? c.AccountTypeNameAr : c.AccountTypeNameEn));

        }
        private void PrepareData()
        {

        }

        private void ClearControls(Account item)
        {
            currentObject = item ;
            if (!isNavMode)
            {
                currentObject.AccountNumber  = accountService.GetCode(selectedNode.AccountID == 0 ? "Null" : selectedNode.AccountID.ToString(), new DataAccess.Main(ConnectionString));
                if (GlobalVariables.SeeingVisibleAccount)
                {
                    currentObject.IsVisible = selectedNode.IsVisible;
                }
                currentObject.AccountTypeID = selectedNode.AccountTypeID;
            }
            else
            {
                selectedNode = null;
            }
        }
        public void SetControls(GlobalVariables.States state)
        {
            currentState = state;
            isNavMode = currentState == GlobalVariables.States.NavMode;
            if (isNavMode)
            {
                currentObject = selectedNode.Clone();
            }
        }
        private void DisplayData(Account item)
        {
            if (item != null)       
            {
                item = accountService.Select(item.AccountID, "-1", systemSettings.IsArabic, new DataAccess.Main(ConnectionString)).FirstOrDefault();
                currentObject = item.Clone();
                selectedNode = item;
                NodeLevel = (int)selectedNode.LevelID;
            }

        }


        private async Task<int> TreeAddData(Account item)
        {
            item.AccountID = -1;
            item.AccountNameEn = currentObject.AccountNameEn == null ? currentObject.AccountNameAr : currentObject.AccountNameEn;
            item.ParentID = selectedNode.AccountID;
            item.LevelID = selectedNode.LevelID + 1;
            item.IsMain = NodeLevel == 0 ? true : false;
            item.BranchID = int.Parse(userData.BranchID.ToString());

            return accountService.Insert_Update(item, int.Parse(userData.UserID.ToString()), new DataAccess.Main(ConnectionString));
            //FillData();
        }
        private async Task<int> TreeUpdateData(Account item)
        {
            item.AccountNameEn = currentObject.AccountNameEn == "" ? currentObject.AccountNameAr : currentObject.AccountNameEn;
            item.ParentID = selectedNode.ParentID;
            item.LevelID = NodeLevel;
            item.BranchID = int.Parse(userData.BranchID.ToString());
            return accountService.Insert_Update(item, 1, new DataAccess.Main(ConnectionString));
        }
        private void TreeDeleteData()
        {
            accountService.Delete(selectedNode.AccountID, 1, new DataAccess.Main(ConnectionString));
            js.InvokeVoidAsync("TreeView.setHighlightDedault");

        }
        private void Cancel()
        {
            currentObject = selectedNode.Clone();
            isNavMode = true;
        }
        private void cboCountiesValueChanges()
        {
        }
    }
}

