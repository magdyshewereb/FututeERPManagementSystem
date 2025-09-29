using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.Privilege
{
    public class UserFormFunctions
    {

        public int UserFormFunctionID { get; set; }

        public int User_ID { get; set; }

        public int FormFunctionID { get; set; }

        public int BranchID { get; set; }

        public bool Deleted { get; set; }

        public UserFormFunctions Clone()
        {
            return (UserFormFunctions)MemberwiseClone();
        }
    }
    public class UserFormFunctionsService : Interfaces.IScopedService
    {


        public int Insert_Update(UserFormFunctions obj, int UserID, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[6];
            parameters[0] = new SqlParameter("@UserFormFunctionID", obj.UserFormFunctionID);
            parameters[1] = new SqlParameter("@User_ID", obj.User_ID);
            parameters[2] = new SqlParameter("@FormFunctionID", obj.FormFunctionID);
            parameters[3] = new SqlParameter("@BranchID", obj.BranchID);
            parameters[4] = new SqlParameter("@Deleted", obj.Deleted);
            parameters[5] = new SqlParameter("@UserID", UserID);
            DataTable dataTable = main.ExecuteQuery_DataTable("Prv_UsersFormsFunctions_Insert_Update", true, parameters);
            return int.Parse(dataTable.Rows[0][0].ToString());
        }

        public List<UserFormFunctions> Select(int UserFormFunctionID, string BranchIDs, bool IsArabic, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@UserFormFunctionID", UserFormFunctionID);
            parameters[1] = new SqlParameter("@BranchIDs", BranchIDs);
            parameters[2] = new SqlParameter("@IsArabic", IsArabic);
            DataTable dataTable = main.ExecuteQuery_DataTable("Prv_UsersFormsFunctions_Select", true, parameters);
            List<UserFormFunctions> lst = new List<UserFormFunctions>();
            if (dataTable != null)
            {
                lst = main.CreateListFromTable<UserFormFunctions>(dataTable);
            }
            return lst;
        }
        public DataTable SelectFunctions(int UserID, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@User_ID", UserID);
            DataTable dataTable =  main.ExecuteQuery_DataTable("Prv_UsersFormsFunctions_SelectFunctions", true, parameters);
            return dataTable;
        }
        public void Delete(int UserFormFunctionID, int UserID, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@UserFormFunctionID", UserFormFunctionID);
            parameters[1] = new SqlParameter("@UserID", UserID);
            main.ExecuteNonQuery("Prv_UsersFormsFunctions_Delete", true, parameters);
        }
        public void DeleteVirtual(int UserFormFunctionID, int UserID, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@UserFormFunctionID", UserFormFunctionID);
            parameters[1] = new SqlParameter("@UserID", UserID);
            main.ExecuteNonQuery("Prv_UsersFormsFunctions_DeleteVirtual", true, parameters);
        }








    }
}
