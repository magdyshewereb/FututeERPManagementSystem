using ERPManagement.UI.DataModels.Interfaces;
using System.Drawing;

namespace ERPManagement.UI.GeneralClasses
{
    public class GlobalVariables : ISingletonService
    {
        public enum States
        {
            Adding = 1,
            Updating = 2,
            NavMode = 3
        };

        public static bool IsEINV = true;
        public static bool SeeingVisibleAccount = true;
        public static int LocalCurrencyID = 1;
        public static string AssetSubAccountTypeIDs = ",1,";
        public static string EmployeeSubAccountTypeIDs = ",2,7,";
        public static string SupplierSubAccountTypeIDs = ",3,5,";
        public static string ClientSubAccountTypeIDs = ",4,5,7,";
        public static string SafeIDs = ",1,2,";

        //System Functions
        public static bool SeeingInvisibleAcounts = true;
        public static bool SeeingClosedYears = true;
        public static bool CanDelete = true;
        public static bool CanModifyOtherBranch = true;
        public static bool ClosedPeriod = true;


        //-----------------------Styling ERP---------------------
        public static Color BackgroundColor = Color.FromArgb(12, 91, 170);
        public static Color ForegroundColor = Color.FromArgb(255, 198, 0);
        public static string LabelClass = "form-label fw-bold";
        public static string CheckboxClass = "form-check-input";
        public static string ComboboxClass = "form-select fw-bold text-center";
        public static string TextBoxClass = "form-control fw-bold text-center";
        public static string NumberClass = "form-control";
        public static string DateTimeClass = "form-control";
        public static string TableClass = "table  table-bordered table-sm  table-hover";
        public static string TableHeaderClass = "text-center";
        public static string TableCellDataClass = "text-center";
        //--------------------------------------------------------
        //-----------------------Styling TabControls---------------------
        public static string TabCheckboxClass = "form-check-input";
        public static string TabComboboxClass = "form-select form-select-sm fw-bold text-center";
        public static string TabTextBoxClass = "form-control form-control-sm fw-bold text-center";
        public static string TabNumberClass = "form-control form-control-sm";
        public static string TabDateTimeClass = "form-control form-control-sm";
    }
}
