using ERPManagement.UI.DataAccess;
using ERPManagement.UI.DataModels.ProtectedLocalStorage;
using ERPManagement.UI.GeneralClasses;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System.Data;
using System.Text.Json;
using static ERPManagement.UI.Components.Global.ModalDialog;


namespace ERPManagement.UI.Components.Base
{
    //TreeComponent<TItem, Tlevel> : ComponentBase where TItem : new() where Tlevel : new()
    public partial class FrmHeaderDetails<TItem, TItemDetails>
        : ComponentBase where TItem : new() where TItemDetails : new()
    {
        public string ConnectionString { get; set; }
        [Parameter]
        public RenderFragment HeaderContent { get; set; }
        [Parameter]
        public RenderFragment FooterContent { get; set; }
        [Parameter]
        public IStringLocalizer localizer { get; set; }
        [Parameter]
        public bool isNavMode { get; set; }
        GlobalVariables.States currentState = GlobalVariables.States.NavMode;
        UserLoginData userData { get; set; }
        private SystemSettings systemSettings { get; set; } = new();
        HiddenButtonsConfig hiddenButtons = new HiddenButtonsConfig();
        [Parameter]
        public DataRow? DrMaster { get; set; }
        [Parameter]
        public string RowID { get; set; } = "";
        [Parameter]
        public TItem ItemObj { get; set; } = new TItem();
        [Parameter]
        public string DateCol { get; set; }
        [Parameter]
        public string IDCol { get; set; }
        [Parameter]
        public string NoCol { get; set; }
        [Parameter]
        public string TableName { get; set; }
        [Parameter]
        public string FormName { get; set; } // page title
        [Parameter]
        public EventCallback<TItem> OnClearControlsCallback { get; set; }
        [Parameter]
        public EventCallback<GlobalVariables.States> OnSetControlsCallback { get; set; }
        [Parameter]
        public EventCallback<DataRow> OnDisplayDataCallback { get; set; }
        [Parameter]
        public Func<Task<bool>> AddFun { get; set; }
        [Parameter]
        public Func<Task<bool>> UpdateFun { get; set; }
        [Parameter]
        public EventCallback OnDeleteCallBack { get; set; }
        [Parameter]
        public EventCallback<TItem> OnCancelCallBack { get; set; }
        //[Parameter]
        //public string SearchFormFullName { get; set; }



        // parameters for shearch component
        #region  shearch component
        [Parameter]
        public bool IsSearchMDVisible { get; set; } = false;
        [Parameter]
        public DataTable dtSearch { get; set; }
        [Parameter]
        public string FormFullName { get; set; }
        [Parameter]
        public Func<Task<DataTable>> SearchFun { get; set; }
        [Parameter]
        public EventCallback<DataTable> OnSearchDubleClickCallback { get; set; }
        [Parameter]
        public EventCallback<int> OnSearchOkCallback { get; set; }
        [Parameter]
        public Dictionary<string, int> lstSearchWidth { get; set; }
        #endregion
        // Parameters for Grid component\
        #region Grid component
        [Parameter]
        public TItemDetails itemDetailsDefultObj { get; set; } = new TItemDetails();
        [Parameter]
        public List<TItemDetails> lstDetails { get; set; } = new List<TItemDetails>();
        [Parameter]
        public Dictionary<string, Dictionary<string, string>> valueLists { get; set; }//= new Dictionary<string, Dictionary<string, string>>();

        [Parameter]
        public Dictionary<string, string> lstFilters { get; set; }
        [Parameter]
        public DataSet ds { get; set; }
        [Parameter]
        public Dictionary<string, int> lstDetailGridWidth { get; set; } = new Dictionary<string, int>();
        [Parameter]
        public List<string> DetailsInvisibleColumns { get; set; }
        private DataGrid<TItemDetails> gridComponentDetails; // as refrerence to grid component
        #endregion
        //var of controles not saved
        //modal box
        #region modal box
        [Parameter]
        public ModalDialogType MBType { get; set; } = ModalDialogType.Ok;
        int confirmUpdatechildAcc = -1;
        [Parameter]
        public bool IsInfoMBVisible { get; set; } = false;
        [Parameter]
        public string InfoMessage { get; set; } = "";
        // confirmUpdatechildAcc is int to be able to return more than two state (1=true, 0=false,-1 is not confermed)
        [Parameter]
        public string QuesMessage { get; set; } = "";
        #endregion
        DateTime Date
        {
            get
            {
                var prop = ItemObj?.GetType().GetProperty(DateCol);
                if (prop == null) return default;

                var value = prop.GetValue(ItemObj, null)?.ToString();
                return DateTime.TryParse(value, out var result) ? result : default;
            }
            set
            {
                var prop = ItemObj?.GetType().GetProperty(DateCol);
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(ItemObj, value);
                }

                DateChanged();
            }
        }
        Main main;
        protected override async Task OnInitializedAsync()
        {
            if (ConnectionString == null || ConnectionString == "")
            {
                ConnectionString = await protectedLocalStorageService.GetConnectionStringAsync();
            }
            userData = await protectedLocalStorageService.GetUserDataAsync();
            systemSettings = await protectedLocalStorageService.GetSystemSettingsAsync();
            main = new Main(ConnectionString);

            hiddenButtons.HideCopy = hiddenButtons.HideAddRoot = true;
            globalFunctions.SetPropertyValue(ItemObj, DateCol, DateTime.Now);
        }
        public void SetControls(GlobalVariables.States state)
        {
            OnSetControlsCallback.InvokeAsync(state);
            if (currentState == GlobalVariables.States.Updating)
            {
                OnDisplayDataCallback.InvokeAsync(DrMaster);
            }
            currentState = state;
            isNavMode = currentState == GlobalVariables.States.NavMode;
        }
        private void ClearControls()
        {
            ItemObj = new TItem();
            if (!isNavMode)
            {
                OnClearControlsCallback.InvokeAsync(ItemObj);
            }
        }

        private async Task FillData(string ID)
        {
            RowID = ID;
            await OnSearchOkCallback.InvokeAsync(int.Parse(RowID));
            IsSearchMDVisible = false;
        }
        private async Task FillData(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                dtSearch = dt;
                RowID = dt.Rows[0][IDCol].ToString();
                await OnSearchDubleClickCallback.InvokeAsync(dtSearch);
                IsSearchMDVisible = false;
            }
        }
        private async void btnSearchClick()
        {
            dtSearch = await SearchFun.Invoke();

            IsSearchMDVisible = true;
        }
        private async Task<List<string>> ValidateData()
        {
            List<string> lstMessages = new List<string> { "", "" };

            //if (!fiscalYearService.ChkForConfirmedFiscalYear(bankInObj.BankInDate.ToString("MM'/'dd'/'yyyy"), new DataAccess.Main(ConnectionString)))
            //{
            //    lstMessages[0] = systemSettings.IsArabic ? "السنة المالية غير معتمدة" : "The Fiscal Year is not confirmed..";
            //    return lstMessages;
            //}
            //else if (fiscalYearService.ChkForClosingFsicalPeriod(bankInObj.BankInDate.ToString("MM'/'dd'/'yyyy"), userData.BranchID, new DataAccess.Main(ConnectionString)))
            //{
            //    lstMessages[0] = systemSettings.IsArabic ? "لقد قمت بأختيار تاريخ يقع في فتره ماليه مغلقة" : "The Date you choosed\n\r exists in closed fisical period";
            //    return lstMessages;
            //}

            //else if (bankInObj.BankInNo == null || bankInObj.BankInNo == "")
            //{
            //    lstMessages[0] = systemSettings.IsArabic ? "برجاء إدخال كود القيد" : "Please Enter JV Code";
            //    return lstMessages;
            //}
            //else if (lstJVDetails.Count < 2)
            //{
            //    lstMessages[0] = systemSettings.IsArabic ? "برجاء إدخــال تفاصيل لهذا القيـــد" : "Please insert details for this JV";
            //    return lstMessages;
            //}

            //for (int i = 0; i < lstJVDetails.Count; i++)
            //{
            //    if (lstJVDetails[i].AccountID == null)
            //    {
            //        lstMessages[0] = systemSettings.IsArabic ? "برجاء ادخال اسم الحساب أو حذف السطر  " : "Please Enter Account Name or Delete Row ";
            //        return lstMessages;
            //    }
            //    if (lstJVDetails[i].ExchangeRate <= 0)
            //    {
            //        lstMessages[0] = systemSettings.IsArabic ? "سعر التحويل لابد ان يكون اكبر من الصفر" : "Exchange Rate Must Be Greater Than Zero";
            //        return lstMessages;
            //    }

            //    //Enforcing SubAccounts Choice
            //    if (UseSubAccounts) // && GlobalFunctions.GetOption("EnforceSubAccountsUse")
            //    {
            //        if (
            //            valueLists["SubAccountID"] != null
            //            && dtSubAccounts.Select("AccountID=" + lstJVDetails[i].AccountID).Length > 0
            //            && (lstJVDetails[i].SubAccountID == null || dtSubAccounts.Select(" SubAccountID = " + lstJVDetails[i].SubAccountID).Length == 0)
            //            )
            //        {
            //            lstMessages[0] = systemSettings.IsArabic ? "برجاء اختيار حساب تحليلي" : "Please choose SubAccount";
            //            return lstMessages;
            //        }
            //    }

            //    //Enforcing CostCenters Choice
            //    if (UseCostCenters)// && GlobalFunctions.GetOption("EnforceCostCentersUse")
            //    {
            //        if (
            //            dtAccounts.Select("AccountID = " + lstJVDetails[i].AccountID)[0]["AccountTypeID"].ToString() == "3"
            //        ||
            //            dtAccounts.Select("AccountID = " + lstJVDetails[i].AccountID)[0]["AccountTypeID"].ToString() == "4"
            //            )
            //        {
            //            if (lstJVDetails[i].CostCenterID == null)
            //            {
            //                lstMessages[0] = systemSettings.IsArabic ? "برجاء اختيار مركز تكلفة" : "Please choose Cost-Center";
            //                return lstMessages;
            //            }
            //        }
            //    }
            //}
            //else if( currentObject.EINVCurrencyID)
            //else if (CheckForValue("Currency", "CurrencyNameAr", txtCurrencyArabicName.Text, (Updating ? this.ULGData.ActiveRow.Cells["CurrencyNameAr"].Value.ToString() : ""), true) > 0)
            //{

            //    errorMessage = isArabic ? " إسم العملة بالعربية متواجد من قبل ": "Currency Arabic Name Already Exist";
            //    txtCurrencyArabicName.Focus();
            //    return false;
            //}
            return lstMessages;
        }
        private async Task<bool> Add()
        {
            bool IsAdded = await AddFun.Invoke();
            return IsAdded;
        }
        private async Task<bool> Update()
        {
            Main main = new Main(ConnectionString);
            //main.StartBulkTrans(true);
            //try
            //{
            //    int ID = jvService.GenerateJV_Update(jvObj, lstJVDetails, userData.BranchID, userData.UserID, new DataAccess.Main(ConnectionString));

            //    if (main.strMessageDetail != null && main.strMessageDetail.Length > 0)
            //    {
            //        InfoMessage = main.strMessageDetail;
            //        IsInfoMBVisible = true;
            //        main.RollbackBulkTrans(true);
            //        StateHasChanged();
            //        return false;
            //    }
            //    else
            //    {
            //        main.EndBulkTrans(true);
            //        return true;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    InfoMessage = main.strMessageDetail + ex.Message;
            //    IsInfoMBVisible = true;
            //    main.RollbackBulkTrans(true);
            //    StateHasChanged();
            return false;
            //}
        }
        private void Copy()
        {
        }
        private async Task btnDelete()
        {
            //await OnDeleteCallBack.InvokeAsync();
            if (DrMaster != null)
            {
                RowID = DrMaster[IDCol].ToString();
                if (!GlobalVariables.CanDelete)
                {
                    InfoMessage = systemSettings.IsArabic ? "لا توجد هذه الصلاحية. برجاء الرجوع لمسئول الأمن" : "This Privilege is Unavailable.Please check Security Administrator";
                    return;
                }
                if (!GlobalVariables.CanModifyOtherBranch)
                {
                    InfoMessage = systemSettings.IsArabic ? "لا يمكن حذف هذه الحركة لإنها تابعة لفرع آخر" : "Cannot Update This Transaction Because It Related to Another Branch ";
                    return;
                }
                if (GlobalVariables.ClosedPeriod)
                {
                    InfoMessage = systemSettings.IsArabic ? "لا يمكن حذف هذه الحركة لإنها تابعة لفتره مغلقه" : "Cannot Delete This Transaction Because It Related to ClosedPeriod ";
                    return;
                }

                //GlobalVariables.QuestionMB.Show("هل تريد حذف هذه البيانات ؟", "Are You Sure You want to Delete this Data?");
                //DataSaved = true;
                //if (GlobalVariables.MessageBoxResult == 'Y')
                //{
                //    DeleteData();
                //    if (DataSaved)
                //    {
                //        FillData();
                //        DrMaster = null;
                //    }
                //}
            }
        }
        private async Task Delete()
        {

        }
        private void Next()
        {
            if (dtSearch != null && dtSearch.Rows.Count > 1)
            {
                if (ItemObj != null)
                {
                    for (int i = 1; i < dtSearch.Rows.Count; i++)
                    {
                        if (dtSearch.Rows[i][IDCol].ToString() == globalFunctions.GetPropertyValue(ItemObj, IDCol))
                        {
                            RowID = dtSearch.Rows[i - 1][IDCol].ToString();
                            FillData(RowID);
                            break;
                        }
                    }
                }
                else
                {
                    RowID = dtSearch.Rows[0][IDCol].ToString();
                    FillData(RowID);
                }
            }
            else
            {
                Main main = new Main(ConnectionString);
                DataTable dt = main.SelectNext("," + userData.BranchID + ",", TableName, IDCol, NoCol, DateCol, RowID, "", "1");
                if (dt.Rows.Count > 0)
                {
                    RowID = dt.Rows[0][IDCol].ToString();
                }
                else
                {
                    RowID = "";
                }
                FillData(RowID);
            }

        }
        private void Previous()
        {
            if (dtSearch != null && dtSearch.Rows.Count > 1)
            {
                if (ItemObj != null)
                {
                    for (int i = 0; i < dtSearch.Rows.Count - 1; i++)
                    {
                        if (dtSearch.Rows[i][IDCol].ToString() == globalFunctions.GetPropertyValue(ItemObj, IDCol))
                        {
                            RowID = dtSearch.Rows[i + 1][IDCol].ToString();
                            FillData(RowID);
                            break;
                        }
                    }
                }
                else
                {
                    RowID = dtSearch.Rows[dtSearch.Rows.Count - 1]["BankInID"].ToString();
                    FillData(RowID);

                }
            }
            else
            {
                Main main = new Main(ConnectionString);
                DataTable dt = main.SelectNext("," + userData.BranchID + ",", TableName, IDCol, NoCol, DateCol, RowID, "", "0");
                if (dt.Rows.Count > 0)
                {
                    RowID = dt.Rows[0][IDCol].ToString();
                }
                else
                {
                    RowID = "";
                }
                FillData(RowID);
            }
        }

        private string CheckBeforeDelete()
        {
            //if (GlobalVariables.LocalCurrencyID == currentObject.CurrencyID)
            //{
            //    return isArabic ? "لا يمكن حذف العملة المحلية" : "You cannot delete the local currency.";
            //}
            return "";
        }
        private void Cancel()
        {
            //FillData();
            //currentObject = oldObject.Clone();
            //currencies[currencies.FindIndex(cur => cur.CurrencyID == currentObject.CurrencyID)] = currentObject;
        }
        private void Close()
        {
            dtSearch = null;
            IsInfoMBVisible = false;
            IsSearchMDVisible = false;
        }
        private void ChangeNavMode(GlobalVariables.States state)
        {
            currentState = state;
            isNavMode = currentState == GlobalVariables.States.NavMode;
        }
        private TItemDetails AddNewDefaultItem(TItemDetails _item)
        {
            _item = new TItemDetails();
            _item = CloneWithJson(itemDetailsDefultObj);
            return _item;
        }
        public static TItemDetails CloneWithJson<TItemDetails>(TItemDetails obj)
        {
            var json = JsonSerializer.Serialize(obj);
            return JsonSerializer.Deserialize<TItemDetails>(json);
        }
        private TItemDetails CellUpdate(TItemDetails item, string col)
        {
            return item;
        }
        private TItemDetails CellListChanged(TItemDetails item, string col)
        {
            StateHasChanged();
            return item;
        }


        private void DateChanged()
        {
        }
    }
}
