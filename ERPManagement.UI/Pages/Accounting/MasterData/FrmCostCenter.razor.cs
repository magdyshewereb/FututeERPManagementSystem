using ERPManagement.Application.Shared.Enums;
using ERPManagement.UI.Components.Base;
using ERPManagement.UI.DataModels.Accounting;
using ERPManagement.UI.DataModels.ProtectedLocalStorage;
using ERPManagement.UI.GeneralClasses;

namespace ERPManagement.UI.Pages.Accounting.MasterData
{
    public partial class FrmCostCenter : PageLayoutComponent<CostCenter>//, IButtonActions<CostCenter>
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

        public FormState State { get; set; } = FormState.View;

        public HiddenButtonsConfig HiddenButtons { get; set; } = new();

        //HiddenButtonsConfig hiddenButtons = new HiddenButtonsConfig();
        List<CostCenter> costCenters = new List<CostCenter>();
        bool IsEnabled = false;
        ////Tree Table column
        //string IDCol = "CostCenterID";
        //string NoCol = "CostCenterNumber";


        //string NameCol = "CostCenterNameAr";
        //string NameEnCol = "CostCenterNameEn";
        //string ParentIDCol = "ParentID";
        //string IsMainCol = "IsMain";
        //string ItemLevelCol = "LevelID";
        //string TableName = "A_CostCenters";
        ////Levels Table
        //string LevelsTable = "A_CostCenterLevels";
        //string LevelsCol = "LevelID";
        //string LevelsWidthCol = "Width";

        protected override async Task OnInitializedAsync()
        {
            invisibleColumns = new List<string> { "CostCenterID", "ParentID", "IsMain", "LevelID", "BranchID", "Deleted" };
            HiddenButtons.HideCopy = HiddenButtons.HideNext = HiddenButtons.HidePrevious = HiddenButtons.HideSearch = true;
            if (ConnectionString == null || ConnectionString == "")
            {
                ConnectionString = await protectedLocalStorageService.GetConnectionStringAsync();
            }
            userData = await protectedLocalStorageService.GetUserDataAsync();
            systemSettings = await protectedLocalStorageService.GetSystemSettingsAsync();
            costCenters = costCenterService.Select(-1, "-1", true, new DataAccess.Main(ConnectionString));

        }
    }
}
