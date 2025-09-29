using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.Defaults
{
public class JVDefaults
{

public int TransTypeID { get; set; }

public string? TransTypeNumber { get; set; }

public string TransTypeNameAr { get; set; }

public string? TransTypeNameEn { get; set; }

public int? JournalTypeID { get; set; }

public int? JVTypeID { get; set; }

public JVDefaults Clone()
{
return (JVDefaults)MemberwiseClone();
}
}
public class JVDefaultsService : Interfaces.IScopedService
{

public string GetCode(Main main)
{
            DataTable dataTable = main.ExecuteQuery_DataTable("JVDefaults_GetCode",true,null);
return dataTable.Rows[0][0].ToString();
}
public string GetCodeByBranchID(DateTime Date,string BranchID,Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@Date", Date);
parameters[1] = new SqlParameter("@BranchID",BranchID);
DataTable dataTable = main.ExecuteQuery_DataTable("JVDefaults_GetCodeByBranchID",true,parameters);
return dataTable.Rows[0][0].ToString();
}

public int Insert_Update(JVDefaults obj,int UserID,Main main)
{
SqlParameter[] parameters = new SqlParameter[7];
parameters[0] = new SqlParameter("@TransTypeID", obj.TransTypeID== null ? DBNull.Value : obj.TransTypeID);
parameters[1] = new SqlParameter("@TransTypeNumber", obj.TransTypeNumber== null ? DBNull.Value : obj.TransTypeNumber);
parameters[2] = new SqlParameter("@TransTypeNameAr", obj.TransTypeNameAr== null ? DBNull.Value : obj.TransTypeNameAr);
parameters[3] = new SqlParameter("@TransTypeNameEn", obj.TransTypeNameEn== null ? DBNull.Value : obj.TransTypeNameEn);
parameters[4] = new SqlParameter("@JournalTypeID", obj.JournalTypeID== null ? DBNull.Value : obj.JournalTypeID);
parameters[5] = new SqlParameter("@JVTypeID", obj.JVTypeID== null ? DBNull.Value : obj.JVTypeID);
parameters[6] = new SqlParameter("@UserID",UserID);
DataTable dataTable = main.ExecuteQuery_DataTable("JVDefaults_Insert_Update",true,parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}

public DataTable Select(int TransTypeID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@TransTypeID", TransTypeID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable =main.ExecuteQuery_DataTable("JVDefaults_Select",true,parameters);
            return dataTable;
//List<JVDefaults> lst = new List<JVDefaults>();
//if (dataTable != null)
//{
// lst =main.CreateListFromTable<JVDefaults>(dataTable);
//}
//return lst;
}

public void Delete(int TransTypeID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@TransTypeID", TransTypeID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("JVDefaults_Delete",true,parameters);
}
public void DeleteVirtual(int TransTypeID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@TransTypeID", TransTypeID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("JVDefaults_DeleteVirtual",true,parameters);
}


public List<JVDefaults> SelectByJournalTypeID(int JournalTypeID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JournalTypeID", JournalTypeID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("JVDefaults_SelectByJournalTypeID",true,parameters);
List<JVDefaults> lst = new List<JVDefaults>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<JVDefaults>(dataTable);
}
return lst;
}

public void DeleteByJournalTypeID(int JournalTypeID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JournalTypeID", JournalTypeID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("JVDefaults_DeleteByJournalTypeID",true,parameters);
}
public void DeleteVirtualByJournalTypeID(int JournalTypeID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JournalTypeID", JournalTypeID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("JVDefaults_DeleteVirtualByJournalTypeID" ,true,parameters);
}

public List<JVDefaults> SelectByJVTypeID(int JVTypeID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JVTypeID", JVTypeID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("JVDefaults_SelectByJVTypeID",true,parameters);
List<JVDefaults> lst = new List<JVDefaults>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<JVDefaults>(dataTable);
}
return lst;
}

public void DeleteByJVTypeID(int JVTypeID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JVTypeID", JVTypeID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("JVDefaults_DeleteByJVTypeID",true,parameters);
}
public void DeleteVirtualByJVTypeID(int JVTypeID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JVTypeID", JVTypeID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("JVDefaults_DeleteVirtualByJVTypeID" ,true,parameters);
}

}
}
