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
    public partial class FrmCostCenterTree : ComponentBase
    {
        public string ConnectionString { get; set; }
        
        CostCenter currentObject = new CostCenter();
        CostCenter selectedNode;

        List<string> invisibleColumns;
        bool isNavMode = true;
        public int NodeLevel;
        GlobalVariables.States currentState = GlobalVariables.States.NavMode;
        UserLoginData userData { get; set; }
        private SystemSettings systemSettings { get; set; } = new();
        HiddenButtonsConfig hiddenButtons = new HiddenButtonsConfig();


        //Tree Table column
        string IDCol = "CostCenterID";
        string NoCol = "CostCenterNumber";
        string NameCol = "CostCenterNameAr";
        string NameEnCol = "CostCenterNameEn";
        string ParentIDCol = "ParentID";
        string IsMainCol = "IsMain";
        string ItemLevelCol = "LevelID";
        string TableName = "A_CostCenters";
        //Levels Table
        string LevelsTable = "A_CostCenterLevels";
        string LevelsCol = "LevelID";
        string LevelsWidthCol = "Width";

        protected override async Task OnInitializedAsync()
        {
            invisibleColumns = new List<string> { "CostCenterID", "ParentID", "IsMain", "LevelID", "BranchID", "Deleted" };
            hiddenButtons.HideCopy = hiddenButtons.HideNext = hiddenButtons.HidePrevious = hiddenButtons.HideSearch = true;
            if (ConnectionString == null || ConnectionString == "")
            {
                ConnectionString = await protectedLocalStorageService.GetConnectionStringAsync();
            }
            userData = await protectedLocalStorageService.GetUserDataAsync();
            systemSettings = await protectedLocalStorageService.GetSystemSettingsAsync();

        }
        private void PrepareData()
        {

        }

        private void ClearControls()
        {
            currentObject = new CostCenter();
            if (!isNavMode)
            {
                currentObject.CostCenterNumber = costCenterService.GetCode(selectedNode == null || selectedNode.CostCenterID == 0 ? "Null" : selectedNode.CostCenterID.ToString(), new DataAccess.Main(ConnectionString)).ToString();
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
        private void DisplayData(CostCenter item)
        {
            if (item != null)
            {
                item = costCenterService.Select(item.CostCenterID, "-1", systemSettings.IsArabic, new DataAccess.Main(ConnectionString)).FirstOrDefault();
                currentObject = item.Clone();
                selectedNode = item;
                NodeLevel = selectedNode.LevelID;
            }

        }


        private async Task<int> TreeAddData(CostCenter costCenter)
        {
            costCenter.CostCenterID = -1;
            costCenter.CostCenterNameEn = currentObject.CostCenterNameEn == null ? currentObject.CostCenterNameAr : currentObject.CostCenterNameEn;
            costCenter.ParentID = selectedNode == null ? null : selectedNode.CostCenterID;
            costCenter.LevelID = selectedNode == null ? 1 : selectedNode.LevelID + 1;
            costCenter.IsMain = selectedNode == null ? true :selectedNode.LevelID == 0 ? true : false;
            costCenter.BranchID = int.Parse(userData.BranchID.ToString());

            return costCenterService.Insert_Update(costCenter, int.Parse(userData.UserID.ToString()), new DataAccess.Main(ConnectionString));
            //FillData();
        }
        private async Task<int> TreeUpdateData(CostCenter costCenter)
        {
            costCenter.CostCenterNameEn = currentObject.CostCenterNameEn == "" ? currentObject.CostCenterNameAr : currentObject.CostCenterNameEn;
            costCenter.ParentID = selectedNode.ParentID;
            costCenter.LevelID = NodeLevel;
            costCenter.BranchID = int.Parse(userData.BranchID.ToString());
            return costCenterService.Insert_Update(currentObject, 1, new DataAccess.Main(ConnectionString));
        }
        private void TreeDeleteData()
        {
            costCenterService.Delete(selectedNode.CostCenterID, 1, new DataAccess.Main(ConnectionString));
            js.InvokeVoidAsync("TreeView.setHighlightDedault");

        }
        private void Cancel()
        {
            currentObject = selectedNode == null? new CostCenter() :  selectedNode.Clone();
            isNavMode = true;
        }
        private void cboCountiesValueChanges()
        {
        }
    }
}

