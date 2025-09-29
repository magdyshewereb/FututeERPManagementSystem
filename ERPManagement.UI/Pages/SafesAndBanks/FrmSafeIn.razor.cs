using ERPManagement.UI.Components.Base;
using ERPManagement.UI.DataAccess;
using ERPManagement.UI.DataModels.Accounting;
using ERPManagement.UI.DataModels.Accounting.MasterData.Currency;
using ERPManagement.UI.DataModels.EInvoices;
using ERPManagement.UI.DataModels.ProtectedLocalStorage;
using ERPManagement.UI.DataModels.SafesAndBanks;
using ERPManagement.UI.GeneralClasses;
using Microsoft.AspNetCore.Components;
using System.Data;
using static ERPManagement.UI.Components.Global.ModalDialog;

namespace ERPManagement.UI.Pages.SafesAndBanks
{
    public partial class FrmSafeIn : ComponentBase
    {
        public string ConnectionString { get; set; }
        UserLoginData userData { get; set; }
        private SystemSettings systemSettings { get; set; } = new();
        Main main;

        SafeIn safeInObj = new SafeIn();
        SafeIn safeInSelectedObj;
        SafeIn safeInRepeatedObj;
        SafeInDetails safeInDetailsObj = new SafeInDetails();
        DataTable dtCurrency = new DataTable();
        DataTable dtJVDefaults = new DataTable();
        List<SafeInDetails> lstSafeInDetails = new List<SafeInDetails>();
        DataTable dtSubAccounts = new DataTable(); //for sub accounts>
        DataTable dtCostCenters = new DataTable(); //for sub accounts>
        DataTable dtJVs = new DataTable(); //for sub accounts>
        DataTable dtAccounts = new DataTable(); //for sub accounts>
        DataSet ds = new DataSet();
        List<string> JVDetailsInvisibleColumns;
        private DataGrid<JVDetails> gridComponentJVDetails;
        bool isNavMode = true;

        Currency currentObject = new Currency();
        string SearchCode = "";
        string formName = "";
        int lstcount = 0;
        List<Currency> currencies;
        List<string> invisibleColumns;
        Dictionary<string, Dictionary<string, string>> valueLists = new Dictionary<string, Dictionary<string, string>>();
        List<EINV_Currency> EINVcurrencies;
        bool isNaveMode = true;
        GlobalVariables.States currentState = GlobalVariables.States.NavMode;
        bool isModalVisible;
        HiddenButtonsConfig hiddenButtons = new HiddenButtonsConfig();
        //modal box
        ModalDialogType MBType = ModalDialogType.Ok;
        int confirmUpdatechildAcc = -1;
        bool IsInfoMBVisible = false;
        bool IsSearchMDVisible = false;
        string InfoMessage = "";
        // confirmUpdatechildAcc is int to be able to return more than two state (1=true, 0=false,-1 is not confermed)
        string QuesMessage = "";
        Dictionary<string, int> lstSearchWidth = new Dictionary<string, int>();

        DateTime jvDate
        {
            get { return safeInObj.SafeInDate; }
            set
            {
                safeInObj.SafeInDate = value;
                DateChanged();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            if (ConnectionString == null || ConnectionString == "")
            {
                ConnectionString = await protectedLocalStorageService.GetConnectionStringAsync();
            }
            userData = await protectedLocalStorageService.GetUserDataAsync();
            systemSettings = await protectedLocalStorageService.GetSystemSettingsAsync();
            main = new Main(ConnectionString);

            invisibleColumns = new List<string> { "CurrencyID", "BranchID", "Deleted" };
            hiddenButtons.HideCopy = hiddenButtons.HideNext = hiddenButtons.HidePrevious = hiddenButtons.HideSearch = hiddenButtons.HideAddRoot = true;

            if (GlobalVariables.IsEINV)
            {
                EINVcurrencies = EcurrencyService.FillCombo(-1, true, new Main(ConnectionString), false);
                var EINVCurr = EINVcurrencies.Select(x => new
                {
                    Id = x.EINVCurrencyID,
                    Name = x.EINVCurrencyName
                });
                valueLists.Add("EINVCurrencyID", EINVcurrencies.ToDictionary(c => c.EINVCurrencyID.ToString(), c => c.EINVCurrencyName));
            }
            else
            {
                invisibleColumns.Add("EINVCurrencyID");
            }

            currencies = currencyService.Select(-1, "-1", true, new Main(ConnectionString), false);
            lstcount = currencies.Count;



            //PageNameService.CurrentPageName = localizer["Currencies"];
            formName = localizer["SafeIn"];
        }

        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    if (firstRender)
        //    {
        //        if (ConnectionString == "")
        //        {
        //            ConnectionString = await protectedLocalStorageService.GetConnectionStringAsync();
        //        }
        //        if (GlobalVariables.IsEINV)
        //        {
        //            EINVcurrencies = EcurrencyService.FillCombo(-1, true, new DataAccess.Main(ConnectionString), false);
        //            var EINVCurr = EINVcurrencies.Select(x => new
        //            {
        //                Id = x.EINVCurrencyID,
        //                Name = x.EINVCurrencyName
        //            });
        //            valueLists.Add("EINVCurrencyID", EINVcurrencies.ToDictionary(c => c.EINVCurrencyID.ToString(), c => c.EINVCurrencyName));
        //        }
        //        else
        //        {
        //            invisibleColumns.Add("EINVCurrencyID");
        //        }

        //        currencies = currencyService.Select(-1, "-1", true, new DataAccess.Main(ConnectionString), false);
        //        lstcount = currencies.Count;
        //        StateHasChanged();
        //    }
        //}
        //protected override void OnParametersSet()
        //{
        //    //  ConnectionString = appState.ConnectionString;
        //    if (ConnectionString != "")
        //    {


        //    }
        //}
        public void ChangeNavMode(GlobalVariables.States state)
        {
            currentState = state;
            isNaveMode = currentState == GlobalVariables.States.NavMode;
        }
        private void HandleItemSelected(object item)
        {
            //if (item is Currency)
            //    currentObject = (Currency)item;
        }
        private async Task<bool> Add()
        {
            try
            {
                if (ValidateData())
                {
                    currencyService.Insert_Update(currentObject, 1, new Main(ConnectionString), false);
                    FillData();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }

        }

        private bool ValidateData()
        {
            if (string.IsNullOrEmpty(currentObject.CurrencyNameAr))
            {

                //ValidationMB.MessageBody = "Please Set Arabic Name";
                //ValidationMB.IsVisible = true;
                return false;
            }
            return true;
        }
        private void DateChanged()
        {
            FillCurrencyDropDown();

            //if (currentState == GlobalVariables.States.Adding)
            //    safeInObj.BankInNo = currentState == GlobalVariables.States.Adding ? safeInService.GetCodeByBranchID(safeInObj.BankInDate.ToString("MM'/'dd'/'yyyy"), userData.BranchID, new DataAccess.Main(ConnectionString)) : "";
        }
        private void FillCurrencyDropDown()
        {
            //dtCurrency = currencyService.FillCurrencyByDate(bankInObj.BankInDate.ToString("MM'/'dd'/'yyyy"), systemSettings.IsArabic, new DataAccess.Main(ConnectionString));

            //if (UseCurrency)
            //{
            //    valueLists.Remove("CurrencyID");
            //    valueLists.Add("CurrencyID", dtCurrency.AsEnumerable().ToDictionary(row => row.Field<int>("CurrencyID").ToString(), row => row.Field<string>("CurrencyName")));
            //}
        }
        private void FillData()
        {
            currencies = currencyService.Select(-1, "-1", true, new Main(ConnectionString), false);

        }
        private void ClearControls()
        {
            ////currentObject = new Currency();
            ////currentObject.BranchID = 1;
            ////currentObject.CurrencyID = -1;
        }
        private void Copy()
        {



        }
        private void Delete()
        {
            //currencyService.Delete(currentObject.CurrencyID, 1, new DataAccess.Main(ConnectionString), false);
            //FillData();
            //currentObject = new Currency();
        }
        private void Next()
        {

        }
        private void Previous()
        {

        }
        private void Search()
        {

        }
        private async Task<bool> Update()
        {
            try
            {
                currencyService.Insert_Update(currentObject, 1, new Main(ConnectionString), false);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private void Close()
        {
            dtJVs = null;
            IsInfoMBVisible = false;
            IsSearchMDVisible = false;
        }
    }
}
