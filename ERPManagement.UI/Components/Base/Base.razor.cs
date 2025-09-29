using Microsoft.AspNetCore.Components;
using System.Data;
using ERPManagement.UI.GeneralClasses;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics.Metrics;
using ERPManagement.UI.DataModels.ProtectedLocalStorage;

namespace ERPManagement.UI.Components.Base
{
    public partial class Base : ComponentBase

    {
        // To Set Form Name 
        [Parameter]
        public string FormFullName { get; set; } = "ERP.Accounting.MasterData.frmCostCenterTree";
        // To Notify When All Privilege Already Set
        [Parameter]
        public EventCallback<FormFunctions> OnFormFunctionsSetCallBack { get; set; } 
        private FormFunctions formFunctions { get; set; } = new();
        private DataTable dtforms,dtformFunctions;
        protected override async Task OnInitializedAsync()
        {
            dtforms = await protectedLocalStorageService.GetUserFormsAsync();
            if (dtforms != null &&  dtforms.Select("FormFullName = '" + FormFullName + "'").Length > 0)
            {              
                dtformFunctions = await protectedLocalStorageService.GetUserFormsFunctions();
            }
            if (dtformFunctions != null)
            {

                DataRow [] drows=  dtformFunctions.Select(" FormID= '" + dtforms.Select("FormFullName = '" + FormFullName + "'")[0]["FormID"].ToString() + "'");
                foreach (DataRow dr in drows) 
                {
                    switch (dr["FunctionNameEn"].ToString())
                    {
                        case "Adding":
                            formFunctions.CanAdd = true;
                            break;
                        case "Updating":
                            formFunctions.CanUpdate = true;
                            break;
                        case "Deleting":
                            formFunctions.CanDelete = true;
                            break;
                        case "Posting":
                            formFunctions.CanPost = true;
                            break;
                        case "EditDate":
                            formFunctions.CanEditDate = true;
                            break;
                        case "Searching":
                            formFunctions.CanSearching = true;
                            break;
                        case "ViewCostPrice":
                            formFunctions.CanViewCostPrice = true;
                            break;
                        case "Discount":
                            formFunctions.CanDiscount = true;
                            break;
                        case "ModifyPriceType":
                            formFunctions.CanModifyPriceType = true;
                            break;
                        case "ModifyQty":
                            formFunctions.CanModifyQty = true;
                            break;
                        case "ViewQty":
                            formFunctions.CanViewQty = true;
                            break;
                        case "ModifySubAccount":
                            formFunctions.CanModifySubAccount = true;
                            break;
                        case "ViewJV":
                            formFunctions.CanViewJV = true;
                            break;
                        case "ViewAllEmployees":
                            formFunctions.ViewAllEmployees = true;
                            break;
                        case "UpdateProductionBatchNo":
                            formFunctions.CanUpdateProductionBatchNo = true;
                            break;
                        case "MinimunCharge":
                            formFunctions.CanMinimunCharge = true;
                            break;
                        case "EditValue":
                            formFunctions.CanEditValue = true;
                            break;
                        case "CloseCheck":
                            formFunctions.CanCloseCheck = true;
                            break;
                        case "SplitCheck":
                            formFunctions.CanSplitCheck = true;
                            break;
                        case "MergeCheck":
                            formFunctions.CanMergeCheck = true;
                            break;
                        case "Direct":
                            formFunctions.CanDirect = true;
                            break;
                        case "Printing":
                            formFunctions.CanPrint = true;
                            break;
                        case "Exporting":
                            formFunctions.CanExport = true;
                            break;
                        case "View Reports":
                            formFunctions.CanViewReport = true;
                            break;
                        case "Print Reports":
                            formFunctions.CanPrintReport = true;
                            break;
                        case "DirectPrint":
                            formFunctions.CanDirectPrint = true;
                            break;
                        case "Tap1":
                            formFunctions.Tap1 = true;
                            break;
                        case "Tap2":
                            formFunctions.Tap2 = true;
                            break;
                        case "Tap3":
                            formFunctions.Tap3 = true;
                            break;
                        case "Tap4":
                            formFunctions.Tap4 = true;
                            break;
                        case "Tap5":
                            formFunctions.Tap5 = true;
                            break;
                        case "Tap6":
                            formFunctions.Tap6 = true;
                            break;
                        case "Tap7":
                            formFunctions.Tap7 = true;
                            break;
                        case "Tap8":
                            formFunctions.Tap8 = true;
                            break;
                        case "Tap9":
                            formFunctions.Tap9 = true;
                            break;
                        case "MultiPrint":
                            formFunctions.CanMultiPrint = true;
                            break;
                        case "ViewAllBranches":
                            formFunctions.ViewAllBranches = true;
                            break;

                    }
                }
               await OnFormFunctionsSetCallBack.InvokeAsync(formFunctions);
            }

        }
    }
}
