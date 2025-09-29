using System.Data;
using ERPManagement.UI.DataAccess;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;

namespace ERPManagement.UI.DataModels.Accounting
{
    public class Account
    {

        public int AccountID { get; set; }

        public string? AccountNumber { get; set; }

        public string? AccountNameAr { get; set; }

        public string? AccountNameEn { get; set; }

        public int? ParentID { get; set; }

        public bool IsMain { get; set; }

        public int? LevelID { get; set; }

        public int? AccountTypeID { get; set; }

        public bool IsVisible { get; set; }

        public bool Deleted { get; set; }

        public int BranchID { get; set; }

        public Account Clone()
        {
            return (Account)MemberwiseClone();
        }
    }
    public class AccountService : Interfaces.IScopedService
    {
        public string GetCode(string ParentID, Main main)
        {
            string strSql = string.Empty;
            strSql += "  ";
            strSql += " select dbo.A_Accounts_GetCode(   ";
            strSql += ParentID + ") as NewCode";	//ParentID
            DataTable dataTable =  main.ExecuteQuery_DataTable(strSql, false, null);
            return dataTable.Rows[0][0].ToString();
        }
        public string SelectRelations(string AccountID, Main main, string IsArabic )
        {
            string strSql = string.Empty;
            strSql += " A_Accounts_Relations ";
            strSql += AccountID + ",";
            strSql += IsArabic;
            DataTable dataTable = main.ExecuteQuery_DataTable(strSql, false, null);
            return dataTable.Rows[0][0].ToString();
        }
        public string GetCodeByBranchID(DateTime Date, int BranchID, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@Date", Date);
            parameters[1] = new SqlParameter("@BranchID", BranchID);
            DataTable dataTable = main.ExecuteQuery_DataTable(" A_Accounts_GetCodeByBranchID ", true, parameters);
            return dataTable.Rows[0][0].ToString();
        }

        public int Insert_Update(Account obj, int UserID, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[12];
            parameters[0] = new SqlParameter("@AccountID", obj.AccountID);
            parameters[1] = new SqlParameter("@AccountNumber", obj.AccountNumber);
            parameters[2] = new SqlParameter("@AccountNameAr", obj.AccountNameAr);
            parameters[3] = new SqlParameter("@AccountNameEn", obj.AccountNameEn);
            parameters[4] = new SqlParameter("@ParentID", obj.ParentID);
            parameters[5] = new SqlParameter("@IsMain", obj.IsMain);
            parameters[6] = new SqlParameter("@LevelID", obj.LevelID);
            parameters[7] = new SqlParameter("@AccountTypeID", obj.AccountTypeID);
            parameters[8] = new SqlParameter("@IsVisible", obj.IsVisible);
            parameters[9] = new SqlParameter("@Deleted", obj.Deleted);
            parameters[10] = new SqlParameter("@BranchID", obj.BranchID);
            parameters[11] = new SqlParameter("@UserID", UserID);
            DataTable dataTable =  main.ExecuteQuery_DataTable("A_Accounts_Insert_Update", true, parameters);
            return int.Parse(dataTable.Rows[0][0].ToString());
        }

        public List<Account> Select(int AccountID, string BranchIDs, bool IsArabic, Main main )
        {
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@AccountID", AccountID);
            parameters[1] = new SqlParameter("@BranchIDs", BranchIDs);
            parameters[2] = new SqlParameter("@IsArabic", IsArabic);
            DataTable dataTable =  main.ExecuteQuery_DataTable("A_Accounts_Select", true, parameters);
            List<Account> lst = new List<Account>();
            if (dataTable != null)
            {
                lst = main.CreateListFromTable<Account>(dataTable);
            }
            return lst;
        }
        public DataTable FillCombo(string SeeingInvisibleAcounts, bool IsArabic, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@SeeingInvisibleAcounts", SeeingInvisibleAcounts);
            parameters[1] = new SqlParameter("@IsArabic", IsArabic);
            DataTable dataTable = main.ExecuteQuery_DataTable("A_Accounts_Fill_Combo", true, parameters);
            return dataTable;
        }
        public void Delete(int AccountID, int UserID, Main main )
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@AccountID", AccountID);
            parameters[1] = new SqlParameter("@UserID", UserID);
           
                main.ExecuteNonQuery("A_Accounts_Delete", true, parameters);
        }
        public void DeleteVirtual(int AccountID, int UserID, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@AccountID", AccountID);
            parameters[1] = new SqlParameter("@UserID", UserID);
          
                main.ExecuteNonQuery(" A_Accounts_DeleteVirtual  ", true, parameters);
        }
    }
}
