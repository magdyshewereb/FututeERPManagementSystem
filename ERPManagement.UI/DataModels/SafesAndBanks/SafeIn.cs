using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataModels.Interfaces;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.SafesAndBanks
{
public class SafeIn
{

public int SafeInID { get; set; }

public string SafeInNo { get; set; }

public DateTime SafeInDate { get; set; }

public int SafeID { get; set; }

public int CurrencyID { get; set; }

public decimal ExchangeRate { get; set; }

public string ChargedPerson { get; set; }

public string Notes { get; set; }

public decimal?  Total { get; set; }

public int? SalesManID { get; set; }

public int? JvID { get; set; }

public bool  Approved { get; set; }

public int? BranchID { get; set; }

public bool  Deleted { get; set; }

public SafeIn Clone()
{
return (SafeIn)MemberwiseClone();
}
}
public class SafeInService : IScopedService
    {


public int Insert_Update(SafeIn obj,int UserID,Main main)
{
SqlParameter[] parameters = new SqlParameter[15];
parameters[0] = new SqlParameter("@SafeInID", obj.SafeInID== null ? DBNull.Value : obj.SafeInID);
parameters[1] = new SqlParameter("@SafeInNo", obj.SafeInNo== null ? DBNull.Value : obj.SafeInNo);
parameters[2] = new SqlParameter("@SafeInDate", obj.SafeInDate== null ? DBNull.Value : obj.SafeInDate);
parameters[3] = new SqlParameter("@SafeID", obj.SafeID== null ? DBNull.Value : obj.SafeID);
parameters[4] = new SqlParameter("@CurrencyID", obj.CurrencyID== null ? DBNull.Value : obj.CurrencyID);
parameters[5] = new SqlParameter("@ExchangeRate", obj.ExchangeRate== null ? DBNull.Value : obj.ExchangeRate);
parameters[6] = new SqlParameter("@ChargedPerson", obj.ChargedPerson== null ? DBNull.Value : obj.ChargedPerson);
parameters[7] = new SqlParameter("@Notes", obj.Notes== null ? DBNull.Value : obj.Notes);
parameters[8] = new SqlParameter("@Total", obj.Total== null ? DBNull.Value : obj.Total);
parameters[9] = new SqlParameter("@SalesManID", obj.SalesManID== null ? DBNull.Value : obj.SalesManID);
parameters[10] = new SqlParameter("@JvID", obj.JvID== null ? DBNull.Value : obj.JvID);
parameters[11] = new SqlParameter("@Approved", obj.Approved== null ? false : obj.Approved);
parameters[13] = new SqlParameter("@BranchID", obj.BranchID== null ? DBNull.Value : obj.BranchID);
parameters[14] = new SqlParameter("@Deleted", obj.Deleted== null ? false : obj.Deleted);
parameters[14] = new SqlParameter("@UserID",UserID);
DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeIn_Insert_Update",true,parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}

public List<SafeIn> Select(int SafeInID, string BranchIDs, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[3];
parameters[0] = new SqlParameter("@SafeInID", SafeInID);
parameters[1] = new SqlParameter("@BranchIDs",BranchIDs);
parameters[2] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable =main.ExecuteQuery_DataTable("SB_SafeIn_Select",true,parameters);
List<SafeIn> lst = new List<SafeIn>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeIn>(dataTable);
}
return lst;
}

public void Delete(int SafeInID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SafeInID", SafeInID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeIn_Delete",true,parameters);
}
public void DeleteVirtual(int SafeInID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SafeInID", SafeInID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeIn_DeleteVirtual",true,parameters);
}


public List<SafeIn> SelectByFiscalYearID(int FiscalYearID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@FiscalYearID", FiscalYearID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeIn_SelectByFiscalYearID",true,parameters);
List<SafeIn> lst = new List<SafeIn>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeIn>(dataTable);
}
return lst;
}

public void DeleteByFiscalYearID(int FiscalYearID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@FiscalYearID", FiscalYearID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeIn_DeleteByFiscalYearID",true,parameters);
}
public void DeleteVirtualByFiscalYearID(int FiscalYearID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@FiscalYearID", FiscalYearID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeIn_DeleteVirtualByFiscalYearID" ,true,parameters);
}

public List<SafeIn> SelectByJvID(int JvID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JvID", JvID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeIn_SelectByJvID",true,parameters);
List<SafeIn> lst = new List<SafeIn>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeIn>(dataTable);
}
return lst;
}

public void DeleteByJvID(int JvID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JvID", JvID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeIn_DeleteByJvID",true,parameters);
}
public void DeleteVirtualByJvID(int JvID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JvID", JvID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeIn_DeleteVirtualByJvID" ,true,parameters);
}

public List<SafeIn> SelectByCurrencyID(int CurrencyID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CurrencyID", CurrencyID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeIn_SelectByCurrencyID",true,parameters);
List<SafeIn> lst = new List<SafeIn>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeIn>(dataTable);
}
return lst;
}

public void DeleteByCurrencyID(int CurrencyID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CurrencyID", CurrencyID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeIn_DeleteByCurrencyID",true,parameters);
}
public void DeleteVirtualByCurrencyID(int CurrencyID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CurrencyID", CurrencyID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeIn_DeleteVirtualByCurrencyID" ,true,parameters);
}

public List<SafeIn> SelectByBranchID(int BranchID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeIn_SelectByBranchID",true,parameters);
List<SafeIn> lst = new List<SafeIn>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeIn>(dataTable);
}
return lst;
}

public void DeleteByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeIn_DeleteByBranchID",true,parameters);
}
public void DeleteVirtualByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeIn_DeleteVirtualByBranchID" ,true,parameters);
}

public List<SafeIn> SelectBySafeID(int SafeID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SafeID", SafeID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeIn_SelectBySafeID",true,parameters);
List<SafeIn> lst = new List<SafeIn>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeIn>(dataTable);
}
return lst;
}

public void DeleteBySafeID(int SafeID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SafeID", SafeID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeIn_DeleteBySafeID",true,parameters);
}
public void DeleteVirtualBySafeID(int SafeID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SafeID", SafeID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeIn_DeleteVirtualBySafeID" ,true,parameters);
}

}
}
