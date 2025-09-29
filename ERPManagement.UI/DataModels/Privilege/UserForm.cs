using ERPManagement.UI.DataAccess;
using ERPManagement.UI.DataModels.Interfaces;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Data;
//using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.Privilege
{
    public class UserForm
    {

        public int UserFormID { get; set; }

        public int User_ID { get; set; }

        public int FormID { get; set; }

        public int PeriodDays { get; set; }

        public int BranchID { get; set; }

        public bool Deleted { get; set; }

    }
    public class UserFormService : IScopedService
    {

        public string GetCode(Main main, bool IsFromServer)
        {
            DataTable dataTable = IsFromServer ? main.SyncExecuteQuery_DataTable(" Prv_UsersForms_GetCode ", true, null) : main.ExecuteQuery_DataTable(" Prv_UsersForms_GetCode ", true, null);
            return dataTable.Rows[0][0].ToString();
        }


        public int Insert_Update(UserForm obj, int UserID, Main main, bool IsFromServer)
        {
            SqlParameter[] parameters = new SqlParameter[7];
            parameters[0] = new SqlParameter("@UserFormID", obj.UserFormID);
            parameters[1] = new SqlParameter("@User_ID", obj.User_ID);
            parameters[2] = new SqlParameter("@FormID", obj.FormID);
            parameters[3] = new SqlParameter("@PeriodDays", obj.PeriodDays);
            parameters[4] = new SqlParameter("@BranchID", obj.BranchID);
            parameters[5] = new SqlParameter("@Deleted", obj.Deleted);
            parameters[7] = new SqlParameter("@UserID", UserID);
            DataTable dataTable = IsFromServer ? main.SyncExecuteQuery_DataTable(" Prv_UsersForms_Insert_Update  ", true, parameters) : main.ExecuteQuery_DataTable(" Prv_UsersForms_Insert_Update  ", true, parameters);
            return int.Parse(dataTable.Rows[0][0].ToString());
        }

        public List<UserForm> Select(int UserFormID, string BranchIDs, bool IsArabic, Main main, bool IsFromServer)
        {
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@UserFormID", UserFormID);
            parameters[1] = new SqlParameter("@BranchIDs", BranchIDs);
            parameters[0] = new SqlParameter("@IsArabic", IsArabic);
            DataTable dataTable = IsFromServer ? main.SyncExecuteQuery_DataTable(" Prv_UsersForms_Select  ", true, parameters) : main.ExecuteQuery_DataTable(" Prv_UsersForms_Select  ", true, parameters);
            List<UserForm> lst = new List<UserForm>();
            if (dataTable != null)
            {
                lst = main.CreateListFromTable<UserForm>(dataTable);
                return lst;
            }
            return lst;
        }
        public DataTable SelectForms(int UserID, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@UserID", UserID);
            DataTable dataTable = main.ExecuteQuery_DataTable("Prv_UsersForms_SelectForms_ERPWeb", true, parameters);
            return dataTable;
        }

        public void Delete(int UserFormID, int UserID, Main main, bool IsFromServer)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@UserFormID", UserFormID);
            parameters[1] = new SqlParameter("@UserID", UserID);
            if (IsFromServer)
                main.SyncExecuteNonQuery(" Prv_UsersForms_Delete  ", true, parameters);
            else
                main.ExecuteNonQuery(" Prv_UsersForms_Delete  ", true, parameters);
        }

    }
}
