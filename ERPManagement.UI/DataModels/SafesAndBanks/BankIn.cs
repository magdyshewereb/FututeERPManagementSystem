using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataModels.Interfaces;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.SafesAndBanks
{
public class BankIn
{

public int BankInID { get; set; }

public string BankInNo { get; set; }

public DateTime BankInDate { get; set; }

public int CurrencyID { get; set; }

public decimal ExchangeRate { get; set; }

public bool  IsCheck { get; set; }

public string? CheckNo { get; set; }

public DateTime? CheckDate { get; set; }

public int BankID { get; set; }

public string ChargedPerson { get; set; }

public string? Notes { get; set; }

public decimal?  Total { get; set; }

public string? ReceivingBank { get; set; }

public int? BinderSafeID { get; set; }

public bool  UnderCollection { get; set; }

public DateTime? UnderCollectionDate { get; set; }

public bool  Collected { get; set; }

public bool  Returned { get; set; }

public bool  ToSafe { get; set; }

public int? SafeID { get; set; }

public bool  IsEndorsement { get; set; }

public int? AccountID { get; set; }

public int? SubAccountID { get; set; }

public DateTime? CollectionDate { get; set; }

public decimal?  CollectionExpense { get; set; }

public decimal?  SenderBankExpense { get; set; }

public int? SalesManID { get; set; }

public int? JvID { get; set; }

public int? JvID2 { get; set; }

public int? JvID3 { get; set; }

public bool  Approved { get; set; }

public int? BranchID { get; set; }

public bool  Deleted { get; set; }

public BankIn Clone()
{
return (BankIn)MemberwiseClone();
}
}
public class BankInService : IScopedService
    {
        public string GetCodeByBranchID(string Date, string BranchID, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@Date", Date == null ? DBNull.Value : Date);
            parameters[1] = new SqlParameter("@BranchID", BranchID == null ? DBNull.Value : BranchID);
            DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankIn_GetCodeByBranchID", true, parameters);

            return dataTable.Rows[0][0].ToString();
        }
        public DataTable Search(string BranchIDs, string FromDate, string ToDate, int Approved, int UnderCollection, int Collected, int Returned, int IsCheck, int Deleted, bool IsArabic, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[10];
            parameters[0] = new SqlParameter("@BranchIDs", BranchIDs);
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@Approved", Approved);
            parameters[4] = new SqlParameter("@UnderCollection", UnderCollection);
            parameters[5] = new SqlParameter("@Collected", Collected);
            parameters[6] = new SqlParameter("@Returned", Returned);
            parameters[8] = new SqlParameter("@Deleted", Deleted);
            parameters[9] = new SqlParameter("@IsArabic", IsArabic);
            parameters[7] = new SqlParameter("@IsCheck", IsCheck);
            DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankIn_Search", true, parameters);
            return dataTable;
        }
        public int Insert_Update(BankIn obj,int UserID,Main main)
{
SqlParameter[] parameters = new SqlParameter[34];
parameters[0] = new SqlParameter("@BankInID", obj.BankInID== null ? DBNull.Value : obj.BankInID);
parameters[1] = new SqlParameter("@BankInNo", obj.BankInNo== null ? DBNull.Value : obj.BankInNo);
parameters[2] = new SqlParameter("@BankInDate", obj.BankInDate== null ? DBNull.Value : obj.BankInDate);
parameters[3] = new SqlParameter("@CurrencyID", obj.CurrencyID== null ? DBNull.Value : obj.CurrencyID);
parameters[4] = new SqlParameter("@ExchangeRate", obj.ExchangeRate== null ? DBNull.Value : obj.ExchangeRate);
parameters[5] = new SqlParameter("@IsCheck", obj.IsCheck== null ? false : obj.IsCheck);
parameters[6] = new SqlParameter("@CheckNo", obj.CheckNo== null ? DBNull.Value : obj.CheckNo);
parameters[7] = new SqlParameter("@CheckDate", obj.CheckDate== null ? DBNull.Value : obj.CheckDate);
parameters[8] = new SqlParameter("@BankID", obj.BankID== null ? DBNull.Value : obj.BankID);
parameters[9] = new SqlParameter("@ChargedPerson", obj.ChargedPerson== null ? DBNull.Value : obj.ChargedPerson);
parameters[10] = new SqlParameter("@Notes", obj.Notes== null ? DBNull.Value : obj.Notes);
parameters[11] = new SqlParameter("@Total", obj.Total== null ? DBNull.Value : obj.Total);
parameters[12] = new SqlParameter("@ReceivingBank", obj.ReceivingBank== null ? DBNull.Value : obj.ReceivingBank);
parameters[13] = new SqlParameter("@BinderSafeID", obj.BinderSafeID== null ? DBNull.Value : obj.BinderSafeID);
parameters[14] = new SqlParameter("@UnderCollection", obj.UnderCollection== null ? false : obj.UnderCollection);
parameters[15] = new SqlParameter("@UnderCollectionDate", obj.UnderCollectionDate== null ? DBNull.Value : obj.UnderCollectionDate);
parameters[16] = new SqlParameter("@Collected", obj.Collected== null ? false : obj.Collected);
parameters[17] = new SqlParameter("@Returned", obj.Returned== null ? false : obj.Returned);
parameters[18] = new SqlParameter("@ToSafe", obj.ToSafe== null ? false : obj.ToSafe);
parameters[19] = new SqlParameter("@SafeID", obj.SafeID== null ? DBNull.Value : obj.SafeID);
parameters[20] = new SqlParameter("@IsEndorsement", obj.IsEndorsement== null ? false : obj.IsEndorsement);
parameters[21] = new SqlParameter("@AccountID", obj.AccountID== null ? DBNull.Value : obj.AccountID);
parameters[22] = new SqlParameter("@SubAccountID", obj.SubAccountID== null ? DBNull.Value : obj.SubAccountID);
parameters[23] = new SqlParameter("@CollectionDate", obj.CollectionDate== null ? DBNull.Value : obj.CollectionDate);
parameters[24] = new SqlParameter("@CollectionExpense", obj.CollectionExpense== null ? DBNull.Value : obj.CollectionExpense);
parameters[25] = new SqlParameter("@SenderBankExpense", obj.SenderBankExpense== null ? DBNull.Value : obj.SenderBankExpense);
parameters[26] = new SqlParameter("@SalesManID", obj.SalesManID== null ? DBNull.Value : obj.SalesManID);
parameters[27] = new SqlParameter("@JvID", obj.JvID== null ? DBNull.Value : obj.JvID);
parameters[28] = new SqlParameter("@JvID2", obj.JvID2== null ? DBNull.Value : obj.JvID2);
parameters[29] = new SqlParameter("@JvID3", obj.JvID3== null ? DBNull.Value : obj.JvID3);
parameters[30] = new SqlParameter("@Approved", obj.Approved== null ? false : obj.Approved);
parameters[32] = new SqlParameter("@BranchID", obj.BranchID== null ? DBNull.Value : obj.BranchID);
parameters[33] = new SqlParameter("@Deleted", obj.Deleted== null ? false : obj.Deleted);
parameters[33] = new SqlParameter("@UserID",UserID);
DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankIn_Insert_Update",true,parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}

public DataTable Select(int BankInID, string BranchIDs, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[3];
parameters[0] = new SqlParameter("@BankInID", BankInID);
parameters[1] = new SqlParameter("@BranchIDs",BranchIDs);
parameters[2] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable =main.ExecuteQuery_DataTable("SB_BankIn_Select",true,parameters);
//List<BankIn> lst = new List<BankIn>();
//if (dataTable != null)
//{
// lst =main.CreateListFromTable<BankIn>(dataTable);
//}
return dataTable;
}

public void Delete(int BankInID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BankInID", BankInID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankIn_Delete",true,parameters);
}
public void DeleteVirtual(int BankInID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BankInID", BankInID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankIn_DeleteVirtual",true,parameters);
}


public List<BankIn> SelectByAccountID(int AccountID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@AccountID", AccountID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankIn_SelectByAccountID",true,parameters);
List<BankIn> lst = new List<BankIn>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<BankIn>(dataTable);
}
return lst;
}

public void DeleteByAccountID(int AccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@AccountID", AccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankIn_DeleteByAccountID",true,parameters);
}
public void DeleteVirtualByAccountID(int AccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@AccountID", AccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankIn_DeleteVirtualByAccountID" ,true,parameters);
}

public List<BankIn> SelectByFiscalYearID(int FiscalYearID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@FiscalYearID", FiscalYearID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankIn_SelectByFiscalYearID",true,parameters);
List<BankIn> lst = new List<BankIn>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<BankIn>(dataTable);
}
return lst;
}

public void DeleteByFiscalYearID(int FiscalYearID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@FiscalYearID", FiscalYearID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankIn_DeleteByFiscalYearID",true,parameters);
}
public void DeleteVirtualByFiscalYearID(int FiscalYearID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@FiscalYearID", FiscalYearID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankIn_DeleteVirtualByFiscalYearID" ,true,parameters);
}

public List<BankIn> SelectByJvID(int JvID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JvID", JvID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankIn_SelectByJvID",true,parameters);
List<BankIn> lst = new List<BankIn>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<BankIn>(dataTable);
}
return lst;
}

public void DeleteByJvID(int JvID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JvID", JvID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankIn_DeleteByJvID",true,parameters);
}
public void DeleteVirtualByJvID(int JvID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JvID", JvID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankIn_DeleteVirtualByJvID" ,true,parameters);
}

public List<BankIn> SelectByJvID2(int JvID2, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JvID2", JvID2);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankIn_SelectByJvID2",true,parameters);
List<BankIn> lst = new List<BankIn>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<BankIn>(dataTable);
}
return lst;
}

public void DeleteByJvID2(int JvID2, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JvID2", JvID2);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankIn_DeleteByJvID2",true,parameters);
}
public void DeleteVirtualByJvID2(int JvID2, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JvID2", JvID2);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankIn_DeleteVirtualByJvID2" ,true,parameters);
}

public List<BankIn> SelectByJvID3(int JvID3, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JvID3", JvID3);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankIn_SelectByJvID3",true,parameters);
List<BankIn> lst = new List<BankIn>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<BankIn>(dataTable);
}
return lst;
}

public void DeleteByJvID3(int JvID3, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JvID3", JvID3);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankIn_DeleteByJvID3",true,parameters);
}
public void DeleteVirtualByJvID3(int JvID3, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JvID3", JvID3);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankIn_DeleteVirtualByJvID3" ,true,parameters);
}

public List<BankIn> SelectBySubAccountID(int SubAccountID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankIn_SelectBySubAccountID",true,parameters);
List<BankIn> lst = new List<BankIn>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<BankIn>(dataTable);
}
return lst;
}

public void DeleteBySubAccountID(int SubAccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankIn_DeleteBySubAccountID",true,parameters);
}
public void DeleteVirtualBySubAccountID(int SubAccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankIn_DeleteVirtualBySubAccountID" ,true,parameters);
}

public List<BankIn> SelectByCurrencyID(int CurrencyID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CurrencyID", CurrencyID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankIn_SelectByCurrencyID",true,parameters);
List<BankIn> lst = new List<BankIn>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<BankIn>(dataTable);
}
return lst;
}

public void DeleteByCurrencyID(int CurrencyID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CurrencyID", CurrencyID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankIn_DeleteByCurrencyID",true,parameters);
}
public void DeleteVirtualByCurrencyID(int CurrencyID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CurrencyID", CurrencyID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankIn_DeleteVirtualByCurrencyID" ,true,parameters);
}

public List<BankIn> SelectByBranchID(int BranchID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankIn_SelectByBranchID",true,parameters);
List<BankIn> lst = new List<BankIn>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<BankIn>(dataTable);
}
return lst;
}

public void DeleteByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankIn_DeleteByBranchID",true,parameters);
}
public void DeleteVirtualByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankIn_DeleteVirtualByBranchID" ,true,parameters);
}

public List<BankIn> SelectByBankID(int BankID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BankID", BankID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankIn_SelectByBankID",true,parameters);
List<BankIn> lst = new List<BankIn>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<BankIn>(dataTable);
}
return lst;
}

public void DeleteByBankID(int BankID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BankID", BankID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankIn_DeleteByBankID",true,parameters);
}
public void DeleteVirtualByBankID(int BankID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BankID", BankID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankIn_DeleteVirtualByBankID" ,true,parameters);
}

public List<BankIn> SelectBySafeID(int SafeID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SafeID", SafeID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankIn_SelectBySafeID",true,parameters);
List<BankIn> lst = new List<BankIn>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<BankIn>(dataTable);
}
return lst;
}

public void DeleteBySafeID(int SafeID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SafeID", SafeID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankIn_DeleteBySafeID",true,parameters);
}
public void DeleteVirtualBySafeID(int SafeID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SafeID", SafeID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankIn_DeleteVirtualBySafeID" ,true,parameters);
}

public List<BankIn> SelectByBinderSafeID(int BinderSafeID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BinderSafeID", BinderSafeID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankIn_SelectByBinderSafeID",true,parameters);
List<BankIn> lst = new List<BankIn>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<BankIn>(dataTable);
}
return lst;
}

public void DeleteByBinderSafeID(int BinderSafeID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BinderSafeID", BinderSafeID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankIn_DeleteByBinderSafeID",true,parameters);
}
public void DeleteVirtualByBinderSafeID(int BinderSafeID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BinderSafeID", BinderSafeID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankIn_DeleteVirtualByBinderSafeID" ,true,parameters);
}

}
}
