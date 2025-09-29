using ERPManagement.UI.DataAccess;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ERPManagement.UI.DataModels.Accounting
{
    public class AccountType
    {
        public int AccountTypeID { get; set; }

        public string AccountTypeNameAr { get; set; }

        public string AccountTypeNameEn { get; set; }

        public bool Deleted { get; set; }

        public int? BranchID { get; set; }
        

    }
    public class AccountTypeService : Interfaces.IScopedService
    {
        public List<AccountType> Select(int AccountTypeID, string BranchIDs, bool IsArabic, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@AccountTypeID", AccountTypeID);
            parameters[1] = new SqlParameter("@BranchIDs", BranchIDs);
            parameters[2] = new SqlParameter("@IsArabic", IsArabic);
            DataTable dataTable = main.ExecuteQuery_DataTable("A_AccountsTypes_Select", true, parameters);
            List<AccountType> lst = new List<AccountType>();
            if (dataTable != null)
            {
                lst = main.CreateListFromTable<AccountType>(dataTable);
            }
            return lst;
        }
    }
}
