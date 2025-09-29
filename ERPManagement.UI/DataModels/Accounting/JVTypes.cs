using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.Accounting
{
public class JVTypes
{

public int JVTypeID { get; set; }

public string JVTypeNameAr { get; set; }

public string? JVTypeNameEn { get; set; }

public bool  Deleted { get; set; }

public int? BranchID { get; set; }

public JVTypes Clone()
{
return (JVTypes)MemberwiseClone();
}
}
public class JVTypesService : Interfaces.IScopedService
{

public string GetCode(Main main)
{
            DataTable dataTable = main.ExecuteQuery_DataTable("A_JVTypes_GetCode",true,null);
return dataTable.Rows[0][0].ToString();
}
public string GetCodeByBranchID(DateTime Date,string BranchID,Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@Date", Date);
parameters[1] = new SqlParameter("@BranchID",BranchID);
DataTable dataTable = main.ExecuteQuery_DataTable("A_JVTypes_GetCodeByBranchID",true,parameters);
return dataTable.Rows[0][0].ToString();
}

public int Insert_Update(JVTypes obj,int UserID,Main main)
{
SqlParameter[] parameters = new SqlParameter[6];
parameters[0] = new SqlParameter("@JVTypeID", obj.JVTypeID== null ? DBNull.Value : obj.JVTypeID);
parameters[1] = new SqlParameter("@JVTypeNameAr", obj.JVTypeNameAr== null ? DBNull.Value : obj.JVTypeNameAr);
parameters[2] = new SqlParameter("@JVTypeNameEn", obj.JVTypeNameEn== null ? DBNull.Value : obj.JVTypeNameEn);
parameters[3] = new SqlParameter("@Deleted", obj.Deleted== null ? false : obj.Deleted);
parameters[4] = new SqlParameter("@BranchID", obj.BranchID== null ? DBNull.Value : obj.BranchID);
parameters[5] = new SqlParameter("@UserID",UserID);
DataTable dataTable = main.ExecuteQuery_DataTable("A_JVTypes_Insert_Update",true,parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}

public List<JVTypes> Select(int JVTypeID, string BranchIDs, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[3];
parameters[0] = new SqlParameter("@JVTypeID", JVTypeID);
parameters[1] = new SqlParameter("@BranchIDs",BranchIDs);
parameters[2] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable =main.ExecuteQuery_DataTable("A_JVTypes_Select",true,parameters);
List<JVTypes> lst = new List<JVTypes>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<JVTypes>(dataTable);
}
return lst;
}

public void Delete(int JVTypeID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JVTypeID", JVTypeID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_JVTypes_Delete",true,parameters);
}
public void DeleteVirtual(int JVTypeID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JVTypeID", JVTypeID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_JVTypes_DeleteVirtual",true,parameters);
}


public List<JVTypes> SelectByBranchID(int BranchID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("A_JVTypes_SelectByBranchID",true,parameters);
List<JVTypes> lst = new List<JVTypes>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<JVTypes>(dataTable);
}
return lst;
}

public void DeleteByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_JVTypes_DeleteByBranchID",true,parameters);
}
public void DeleteVirtualByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_JVTypes_DeleteVirtualByBranchID" ,true,parameters);
}

}
}
