using System.Data;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataModels.Interfaces;
using ERPManagement.UI.DataAccess;

namespace ERPManagement.UI.DataModels.Privilege
{
    public class Form
    {

        public int FormID { get; set; }

        public string FormNameAr { get; set; }

        public string FormNameEn { get; set; }

        public string? form { get; set; }

        public int? ParentID { get; set; }

        public int? FormOrder { get; set; }

        public bool IsFullName { get; set; }

        public string? FormFullName { get; set; }

        public int BranchID { get; set; }

        public bool Deleted { get; set; }
        public int PeriodDays { get; set; }

    }
public class FormsService : IScopedService
    {

public string GetCode(Main main, bool IsFromServer)
{
DataTable dataTable = IsFromServer ? main.SyncExecuteQuery_DataTable(" Prv_Forms_GetCode " , true, null) : main.ExecuteQuery_DataTable(" Prv_Forms_GetCode ", true, null);
return dataTable.Rows[0][0].ToString();
}

public int Insert_Update(Form obj,int UserID,Main main,bool IsFromServer)
{
SqlParameter[] parameters = new SqlParameter[11];
parameters[0] = new SqlParameter("@FormID", obj.FormID);
parameters[1] = new SqlParameter("@FormNameAr", obj.FormNameAr);
parameters[2] = new SqlParameter("@FormNameEn", obj.FormNameEn);
parameters[3] = new SqlParameter("@Form", obj.form);
parameters[4] = new SqlParameter("@ParentID", obj.ParentID);
parameters[5] = new SqlParameter("@FormOrder", obj.FormOrder);
parameters[6] = new SqlParameter("@IsFullName", obj.IsFullName);
parameters[7] = new SqlParameter("@FormFullName", obj.FormFullName);
parameters[8] = new SqlParameter("@BranchID", obj.BranchID);
parameters[9] = new SqlParameter("@Deleted", obj.Deleted);
parameters[10] = new SqlParameter("@UserID",UserID);
 DataTable dataTable = IsFromServer ? main.SyncExecuteQuery_DataTable(" Prv_Forms_Insert_Update  "  , true, parameters) : main.ExecuteQuery_DataTable(" Prv_Forms_Insert_Update  ", true, parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}

public List<Form> Select(int FormID, string BranchIDs, bool IsArabic, Main main, bool IsFromServer)
{
SqlParameter[] parameters = new SqlParameter[3];
parameters[0] = new SqlParameter("@FormID", FormID);
parameters[1] = new SqlParameter("@BranchIDs",BranchIDs);
parameters[0] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = IsFromServer ? main.SyncExecuteQuery_DataTable(" Prv_Forms_Select  "  , true, parameters) : main.ExecuteQuery_DataTable(" Prv_Forms_Select  ", true, parameters);
List<Form> lst = new List<Form>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<Form>(dataTable);
}
return lst;
}

public void Delete(int FormID, int UserID, Main main, bool IsFromServer)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@FormID", FormID);
parameters[1] = new SqlParameter("@UserID",UserID);
if (IsFromServer)
main.SyncExecuteNonQuery(" Prv_Forms_Delete  "  , true, parameters); 
else
main.ExecuteNonQuery(" Prv_Forms_Delete  ", true, parameters);
}

}
}
