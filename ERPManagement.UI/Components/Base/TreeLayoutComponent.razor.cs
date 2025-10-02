using ERPManagement.Application.Shared.Enums;
using ERPManagement.UI.Components.Base.Services;
using ERPManagement.UI.Components.Base.Services.Buttons.Actions;
using ERPManagement.UI.Components.Base.Services.Tree;
using ERPManagement.UI.GeneralClasses;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System.Data;

namespace ERPManagement.UI.Components.Base
{
    public partial class TreeLayoutComponent<TModel> : ComponentBase,
        IEntityFormActions<TModel>,
        IEntityFormTree<TModel>

        where TModel : new()
    {
        #region Injections & Parameters
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] public IJSRuntime JS { get; set; }

        [Parameter] public string FormName { get; set; }
        [Parameter] public bool IsArabic { get; set; }
        [Parameter] public FormState State { get; set; } = FormState.View;
        [Parameter] public DataTable Data { get; set; }
        [Parameter] public HiddenButtonsConfig HiddenButtons { get; set; } = new();
        [Parameter] public IStringLocalizer Localizer { get; set; }
        [Parameter] public RenderFragment BodyContent { get; set; }

        // Events خاصة بالـ CRUD
        public Func<TModel, int>? OnInsert { get; set; }
        public Func<TModel, bool>? OnUpdate { get; set; }
        public Func<TModel, bool>? OnDelete { get; set; }
        public Func<TModel, string>? CheckBeforeDelete { get; set; }
        #endregion

        #region Properties
        public TModel CurrentObject { get; set; }
        public TModel OldObject { get; set; }
        public bool IsEnabled { get; set; }
        public Func<DataRow, TModel> MapRowToModel { get; set; }
        public DataRow? SelectedRow { get; set; }
        public List<string> ValidationErrors { get; set; } = new();
        public string ConnectionString { get; set; }
        public int CurrentBranchID { get; set; }
        public int CurrentUserID { get; set; }
        #endregion

        #region Adapters
        public IButtonActions<TModel>? ActionsAdapter { get; private set; }
        public ITreeHost<TModel>? TreeAdapter { get; private set; }

        [Parameter] public IButtonActions<TModel>? ExternalActions { get; set; }
        [Parameter] public ITreeHost<TModel>? ExternalTree { get; set; }
        #endregion

        #region Lifecycle
        protected override async Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                ConnectionString =
                    "Server=sql-server,12355;Database=ERP_SS;User Id=sa;Password=;TrustServerCertificate=True;";
            }
            CurrentBranchID = 1;
            CurrentUserID = 3;
            IsArabic = true;

            // Initialize Adapters
            ActionsAdapter = ExternalActions ?? new ButtonActionsAdapter<TModel>(this, JS, ServiceProvider);
            TreeAdapter = ExternalTree ?? new TreeHostAdapter<TModel>(this);

            CurrentObject = new TModel();
            OldObject = new TModel();
        }
        #endregion

        public Action Refresh => StateHasChanged;




    }
}
