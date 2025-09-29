using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataAccess;

namespace ERPManagement.UI.DataModels.Defaults
{
    public class SystemAccount
    {

        public int SystemAccountID { get; set; }

        public string? AccountNameEn { get; set; }

        public string? AccountNameAr { get; set; }

        public int? AccountID { get; set; }

        public bool? HasSubAccount { get; set; }

        public int? SubAccountID { get; set; }

    }
    public class SystemAccountService : Interfaces.IScopedService
    {
        public List<SystemAccount> Select(int SystemAccountID, string BranchIDs, bool IsArabic, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@SystemAccountID", SystemAccountID);
            parameters[1] = new SqlParameter("@BranchIDs", BranchIDs);
            parameters[2] = new SqlParameter("@IsArabic", IsArabic);
            DataTable dataTable = main.ExecuteQuery_DataTable(" SystemAccount_Select  ", true, parameters);
            List<SystemAccount> lst = new List<SystemAccount>();
            if (dataTable != null)
            {
                lst = main.CreateListFromTable<SystemAccount>(dataTable);
            }
            return lst;
        }







    }
}
