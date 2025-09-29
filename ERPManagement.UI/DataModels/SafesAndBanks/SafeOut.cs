using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataModels.Interfaces;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.SafesAndBanks
{
public class SafeOut
{

public int SafeOutID { get; set; }

public string SafeOutNo { get; set; }

public DateTime SafeOutDate { get; set; }

public int SafeID { get; set; }

public int CurrencyID { get; set; }

public decimal ExchangeRate { get; set; }

public bool  IsCustody { get; set; }

public bool  IsRequest { get; set; }

public string ChargedPerson { get; set; }

public string Notes { get; set; }

public decimal?  Total { get; set; }

public int? TaxID { get; set; }

public decimal TaxValue { get; set; }

public decimal?  TotalAfterTax { get; set; }

public int? JvID { get; set; }

public bool  Approved { get; set; }

public int? BranchID { get; set; }

public bool  Deleted { get; set; }

public SafeOut Clone()
{
return (SafeOut)MemberwiseClone();
}
}
public class SafeOutService : IScopedService
    {


public int Insert_Update(SafeOut obj,int UserID,Main main)
{
SqlParameter[] parameters = new SqlParameter[19];
parameters[0] = new SqlParameter("@SafeOutID", obj.SafeOutID== null ? DBNull.Value : obj.SafeOutID);
parameters[1] = new SqlParameter("@SafeOutNo", obj.SafeOutNo== null ? DBNull.Value : obj.SafeOutNo);
parameters[2] = new SqlParameter("@SafeOutDate", obj.SafeOutDate== null ? DBNull.Value : obj.SafeOutDate);
parameters[3] = new SqlParameter("@SafeID", obj.SafeID== null ? DBNull.Value : obj.SafeID);
parameters[4] = new SqlParameter("@CurrencyID", obj.CurrencyID== null ? DBNull.Value : obj.CurrencyID);
parameters[5] = new SqlParameter("@ExchangeRate", obj.ExchangeRate== null ? DBNull.Value : obj.ExchangeRate);
parameters[6] = new SqlParameter("@IsCustody", obj.IsCustody== null ? false : obj.IsCustody);
parameters[7] = new SqlParameter("@IsRequest", obj.IsRequest== null ? false : obj.IsRequest);
parameters[8] = new SqlParameter("@ChargedPerson", obj.ChargedPerson== null ? DBNull.Value : obj.ChargedPerson);
parameters[9] = new SqlParameter("@Notes", obj.Notes== null ? DBNull.Value : obj.Notes);
parameters[10] = new SqlParameter("@Total", obj.Total== null ? DBNull.Value : obj.Total);
parameters[11] = new SqlParameter("@TaxID", obj.TaxID== null ? DBNull.Value : obj.TaxID);
parameters[12] = new SqlParameter("@TaxValue", obj.TaxValue== null ? DBNull.Value : obj.TaxValue);
parameters[13] = new SqlParameter("@TotalAfterTax", obj.TotalAfterTax== null ? DBNull.Value : obj.TotalAfterTax);
parameters[14] = new SqlParameter("@JvID", obj.JvID== null ? DBNull.Value : obj.JvID);
parameters[15] = new SqlParameter("@Approved", obj.Approved== null ? false : obj.Approved);
parameters[17] = new SqlParameter("@BranchID", obj.BranchID== null ? DBNull.Value : obj.BranchID);
parameters[18] = new SqlParameter("@Deleted", obj.Deleted== null ? false : obj.Deleted);
parameters[18] = new SqlParameter("@UserID",UserID);
DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeOut_Insert_Update",true,parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}

public List<SafeOut> Select(int SafeOutID, string BranchIDs, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[3];
parameters[0] = new SqlParameter("@SafeOutID", SafeOutID);
parameters[1] = new SqlParameter("@BranchIDs",BranchIDs);
parameters[2] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable =main.ExecuteQuery_DataTable("SB_SafeOut_Select",true,parameters);
List<SafeOut> lst = new List<SafeOut>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeOut>(dataTable);
}
return lst;
}

public void Delete(int SafeOutID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SafeOutID", SafeOutID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOut_Delete",true,parameters);
}
public void DeleteVirtual(int SafeOutID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SafeOutID", SafeOutID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOut_DeleteVirtual",true,parameters);
}


public List<SafeOut> SelectByFiscalYearID(int FiscalYearID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@FiscalYearID", FiscalYearID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeOut_SelectByFiscalYearID",true,parameters);
List<SafeOut> lst = new List<SafeOut>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeOut>(dataTable);
}
return lst;
}

public void DeleteByFiscalYearID(int FiscalYearID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@FiscalYearID", FiscalYearID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOut_DeleteByFiscalYearID",true,parameters);
}
public void DeleteVirtualByFiscalYearID(int FiscalYearID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@FiscalYearID", FiscalYearID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOut_DeleteVirtualByFiscalYearID" ,true,parameters);
}

public List<SafeOut> SelectByJvID(int JvID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JvID", JvID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeOut_SelectByJvID",true,parameters);
List<SafeOut> lst = new List<SafeOut>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeOut>(dataTable);
}
return lst;
}

public void DeleteByJvID(int JvID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JvID", JvID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOut_DeleteByJvID",true,parameters);
}
public void DeleteVirtualByJvID(int JvID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JvID", JvID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOut_DeleteVirtualByJvID" ,true,parameters);
}

public List<SafeOut> SelectByCurrencyID(int CurrencyID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CurrencyID", CurrencyID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeOut_SelectByCurrencyID",true,parameters);
List<SafeOut> lst = new List<SafeOut>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeOut>(dataTable);
}
return lst;
}

public void DeleteByCurrencyID(int CurrencyID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CurrencyID", CurrencyID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOut_DeleteByCurrencyID",true,parameters);
}
public void DeleteVirtualByCurrencyID(int CurrencyID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CurrencyID", CurrencyID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOut_DeleteVirtualByCurrencyID" ,true,parameters);
}

public List<SafeOut> SelectByBranchID(int BranchID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeOut_SelectByBranchID",true,parameters);
List<SafeOut> lst = new List<SafeOut>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeOut>(dataTable);
}
return lst;
}

public void DeleteByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOut_DeleteByBranchID",true,parameters);
}
public void DeleteVirtualByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOut_DeleteVirtualByBranchID" ,true,parameters);
}

public List<SafeOut> SelectByTaxID(int TaxID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@TaxID", TaxID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeOut_SelectByTaxID",true,parameters);
List<SafeOut> lst = new List<SafeOut>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeOut>(dataTable);
}
return lst;
}

public void DeleteByTaxID(int TaxID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@TaxID", TaxID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOut_DeleteByTaxID",true,parameters);
}
public void DeleteVirtualByTaxID(int TaxID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@TaxID", TaxID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOut_DeleteVirtualByTaxID" ,true,parameters);
}

public List<SafeOut> SelectBySafeID(int SafeID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SafeID", SafeID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeOut_SelectBySafeID",true,parameters);
List<SafeOut> lst = new List<SafeOut>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeOut>(dataTable);
}
return lst;
}

public void DeleteBySafeID(int SafeID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SafeID", SafeID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOut_DeleteBySafeID",true,parameters);
}
public void DeleteVirtualBySafeID(int SafeID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SafeID", SafeID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOut_DeleteVirtualBySafeID" ,true,parameters);
}

}
}
