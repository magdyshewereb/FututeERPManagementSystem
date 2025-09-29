using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.Privilege
{
public class SearchColumns
{

public int SearchColID { get; set; }

public string? FormName { get; set; }

public int? User_ID { get; set; }

public string? ColKey { get; set; }

public int? ColOrder { get; set; }

public bool  Deleted { get; set; }

public int? BranchID { get; set; }

public SearchColumns Clone()
{
return (SearchColumns)MemberwiseClone();
}
}
public class SearchColumnsService : Interfaces.IScopedService
{


public int Insert_Update(string SearchColID, string FormName, string User_ID, string ColKey, string ColOrder, string Deleted, string BranchID, string UserID,Main main)
{
SqlParameter[] parameters = new SqlParameter[8];
parameters[0] = new SqlParameter("@SearchColID", SearchColID);
parameters[1] = new SqlParameter("@FormName", FormName);
parameters[2] = new SqlParameter("@User_ID", User_ID);
parameters[3] = new SqlParameter("@ColKey", ColKey);
parameters[4] = new SqlParameter("@ColOrder", ColOrder);
parameters[5] = new SqlParameter("@Deleted", Deleted);
parameters[6] = new SqlParameter("@BranchID", BranchID);
parameters[7] = new SqlParameter("@UserID",UserID);
DataTable dataTable = main.ExecuteQuery_DataTable("Prv_SearchColumns_Insert_Update",true,parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}



public void DeleteByFormName(string User_ID, string FormName, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("User_ID", User_ID);
            parameters[1] = new SqlParameter("FormName", FormName);
            main.ExecuteQuery_DataTable("Prv_SearchColumns_DeleteByFormName", true, parameters);
        }



        public  DataTable SelectByFormName(string User_ID, string FormName, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("User_ID", User_ID);
            parameters[1] = new SqlParameter("FormName", FormName);
            DataTable dataTable = main.ExecuteQuery_DataTable("Prv_SearchColumns_SelectByFormName", true, parameters);
            return dataTable;
        }

    }
}
