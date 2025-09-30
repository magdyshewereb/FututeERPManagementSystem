using ERPManagement.UI.Components.Base;
using ERPManagement.UI.Components.Base.Services;
using ERPManagement.UI.Components.Base.Services.Buttons;
using ERPManagement.UI.Components.Base.Services.Grid;
using ERPManagement.UI.DataModels.Accounting.MasterData.Currency;
using ERPManagement.UI.DataModels.EInvoices;
using ERPManagement.UI.DataModels.ProtectedLocalStorage;
using ERPManagement.UI.GeneralClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System.Data;

namespace ERPManagement.UI.Pages.Accounting.MasterData
{
    [Authorize(Roles = "ERP.Accounting.MasterData.frmCurrency")]
    public partial class FrmCurrency : PageLayoutComponent<Currency>
    {
        //private PageLayoutComponent<Currency> layout;
        private IButtonActions<Currency> myCustomActions;
        private IButtonNavigations<Currency> myCustomNavigations;
        private IGridHost<Currency> myCustomGrid;
        #region Injected Services
        [Inject] public IStringLocalizer<FrmCurrency> localizer { get; set; } = default!;
        [Inject] public DataModels.Accounting.MasterData.Currency.CurrencyService currencyService { get; set; } = default!;
        [Inject] public ERPManagement.UI.DataModels.EInvoices.CurrencyService EcurrencyService { get; set; } = default!;
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        #endregion

        #region Private Fields
        //private Dictionary<string, Dictionary<object, string>>? fkLookups;
        //private string formName = string.Empty;
        //private List<string>? invisibleColumns = new();
        //private DataTable dtCurrencies = new();
        private List<EINV_Currency> EINVcurrencies = new();
        //private bool isArabic = true;
        //protected string connectionString = string.Empty;
        //protected int currentBranchID;
        //protected int currentUserID;

        //private bool IsEnabled { get; set; }
        //UserLoginData userData { get; set; }
        private SystemSettings systemSettings { get; set; } = new();
        Currency currentObject;
        #endregion

        #region Lifecycle Methods
        protected override async Task OnInitializedAsync()
        {
            myCustomActions = new ButtonActionsAdapter<Currency>(this as IEntityForm<Currency>, JS, ServiceProvider);
            myCustomNavigations = new ButtonNavigationsAdapter<Currency>(this);
            myCustomGrid = new GridHostAdapter<Currency>(this);

            await base.OnInitializedAsync();
            Localizer = localizer;
            FormName = localizer["Currencies"];

            InvisibleColumns = new List<string> { "CurrencyID", "BranchID", "Deleted" };
            HiddenButtons.HideCopy = HiddenButtons.HidePrint = HiddenButtons.HideAddRoot = true;
            if (GlobalVariables.IsEINV)
            {
                EINVcurrencies = EcurrencyService.FillCombo(-1, true, new DataAccess.Main(base.ConnectionString), false);
                ForeignKeyLookups = new()
                {
                    { "EINVCurrencyID", EINVcurrencies.ToDictionary(d => (object)d.EINVCurrencyID, d => d.EINVCurrencyName) }
                };
            }
            else
            {
                InvisibleColumns.Add("EINVCurrencyID");
            }
            //OnInsert = model => currencyService.Insert_Update(model, CurrentUserID, new DataAccess.Main(connectionString), false);
            //OnUpdate = model =>
            //{
            //    currencyService.Insert_Update(model, CurrentUserID, new DataAccess.Main(connectionString), false);
            //    return true;
            //};
            //OnDelete = model =>
            //        {
            //            currencyService.Delete(((Currency)(object)model).CurrencyID, CurrentUserID, new DataAccess.Main(connectionString), false);
            //            return true;
            //        };

            OnInsert = InsertCurrency;
            OnUpdate = UpdateCurrency;
            OnDelete = DeleteCurrency;
            CheckBeforeDelete = CheckBeforeDeleteCurrency;
            MapRowToModel = MapRowToCurrency;

            FillData();
        }
        #endregion

        #region Data Loading
        private void FillData()
        {
            Data = currencyService.SelectDataTable(-1, "-1", true, new DataAccess.Main(base.ConnectionString), false);

            if (currentObject == null)
            {
                currentObject = new Currency
                {
                    BranchID = CurrentBranchID,
                    CurrencyID = -1
                };
            }
            base.CurrentObject = currentObject;
        }

        #endregion

        //#region CRUD Handlers
        //private Task HandleNew()
        //{
        //    IsEnabled = true;
        //    NewEntity();
        //    return Task.CompletedTask;
        //}

        //private Task HandleEdit()
        //{
        //    IsEnabled = true;
        //    EditEntity();

        //    return Task.CompletedTask;
        //}
        //private Task HandleSave()
        //{
        //    if (SelectedRow == null)
        //        return AddEntity();
        //    else
        //        return UpdateEntity();
        //}
        //private Task HandleSaveAndClose()
        //{
        //    IsEnabled = false;
        //    HandleSave();
        //    return SaveAndCloseEntity();
        //}
        //private Task HandleDelete()
        //{
        //    return DeleteEntity();
        //}

        //private Task HandleCancel()
        //{
        //    IsEnabled = false;
        //    CancelEntity();

        //    return Task.CompletedTask;
        //}
        //#endregion
        //private Task HandleRowSelected(DataRow row)
        //{
        //    if (row != null)
        //    {
        //        IsEnabled = false;
        //        CurrentObject = currencyService.MapRowToCurrency(row);
        //        OldObject = CurrentObject.Clone();
        //    }
        //    return Task.CompletedTask;
        //}


        private int InsertCurrency(Currency model)
        {
            currencyService.Insert_Update(model, CurrentUserID, new DataAccess.Main(base.ConnectionString), false);
            return 1;
        }

        private bool UpdateCurrency(Currency model)
        {
            currencyService.Insert_Update(model, CurrentUserID, new DataAccess.Main(base.ConnectionString), false);

            return true;
        }

        private bool DeleteCurrency(Currency model)
        {
            currencyService.Delete(((Currency)(object)model).CurrencyID, CurrentUserID, new DataAccess.Main(base.ConnectionString), false);

            return true;
        }

        private Currency MapRowToCurrency(DataRow row)
        {
            return currencyService.MapRowToCurrency(row);
        }

        protected string CheckBeforeDeleteCurrency(Currency model)
        {
            if (GlobalVariables.LocalCurrencyID == model.CurrencyID)
            {
                return IsArabic ? "لا يمكن حذف العملة المحلية" : "You cannot delete the local currency.";
            }
            return string.Empty;
        }



    }
}
