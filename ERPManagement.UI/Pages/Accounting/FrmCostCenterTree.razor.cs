using ERPManagement.UI.Components.Base;
using ERPManagement.UI.Components.Base.Services;
using ERPManagement.UI.Components.Base.Services.Buttons.Actions;
using ERPManagement.UI.DataModels.Accounting.MasterData.CostCenter;
using ERPManagement.UI.DataModels.ProtectedLocalStorage;
using ERPManagement.UI.GeneralClasses;
using Microsoft.AspNetCore.Components;

namespace ERPManagement.UI.Pages.Accounting
{
    public partial class FrmCostCenterTree : GridLayoutComponent<CostCenter>
    {
        [Inject] public CostCenterService costCenterService { get; set; } = default!;
        private IButtonActions<CostCenter>? myCustomActions;
        public string ConnectionString { get; set; }

        CostCenter currentObject = new CostCenter();

        CostCenter selectedNode;

        List<string> invisibleColumns;
        bool isNavMode = true;
        public int NodeLevel;
        GlobalVariables.States currentState = GlobalVariables.States.NavMode;
        UserLoginData userData { get; set; }
        private SystemSettings systemSettings { get; set; } = new();

        private List<CostCenter> costCenters = new List<CostCenter>();
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
            myCustomActions = new ButtonActionsAdapter<CostCenter>(this as IEntityFormActions<CostCenter>, JS, _ServiceProvider);
            base.OnInitializedAsync();
            InvisibleColumns = new List<string> { "CostCenterID", "ParentID", "IsMain", "LevelID", "BranchID", "Deleted" };
            HiddenButtons.HideCopy = HiddenButtons.HideNext = HiddenButtons.HidePrevious = HiddenButtons.HideSearch = true;

            costCenters = costCenterService.Select(-1, "-1", true, new DataAccess.Main(base.ConnectionString));

        }
    }
}
