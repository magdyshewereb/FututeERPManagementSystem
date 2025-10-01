using ERPManagement.Application.Shared.Enums;
using ERPManagement.UI.Components.Base.Services;
using ERPManagement.UI.Components.Base.Services.Buttons.Actions;
using ERPManagement.UI.Components.Base.Services.Buttons.Navigations;
using ERPManagement.UI.Components.Base.Services.Grid;
using ERPManagement.UI.DataModels.ProtectedLocalStorage;
using ERPManagement.UI.GeneralClasses;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System.Data;

namespace ERPManagement.UI.Components.Base
{
    public partial class PageLayoutComponent<TModel> : ComponentBase,
        IEntityFormActions<TModel>,
        IEntityFormNavigation<TModel>,
        IEntityFormGrid<TModel>
        where TModel : new()
    {

        #region Injections & Parameters
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] protected IJSRuntime JS { get; set; }

        [Parameter] public string FormName { get; set; }
        [Parameter] public bool IsArabic { get; set; }
        [Parameter] public FormState State { get; set; } = FormState.View;

        [Parameter] public HiddenButtonsConfig HiddenButtons { get; set; } = new();
        [Parameter] public DataTable Data { get; set; }
        [Parameter] public List<string> InvisibleColumns { get; set; }
        [Parameter] public IStringLocalizer Localizer { get; set; }
        [Parameter] public Dictionary<string, Dictionary<object, string>> ForeignKeyLookups { get; set; } = new();
        [Parameter] public RenderFragment BodyContent { get; set; }
        [Parameter] public EventCallback<DataRow> OnRowSelected { get; set; }
        public Func<DataRow, TModel> MapRowToModel { get; set; }
        public bool IsEnabled { get; set; }


        #endregion

        #region Properties
        public DataRow? SelectedRow { get; set; }
        public TModel CurrentObject { get; set; }
        public TModel OldObject { get; set; }
        public List<string> Errors { get; set; } = new();
        public List<string> ValidationErrors { get; set; } = new();

        //List<string> IEntityForm<TModel>.ValidationErrors { get => Errors; set => Errors = value; }
        protected string ConnectionString { get; set; }
        protected UserLoginData UserData { get; set; }
        protected SystemSettings SystemSettings { get; set; } = new();
        public int CurrentBranchID { get; set; }
        protected int CurrentUserID { get; set; }

        public Func<TModel, int>? OnInsert { get; set; }
        public Func<TModel, bool>? OnUpdate { get; set; }
        public Func<TModel, bool>? OnDelete { get; set; }
        public Func<TModel, string>? CheckBeforeDelete { get; set; }
        #endregion

        #region Adapters
        public IButtonActions<TModel>? ActionsAdapter { get; private set; }
        public IButtonNavigations<TModel>? NavigationsAdapter { get; private set; }
        public IGridHost<TModel>? GridAdapter { get; private set; }

        [Parameter] public IButtonActions<TModel>? ExternalActions { get; set; }
        [Parameter] public IButtonNavigations<TModel>? ExternalNavigations { get; set; }
        [Parameter] public IGridHost<TModel>? ExternalGrid { get; set; }

        #endregion

        #region Lifecycle
        protected override async Task OnInitializedAsync()
        {
            ////////////////// Connection & User Data /////////////////
            //if (string.IsNullOrEmpty(ConnectionString))
            //{
            //    ConnectionString = await protectedLocalStorageService.GetConnectionStringAsync();
            //}
            //UserData = await protectedLocalStorageService.GetUserDataAsync();
            //SystemSettings = await protectedLocalStorageService.GetSystemSettingsAsync();
            // IsArabic = systemSettings.IsArabic;

            if (string.IsNullOrEmpty(ConnectionString))
            {
                ConnectionString =
                 "Server=sql-server,12355;Database=ERP_SS;User Id=sa;Password=;TrustServerCertificate=True;";
                //"Server=.,12355;Database=ERP_SS;User Id=sa;Password=magdy1986;TrustServerCertificate=True;";
            }
            CurrentBranchID = 1;
            CurrentUserID = 3;
            IsArabic = true;
            /////////////////////////////////////////////////////////
            // Initialize Adapters 
            ActionsAdapter = ExternalActions ?? new ButtonActionsAdapter<TModel>(this as IEntityFormActions<TModel>, JS, ServiceProvider);
            NavigationsAdapter = ExternalNavigations ?? new ButtonNavigationsAdapter<TModel>(this as IEntityFormNavigation<TModel>);
            GridAdapter = ExternalGrid ?? new GridHostAdapter<TModel>(this as IEntityFormGrid<TModel>);
            /////////////////////////////////////////////////////////
            CurrentObject = new TModel();
            OldObject = new TModel();

        }
        #endregion

        public Action Refresh => StateHasChanged; // Refresh the UI (UI Render)


    }
}
