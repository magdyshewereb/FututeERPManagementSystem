using System.Data;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ERPManagement.UI.DataModels.Interfaces;
using ERPManagement.UI.DataAccess;

namespace ERPManagement.UI.DataModels.Privilege
{
    public class UserBranch
    {

        public int UserBranchID { get; set; }

        public int User_ID { get; set; }

        public int BranchID { get; set; }

        public bool Deleted { get; set; }

    }
    public class UsersBranchesService : IScopedService
    {
        public string SelectUserBrancheIDs(string UserID,Main main)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@UserID", UserID);
            DataTable dataTable =  main.ExecuteQuery_DataTable("Prv_UsersBranches_selectUserBranches", true, parameters);
            return dataTable.Rows[0][0].ToString();
        }
        public string GetCode(Main main, bool IsFromServer)
        {
            DataTable dataTable = IsFromServer ? main.SyncExecuteQuery_DataTable(" Prv_UsersBranches_GetCode ", true, null) : main.ExecuteQuery_DataTable(" Prv_UsersBranches_GetCode ", true, null);
            return dataTable.Rows[0][0].ToString();
        }
        
        public int Insert_Update(UserBranch obj, int UserID, Main main, bool IsFromServer)
        {
            SqlParameter[] parameters = new SqlParameter[5];
            parameters[0] = new SqlParameter("@UserBranchID", obj.UserBranchID);
            parameters[1] = new SqlParameter("@User_ID", obj.User_ID);
            parameters[2] = new SqlParameter("@BranchID", obj.BranchID);
            parameters[3] = new SqlParameter("@Deleted", obj.Deleted);
            parameters[5] = new SqlParameter("@UserID", UserID);
            DataTable dataTable = IsFromServer ? main.SyncExecuteQuery_DataTable(" Prv_UsersBranches_Insert_Update  ", true, parameters) : main.ExecuteQuery_DataTable(" Prv_UsersBranches_Insert_Update  ", true, parameters);
            return int.Parse(dataTable.Rows[0][0].ToString());
        }

        public List<UserBranch> Select(int UserBranchID, string BranchIDs, bool IsArabic, Main main, bool IsFromServer)
        {
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@UserBranchID", UserBranchID);
            parameters[1] = new SqlParameter("@BranchIDs", BranchIDs);
            parameters[0] = new SqlParameter("@IsArabic", IsArabic);
            DataTable dataTable = IsFromServer ? main.SyncExecuteQuery_DataTable(" Prv_UsersBranches_Select  ", true, parameters) : main.ExecuteQuery_DataTable(" Prv_UsersBranches_Select  ", true, parameters);
            List<UserBranch> lst = new List<UserBranch>();
            if (dataTable != null)
            {
                lst = main.CreateListFromTable<UserBranch>(dataTable);
            }
            return lst;
        }

        public void Delete(int UserBranchID, int UserID, Main main, bool IsFromServer)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@UserBranchID", UserBranchID);
            parameters[1] = new SqlParameter("@UserID", UserID);
            if (IsFromServer)
                main.SyncExecuteNonQuery(" Prv_UsersBranches_Delete  ", true, parameters);
            else
                main.ExecuteNonQuery(" Prv_UsersBranches_Delete  ", true, parameters);
        }
        

    }
}