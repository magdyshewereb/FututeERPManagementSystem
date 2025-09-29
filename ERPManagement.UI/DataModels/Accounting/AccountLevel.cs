using System.Data;
using System.Data.SqlClient;
using ERPManagement.UI.DataAccess;
using Microsoft.Data.SqlClient;

namespace ERPManagement.UI.DataModels.Accounting
{
    public class AccountLevel
    {

        public int LevelID { get; set; }
        public int Level { get; set; }
        public int Width { get; set; }
        public int Start { get; set; }
        public int End { get; set; }

        public bool Deleted { get; set; }

        public int? BranchID { get; set; }

    }
    public class AccountLevelsService : Interfaces.IScopedService
    {

        public string GetCode(Main main)
        {
            DataTable dataTable = main.ExecuteQuery_DataTable("A_AccountLevels_GetCode ",false, null);
            return dataTable.Rows[0][0].ToString();
        }
        public string GetCodeByBranchID(DateTime Date, int BranchID, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@Date", Date);
            parameters[1] = new SqlParameter("@BranchID", BranchID);
            DataTable dataTable = main.ExecuteQuery_DataTable("A_AccountLevels_GetCodeByBranchID ", true, parameters);
            return dataTable.Rows[0][0].ToString();
        }

        public int Insert_Update(AccountLevel obj, int UserID, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[5];
            parameters[0] = new SqlParameter("@LevelID", obj.LevelID);
            parameters[1] = new SqlParameter("@Width", obj.Width);
            parameters[2] = new SqlParameter("@Deleted", obj.Deleted);
            parameters[3] = new SqlParameter("@BranchID", obj.BranchID);
            parameters[4] = new SqlParameter("@UserID", UserID);
            DataTable dataTable = main.ExecuteQuery_DataTable("A_AccountLevels_Insert_Update  ", true, parameters);
            return int.Parse(dataTable.Rows[0][0].ToString());
        }

        public List<AccountLevel> Select(int LevelID, string BranchIDs, bool IsArabic, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@LevelID", LevelID);
            parameters[1] = new SqlParameter("@BranchIDs", BranchIDs);
            parameters[2] = new SqlParameter("@IsArabic", IsArabic);
            DataTable dataTable = main.ExecuteQuery_DataTable("A_AccountLevels_Select  ", true, parameters);
            List<AccountLevel> lst = new List<AccountLevel>();
            if (dataTable != null)
            {
                lst = main.CreateListFromTable<AccountLevel>(dataTable);
            }
            return lst;
        }

        public void Delete(int LevelID, int UserID, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@LevelID", LevelID);
            parameters[1] = new SqlParameter("@UserID", UserID);
            main.ExecuteNonQuery("A_AccountLevels_Delete  ", true, parameters);
        }
        public void DeleteVirtual(int LevelID, int UserID, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@LevelID", LevelID);
            parameters[1] = new SqlParameter("@UserID", UserID);
           
                main.ExecuteNonQuery("A_AccountLevels_DeleteVirtual  ", true, parameters);
        }
    }
}
