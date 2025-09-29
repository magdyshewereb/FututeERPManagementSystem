using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.Accounting
{
public class SubAccounts_Classifications
{

public int SubAccountClassificationID { get; set; }

public string? SubAccountClassificationNameAr { get; set; }

public string? SubAccountClassificationNameEn { get; set; }

public bool  Deleted { get; set; }

public int? BranchID { get; set; }

public SubAccounts_Classifications Clone()
{
return (SubAccounts_Classifications)MemberwiseClone();
}
}
public class SubAccounts_ClassificationsService : Interfaces.IScopedService
{

public string GetCode(Main main)
{
DataTable dataTable= main.ExecuteQuery_DataTable("A_SubAccounts_Classifications_GetCode",true,null);
return dataTable.Rows[0][0].ToString();
}
public string GetCodeByBranchID(DateTime Date,string BranchID,Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@Date", Date);
parameters[1] = new SqlParameter("@BranchID",BranchID);
DataTable dataTable = main.ExecuteQuery_DataTable("A_SubAccounts_Classifications_GetCodeByBranchID",true,parameters);
return dataTable.Rows[0][0].ToString();
}

public int Insert_Update(SubAccounts_Classifications obj,int UserID,Main main)
{
SqlParameter[] parameters = new SqlParameter[6];
parameters[0] = new SqlParameter("@SubAccountClassificationID", obj.SubAccountClassificationID);
parameters[1] = new SqlParameter("@SubAccountClassificationNameAr", obj.SubAccountClassificationNameAr);
parameters[2] = new SqlParameter("@SubAccountClassificationNameEn", obj.SubAccountClassificationNameEn);
parameters[3] = new SqlParameter("@Deleted", obj.Deleted);
parameters[4] = new SqlParameter("@BranchID", obj.BranchID);
parameters[5] = new SqlParameter("@UserID",UserID);
DataTable dataTable = main.ExecuteQuery_DataTable("A_SubAccounts_Classifications_Insert_Update",true,parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}

public List<SubAccounts_Classifications> Select(int SubAccountClassificationID, string BranchIDs, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[3];
parameters[0] = new SqlParameter("@SubAccountClassificationID", SubAccountClassificationID);
parameters[1] = new SqlParameter("@BranchIDs",BranchIDs);
parameters[2] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable =main.ExecuteQuery_DataTable("A_SubAccounts_Classifications_Select",true,parameters);
List<SubAccounts_Classifications> lst = new List<SubAccounts_Classifications>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SubAccounts_Classifications>(dataTable);
}
return lst;
}

public void Delete(int SubAccountClassificationID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountClassificationID", SubAccountClassificationID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_SubAccounts_Classifications_Delete",true,parameters);
}
public void DeleteVirtual(int SubAccountClassificationID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountClassificationID", SubAccountClassificationID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_SubAccounts_Classifications_DeleteVirtual",true,parameters);
}




}
}
