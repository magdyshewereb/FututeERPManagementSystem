using System.Data;
using Microsoft.Data.SqlClient;
using System;
using ERPManagement.UI.DataAccess;

namespace ERPManagement.UI.DataModels.Defaults
{
    public class SyncConnection: object
    {

        public int ConnectionID { get; set; }

        public string ServerName { get; set; }

        public string? ServerName2 { get; set; }

        public string UserName { get; set; }

        public string? Password { get; set; }

        public string DataBaseName { get; set; }

        public int? SyncBranchID { get; set; }

        public bool? IsUnderSync { get; set; }

        public DateTime? SyncStartDate { get; set; }

        public bool Deleted { get; set; }

        public int? BranchID { get; set; }

    }
    public class SyncConnectionService : Interfaces.IScopedService
    {


        public int Insert_Update(SyncConnection obj, int UserID, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[12];
            parameters[0] = new SqlParameter("@ConnectionID", obj.ConnectionID);
            parameters[1] = new SqlParameter("@ServerName", obj.ServerName);
            parameters[2] = new SqlParameter("@ServerName2", obj.ServerName2);
            parameters[3] = new SqlParameter("@UserName", obj.UserName);
            parameters[4] = new SqlParameter("@Password", obj.Password);
            parameters[5] = new SqlParameter("@DataBaseName", obj.DataBaseName);
            parameters[6] = new SqlParameter("@SyncBranchID", obj.SyncBranchID);
            parameters[7] = new SqlParameter("@IsUnderSync", obj.IsUnderSync);
            parameters[8] = new SqlParameter("@SyncStartDate", obj.SyncStartDate);
            parameters[9] = new SqlParameter("@Deleted", obj.Deleted);
            parameters[10] = new SqlParameter("@BranchID", obj.BranchID);
            parameters[12] = new SqlParameter("@UserID", UserID);
            DataTable dataTable = main.ExecuteQuery_DataTable(" Sync_Connection_Insert_Update ", true, parameters);
            return int.Parse(dataTable.Rows[0][0].ToString());
        }

        public List<SyncConnection> Select(int ConnectionID, string BranchIDs, bool IsArabic, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@ConnectionID", ConnectionID);
            parameters[1] = new SqlParameter("@BranchIDs", BranchIDs);
            parameters[2] = new SqlParameter("@IsArabic", IsArabic);
            DataTable dataTable = main.ExecuteQuery_DataTable("Sync_Connection_Select", true, parameters);
            List<SyncConnection> lst = new List<SyncConnection>();
            if (dataTable != null)
            {
                lst = main.CreateListFromTable<SyncConnection>(dataTable);
            }
            return lst;
        }

        public void Delete(int ConnectionID, int UserID, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@ConnectionID", ConnectionID);
            parameters[1] = new SqlParameter("@UserID", UserID);
            main.ExecuteNonQuery(" Sync_Connection_Delete  ", true, parameters);
        }
        public void DeleteVirtual(int ConnectionID, int UserID, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@ConnectionID", ConnectionID);
            parameters[1] = new SqlParameter("@UserID", UserID);
            main.ExecuteNonQuery(" Sync_Connection_DeleteVirtual  ", true, parameters);
        }


    }
}
