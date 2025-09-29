using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.Privilege
{
public class SearchPeriod
{

public int SearchPeriodID { get; set; }

public string FormName { get; set; }

public int User_ID { get; set; }

public int SearchPeriodDays { get; set; }

public bool  Deleted { get; set; }

public int BranchID { get; set; }

public SearchPeriod Clone()
{
return (SearchPeriod)MemberwiseClone();
}
}
public class SearchPeriodService : Interfaces.IScopedService
{


public int Insert_Update(string SearchPeriodID, string FormName, string User_ID, string SearchPeriodDays, string Deleted, string BranchID, string UserID,Main main)
{
SqlParameter[] parameters = new SqlParameter[7];
parameters[0] = new SqlParameter("@SearchPeriodID", SearchPeriodID);
parameters[1] = new SqlParameter("@FormName", FormName);
parameters[2] = new SqlParameter("@User_ID", User_ID);
parameters[3] = new SqlParameter("@SearchPeriodDays", SearchPeriodDays);
parameters[4] = new SqlParameter("@Deleted", Deleted);
parameters[5] = new SqlParameter("@BranchID", BranchID);
parameters[6] = new SqlParameter("@UserID",UserID);
DataTable dataTable = main.ExecuteQuery_DataTable("Prv_SearchPeriod_Insert_Update",true,parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}

        public void DeleteByFormName(string User_ID, string FormName, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("User_ID", User_ID);
            parameters[1] = new SqlParameter("FormName", FormName);
            main.ExecuteQuery_DataTable("Prv_SearchPeriod_DeleteByFormName", true, parameters);
        }



        public  DataTable SelectByFormName(string User_ID, string FormName, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("User_ID", User_ID);
            parameters[1] = new SqlParameter("FormName", FormName);
            DataTable dataTable = main.ExecuteQuery_DataTable("Prv_SearchPeriod_SelectByFormName", true, parameters);
            return dataTable;
        }

    }

}

