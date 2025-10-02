using ERPManagement.Application.Shared.Enums;
using ERPManagement.UI.Components.Base.Services.Buttons.Actions;
using ERPManagement.UI.Components.Base.Services.Tree;
using ERPManagement.UI.DataModels.Accounting.MasterData.CostCenter;
using ERPManagement.UI.DataModels.ProtectedLocalStorage;
using ERPManagement.UI.GeneralClasses;
using Microsoft.AspNetCore.Components;

namespace ERPManagement.UI.Pages.Accounting.MasterData
{

    public partial class FrmCostCenter //: TreeLayoutComponent<CostCenter>
    {
        private IButtonActions<CostCenter>? myCustomActions;
        private ITreeHost<CostCenter>? myCustomTree;
        public string ConnectionString { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }

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
            //myCustomActions = new ButtonActionsAdapter<CostCenter>(this as IEntityFormActions<CostCenter>, JS, ServiceProvider);
            myCustomTree = new TreeHostAdapter<CostCenter>(this as ITreeHost<CostCenter>);

            invisibleColumns = new List<string> { "CostCenterID", "ParentID", "IsMain", "LevelID", "BranchID", "Deleted" };
            HiddenButtons.HideCopy = HiddenButtons.HideNext = HiddenButtons.HidePrevious = HiddenButtons.HideSearch = true;
            if (ConnectionString == null || ConnectionString == "")
            {
                ConnectionString = await protectedLocalStorageService.GetConnectionStringAsync();
            }
            userData = await protectedLocalStorageService.GetUserDataAsync();
            systemSettings = await protectedLocalStorageService.GetSystemSettingsAsync();
            //costCenters = costCenterService.Select(-1, "-1", true, new DataAccess.Main(ConnectionString));

        }
    }
}
