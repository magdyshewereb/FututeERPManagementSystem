using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataModels.Interfaces;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.SafesAndBanks
{
public class BankOut
{

public int BankOutID { get; set; }

public string BankOutNo { get; set; }

public DateTime BankOutDate { get; set; }

public int CurrencyID { get; set; }

public decimal ExchangeRate { get; set; }

public bool  IsRequest { get; set; }

public bool  IsCheck { get; set; }

public string? CheckNo { get; set; }

public DateTime? CheckDate { get; set; }

public int? BankID { get; set; }

public string ChargedPerson { get; set; }

public string? Notes { get; set; }

public decimal?  Total { get; set; }

public string? Payingbank { get; set; }

public int? BinderSafeID { get; set; }

public int? TaxID { get; set; }

public decimal?  TaxValue { get; set; }

public decimal?  TotalAfterTax { get; set; }

public bool  UnderCollection { get; set; }

public DateTime? UnderCollectionDate { get; set; }

public bool  Collected { get; set; }

public bool  Returned { get; set; }

public DateTime? CollectionDate { get; set; }

public decimal?  CollectionExpense { get; set; }

public int? JvID { get; set; }

public int? JvID2 { get; set; }

public int? JvID3 { get; set; }

public bool  Approved { get; set; }

public int? BranchID { get; set; }

public bool  Deleted { get; set; }

public BankOut Clone()
{
return (BankOut)MemberwiseClone();
}
}
public class BankOutService : IScopedService
    {


public int Insert_Update(BankOut obj,int UserID,Main main)
{
SqlParameter[] parameters = new SqlParameter[31];
parameters[0] = new SqlParameter("@BankOutID", obj.BankOutID== null ? DBNull.Value : obj.BankOutID);
parameters[1] = new SqlParameter("@BankOutNo", obj.BankOutNo== null ? DBNull.Value : obj.BankOutNo);
parameters[2] = new SqlParameter("@BankOutDate", obj.BankOutDate== null ? DBNull.Value : obj.BankOutDate);
parameters[3] = new SqlParameter("@CurrencyID", obj.CurrencyID== null ? DBNull.Value : obj.CurrencyID);
parameters[4] = new SqlParameter("@ExchangeRate", obj.ExchangeRate== null ? DBNull.Value : obj.ExchangeRate);
parameters[5] = new SqlParameter("@IsRequest", obj.IsRequest== null ? false : obj.IsRequest);
parameters[6] = new SqlParameter("@IsCheck", obj.IsCheck== null ? false : obj.IsCheck);
parameters[7] = new SqlParameter("@CheckNo", obj.CheckNo== null ? DBNull.Value : obj.CheckNo);
parameters[8] = new SqlParameter("@CheckDate", obj.CheckDate== null ? DBNull.Value : obj.CheckDate);
parameters[9] = new SqlParameter("@BankID", obj.BankID== null ? DBNull.Value : obj.BankID);
parameters[10] = new SqlParameter("@ChargedPerson", obj.ChargedPerson== null ? DBNull.Value : obj.ChargedPerson);
parameters[11] = new SqlParameter("@Notes", obj.Notes== null ? DBNull.Value : obj.Notes);
parameters[12] = new SqlParameter("@Total", obj.Total== null ? DBNull.Value : obj.Total);
parameters[13] = new SqlParameter("@Payingbank", obj.Payingbank== null ? DBNull.Value : obj.Payingbank);
parameters[14] = new SqlParameter("@BinderSafeID", obj.BinderSafeID== null ? DBNull.Value : obj.BinderSafeID);
parameters[15] = new SqlParameter("@TaxID", obj.TaxID== null ? DBNull.Value : obj.TaxID);
parameters[16] = new SqlParameter("@TaxValue", obj.TaxValue== null ? DBNull.Value : obj.TaxValue);
parameters[17] = new SqlParameter("@TotalAfterTax", obj.TotalAfterTax== null ? DBNull.Value : obj.TotalAfterTax);
parameters[18] = new SqlParameter("@UnderCollection", obj.UnderCollection== null ? false : obj.UnderCollection);
parameters[19] = new SqlParameter("@UnderCollectionDate", obj.UnderCollectionDate== null ? DBNull.Value : obj.UnderCollectionDate);
parameters[20] = new SqlParameter("@Collected", obj.Collected== null ? false : obj.Collected);
parameters[21] = new SqlParameter("@Returned", obj.Returned== null ? false : obj.Returned);
parameters[22] = new SqlParameter("@CollectionDate", obj.CollectionDate== null ? DBNull.Value : obj.CollectionDate);
parameters[23] = new SqlParameter("@CollectionExpense", obj.CollectionExpense== null ? DBNull.Value : obj.CollectionExpense);
parameters[24] = new SqlParameter("@JvID", obj.JvID== null ? DBNull.Value : obj.JvID);
parameters[25] = new SqlParameter("@JvID2", obj.JvID2== null ? DBNull.Value : obj.JvID2);
parameters[26] = new SqlParameter("@JvID3", obj.JvID3== null ? DBNull.Value : obj.JvID3);
parameters[27] = new SqlParameter("@Approved", obj.Approved== null ? false : obj.Approved);
parameters[29] = new SqlParameter("@BranchID", obj.BranchID== null ? DBNull.Value : obj.BranchID);
parameters[30] = new SqlParameter("@Deleted", obj.Deleted== null ? false : obj.Deleted);
parameters[30] = new SqlParameter("@UserID",UserID);
DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankOut_Insert_Update",true,parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}

public List<BankOut> Select(int BankOutID, string BranchIDs, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[3];
parameters[0] = new SqlParameter("@BankOutID", BankOutID);
parameters[1] = new SqlParameter("@BranchIDs",BranchIDs);
parameters[2] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable =main.ExecuteQuery_DataTable("SB_BankOut_Select",true,parameters);
List<BankOut> lst = new List<BankOut>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<BankOut>(dataTable);
}
return lst;
}

public void Delete(int BankOutID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BankOutID", BankOutID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankOut_Delete",true,parameters);
}
public void DeleteVirtual(int BankOutID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BankOutID", BankOutID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankOut_DeleteVirtual",true,parameters);
}


public List<BankOut> SelectByFiscalYearID(int FiscalYearID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@FiscalYearID", FiscalYearID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankOut_SelectByFiscalYearID",true,parameters);
List<BankOut> lst = new List<BankOut>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<BankOut>(dataTable);
}
return lst;
}

public void DeleteByFiscalYearID(int FiscalYearID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@FiscalYearID", FiscalYearID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankOut_DeleteByFiscalYearID",true,parameters);
}
public void DeleteVirtualByFiscalYearID(int FiscalYearID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@FiscalYearID", FiscalYearID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankOut_DeleteVirtualByFiscalYearID" ,true,parameters);
}

public List<BankOut> SelectByJvID(int JvID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JvID", JvID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankOut_SelectByJvID",true,parameters);
List<BankOut> lst = new List<BankOut>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<BankOut>(dataTable);
}
return lst;
}

public void DeleteByJvID(int JvID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JvID", JvID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankOut_DeleteByJvID",true,parameters);
}
public void DeleteVirtualByJvID(int JvID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JvID", JvID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankOut_DeleteVirtualByJvID" ,true,parameters);
}

public List<BankOut> SelectByJvID2(int JvID2, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JvID2", JvID2);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankOut_SelectByJvID2",true,parameters);
List<BankOut> lst = new List<BankOut>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<BankOut>(dataTable);
}
return lst;
}

public void DeleteByJvID2(int JvID2, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JvID2", JvID2);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankOut_DeleteByJvID2",true,parameters);
}
public void DeleteVirtualByJvID2(int JvID2, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JvID2", JvID2);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankOut_DeleteVirtualByJvID2" ,true,parameters);
}

public List<BankOut> SelectByJvID3(int JvID3, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JvID3", JvID3);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankOut_SelectByJvID3",true,parameters);
List<BankOut> lst = new List<BankOut>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<BankOut>(dataTable);
}
return lst;
}

public void DeleteByJvID3(int JvID3, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JvID3", JvID3);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankOut_DeleteByJvID3",true,parameters);
}
public void DeleteVirtualByJvID3(int JvID3, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JvID3", JvID3);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankOut_DeleteVirtualByJvID3" ,true,parameters);
}

public List<BankOut> SelectByCurrencyID(int CurrencyID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CurrencyID", CurrencyID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankOut_SelectByCurrencyID",true,parameters);
List<BankOut> lst = new List<BankOut>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<BankOut>(dataTable);
}
return lst;
}

public void DeleteByCurrencyID(int CurrencyID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CurrencyID", CurrencyID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankOut_DeleteByCurrencyID",true,parameters);
}
public void DeleteVirtualByCurrencyID(int CurrencyID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CurrencyID", CurrencyID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankOut_DeleteVirtualByCurrencyID" ,true,parameters);
}

public List<BankOut> SelectByBranchID(int BranchID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankOut_SelectByBranchID",true,parameters);
List<BankOut> lst = new List<BankOut>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<BankOut>(dataTable);
}
return lst;
}

public void DeleteByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankOut_DeleteByBranchID",true,parameters);
}
public void DeleteVirtualByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankOut_DeleteVirtualByBranchID" ,true,parameters);
}

public List<BankOut> SelectByTaxID(int TaxID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@TaxID", TaxID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankOut_SelectByTaxID",true,parameters);
List<BankOut> lst = new List<BankOut>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<BankOut>(dataTable);
}
return lst;
}

public void DeleteByTaxID(int TaxID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@TaxID", TaxID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankOut_DeleteByTaxID",true,parameters);
}
public void DeleteVirtualByTaxID(int TaxID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@TaxID", TaxID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankOut_DeleteVirtualByTaxID" ,true,parameters);
}

public List<BankOut> SelectByBankID(int BankID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BankID", BankID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankOut_SelectByBankID",true,parameters);
List<BankOut> lst = new List<BankOut>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<BankOut>(dataTable);
}
return lst;
}

public void DeleteByBankID(int BankID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BankID", BankID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankOut_DeleteByBankID",true,parameters);
}
public void DeleteVirtualByBankID(int BankID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BankID", BankID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankOut_DeleteVirtualByBankID" ,true,parameters);
}

public List<BankOut> SelectByBinderSafeID(int BinderSafeID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BinderSafeID", BinderSafeID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankOut_SelectByBinderSafeID",true,parameters);
List<BankOut> lst = new List<BankOut>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<BankOut>(dataTable);
}
return lst;
}

public void DeleteByBinderSafeID(int BinderSafeID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BinderSafeID", BinderSafeID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankOut_DeleteByBinderSafeID",true,parameters);
}
public void DeleteVirtualByBinderSafeID(int BinderSafeID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BinderSafeID", BinderSafeID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankOut_DeleteVirtualByBinderSafeID" ,true,parameters);
}

}
}
