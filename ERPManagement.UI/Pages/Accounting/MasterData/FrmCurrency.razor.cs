using ERPManagement.UI.Components.Base;
using ERPManagement.UI.Components.Base.Services;
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
        private Dictionary<string, Dictionary<object, string>>? fkLookups;
        private string formName = string.Empty;
        private List<string>? invisibleColumns;
        private DataTable dtCurrencies = new();
        private List<EINV_Currency> EINVcurrencies = new();
        private bool isArabic;
        private bool IsEnabled { get; set; }
        UserLoginData userData { get; set; }
        private SystemSettings systemSettings { get; set; } = new();
        Currency currentObject;
        #endregion

        #region Lifecycle Methods
        protected override async Task OnInitializedAsync()
        {
            //if (ConnectionString == null || ConnectionString == "")
            //{
            //    ConnectionString = await protectedLocalStorageService.GetConnectionStringAsync();
            //}
            //userData = await protectedLocalStorageService.GetUserDataAsync();
            //systemSettings = await protectedLocalStorageService.GetSystemSettingsAsync();

            myCustomActions = new ButtonActionsAdapter<Currency>(this, JS, ServiceProvider);
            myCustomNavigations = new ButtonNavigationsAdapter<Currency>(this);
            myCustomGrid = new GridHostAdapter<Currency>(this);

            await base.OnInitializedAsync();
            isArabic = IsArabic;
            formName = localizer["Currencies"];
            FormName = formName;

            invisibleColumns = new List<string> { "CurrencyID", "BranchID", "Deleted" };
            HiddenButtons.HideCopy = HiddenButtons.HidePrint = HiddenButtons.HideAddRoot = true;
            if (GlobalVariables.IsEINV)
            {
                EINVcurrencies = EcurrencyService.FillCombo(-1, true, new DataAccess.Main(ConnectionString), false);
                fkLookups = new()
                {
                    { "EINVCurrencyID", EINVcurrencies.ToDictionary(d => (object)d.EINVCurrencyID, d => d.EINVCurrencyName) }
                };
            }
            else
            {
                invisibleColumns.Add("EINVCurrencyID");
            }
            //OnInsert = model => currencyService.Insert_Update(model, CurrentUserID, new DataAccess.Main(ConnectionString), false);
            //OnUpdate = model =>
            //{
            //    currencyService.Insert_Update(model, CurrentUserID, new DataAccess.Main(ConnectionString), false);
            //    return true;
            //};
            //OnDelete = model =>
            //        {
            //            currencyService.Delete(((Currency)(object)model).CurrencyID, CurrentUserID, new DataAccess.Main(ConnectionString), false);
            //            return true;
            //        };

            OnInsert = InsertCurrency;
            OnUpdate = UpdateCurrency;
            OnDelete = DeleteCurrency;

            FillData();
        }
        #endregion

        #region Data Loading
        private void FillData()
        {
            dtCurrencies = currencyService.SelectDataTable(-1, "-1", true, new DataAccess.Main(ConnectionString), false);
            Data = dtCurrencies;
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
        private Task HandleRowSelected(DataRow row)
        {
            //myCustomGrid.RowSelected(row);
            if (row != null)
            {
                IsEnabled = false;
                CurrentObject = currencyService.MapRowToCurrency(row);
                OldObject = CurrentObject.Clone();
            }
            return Task.CompletedTask;
        }

        private int InsertCurrency(Currency model)
        {
            currencyService.Insert_Update(model, CurrentUserID, new DataAccess.Main(ConnectionString), false);
            return 1;
        }

        private bool UpdateCurrency(Currency model)
        {
            currencyService.Insert_Update(model, CurrentUserID, new DataAccess.Main(ConnectionString), false);

            return true;
        }

        private bool DeleteCurrency(Currency model)
        {
            currencyService.Delete(((Currency)(object)model).CurrencyID, CurrentUserID, new DataAccess.Main(ConnectionString), false);

            return true;
        }

        private Currency MapRowToCurrency(DataRow row)
        {
            return currencyService.MapRowToCurrency(row);
        }

        protected string CheckBeforeDelete(Currency entity)
        {
            if (GlobalVariables.LocalCurrencyID == entity.CurrencyID)
            {
                return IsArabic ? "لا يمكن حذف العملة المحلية" : "You cannot delete the local currency.";
            }
            return string.Empty;
        }



    }
}
