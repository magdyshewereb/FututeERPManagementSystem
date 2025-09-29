using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataModels.Interfaces;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.SafesAndBanks
{
public class Banks
{

public int BankID { get; set; }

public string? BankCode { get; set; }

public string BankNameAr { get; set; }

public string? BankNameEn { get; set; }

public string BankAccNumber { get; set; }

public int? CurrencyID { get; set; }

public int AccountID { get; set; }

public int? SubAccountID { get; set; }

public int NPAccID { get; set; }

public int NPUnderCollectionAccID { get; set; }

public int NRAccID { get; set; }

public int NRUnderCollectionAccID { get; set; }

public int? CollectionExpensesAccID { get; set; }

public int? SenderBankExpensesAccID { get; set; }

public decimal?  BankMinimumLimit { get; set; }

public string? BankDescription { get; set; }

public int? BranchID { get; set; }

public bool  Deleted { get; set; }

public Banks Clone()
{
return (Banks)MemberwiseClone();
}
}
public class BanksService : IScopedService
    {


public int Insert_Update(Banks obj,int UserID,Main main)
{
SqlParameter[] parameters = new SqlParameter[19];
parameters[0] = new SqlParameter("@BankID", obj.BankID== null ? DBNull.Value : obj.BankID);
parameters[1] = new SqlParameter("@BankCode", obj.BankCode== null ? DBNull.Value : obj.BankCode);
parameters[2] = new SqlParameter("@BankNameAr", obj.BankNameAr== null ? DBNull.Value : obj.BankNameAr);
parameters[3] = new SqlParameter("@BankNameEn", obj.BankNameEn== null ? DBNull.Value : obj.BankNameEn);
parameters[4] = new SqlParameter("@BankAccNumber", obj.BankAccNumber== null ? DBNull.Value : obj.BankAccNumber);
parameters[5] = new SqlParameter("@CurrencyID", obj.CurrencyID== null ? DBNull.Value : obj.CurrencyID);
parameters[6] = new SqlParameter("@AccountID", obj.AccountID== null ? DBNull.Value : obj.AccountID);
parameters[7] = new SqlParameter("@SubAccountID", obj.SubAccountID== null ? DBNull.Value : obj.SubAccountID);
parameters[8] = new SqlParameter("@NPAccID", obj.NPAccID== null ? DBNull.Value : obj.NPAccID);
parameters[9] = new SqlParameter("@NPUnderCollectionAccID", obj.NPUnderCollectionAccID== null ? DBNull.Value : obj.NPUnderCollectionAccID);
parameters[10] = new SqlParameter("@NRAccID", obj.NRAccID== null ? DBNull.Value : obj.NRAccID);
parameters[11] = new SqlParameter("@NRUnderCollectionAccID", obj.NRUnderCollectionAccID== null ? DBNull.Value : obj.NRUnderCollectionAccID);
parameters[12] = new SqlParameter("@CollectionExpensesAccID", obj.CollectionExpensesAccID== null ? DBNull.Value : obj.CollectionExpensesAccID);
parameters[13] = new SqlParameter("@SenderBankExpensesAccID", obj.SenderBankExpensesAccID== null ? DBNull.Value : obj.SenderBankExpensesAccID);
parameters[14] = new SqlParameter("@BankMinimumLimit", obj.BankMinimumLimit== null ? DBNull.Value : obj.BankMinimumLimit);
parameters[15] = new SqlParameter("@BankDescription", obj.BankDescription== null ? DBNull.Value : obj.BankDescription);
parameters[16] = new SqlParameter("@BranchID", obj.BranchID== null ? DBNull.Value : obj.BranchID);
parameters[17] = new SqlParameter("@Deleted", obj.Deleted== null ? false : obj.Deleted);
parameters[18] = new SqlParameter("@UserID",UserID);
DataTable dataTable = main.ExecuteQuery_DataTable("SB_Banks_Insert_Update",true,parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}
        public DataTable FillCombo(bool IsArabic, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@IsArabic", IsArabic);
            DataTable dataTable = main.ExecuteQuery_DataTable("SB_Banks_FillCombo", true, parameters);
            return dataTable;
        }
        public List<Banks> Select(int BankID, string BranchIDs, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[3];
parameters[0] = new SqlParameter("@BankID", BankID);
parameters[1] = new SqlParameter("@BranchIDs",BranchIDs);
parameters[2] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable =main.ExecuteQuery_DataTable("SB_Banks_Select",true,parameters);
List<Banks> lst = new List<Banks>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<Banks>(dataTable);
}
return lst;
}

public void Delete(int BankID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BankID", BankID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_Banks_Delete",true,parameters);
}
public void DeleteVirtual(int BankID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BankID", BankID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_Banks_DeleteVirtual",true,parameters);
}


public List<Banks> SelectByCollectionExpensesAccID(int CollectionExpensesAccID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CollectionExpensesAccID", CollectionExpensesAccID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_Banks_SelectByCollectionExpensesAccID",true,parameters);
List<Banks> lst = new List<Banks>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<Banks>(dataTable);
}
return lst;
}

public void DeleteByCollectionExpensesAccID(int CollectionExpensesAccID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CollectionExpensesAccID", CollectionExpensesAccID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_Banks_DeleteByCollectionExpensesAccID",true,parameters);
}
public void DeleteVirtualByCollectionExpensesAccID(int CollectionExpensesAccID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CollectionExpensesAccID", CollectionExpensesAccID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_Banks_DeleteVirtualByCollectionExpensesAccID" ,true,parameters);
}

public List<Banks> SelectByNPAccID(int NPAccID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@NPAccID", NPAccID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_Banks_SelectByNPAccID",true,parameters);
List<Banks> lst = new List<Banks>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<Banks>(dataTable);
}
return lst;
}

public void DeleteByNPAccID(int NPAccID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@NPAccID", NPAccID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_Banks_DeleteByNPAccID",true,parameters);
}
public void DeleteVirtualByNPAccID(int NPAccID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@NPAccID", NPAccID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_Banks_DeleteVirtualByNPAccID" ,true,parameters);
}

public List<Banks> SelectByNPUnderCollectionAccID(int NPUnderCollectionAccID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@NPUnderCollectionAccID", NPUnderCollectionAccID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_Banks_SelectByNPUnderCollectionAccID",true,parameters);
List<Banks> lst = new List<Banks>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<Banks>(dataTable);
}
return lst;
}

public void DeleteByNPUnderCollectionAccID(int NPUnderCollectionAccID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@NPUnderCollectionAccID", NPUnderCollectionAccID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_Banks_DeleteByNPUnderCollectionAccID",true,parameters);
}
public void DeleteVirtualByNPUnderCollectionAccID(int NPUnderCollectionAccID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@NPUnderCollectionAccID", NPUnderCollectionAccID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_Banks_DeleteVirtualByNPUnderCollectionAccID" ,true,parameters);
}

public List<Banks> SelectByNRAccID(int NRAccID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@NRAccID", NRAccID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_Banks_SelectByNRAccID",true,parameters);
List<Banks> lst = new List<Banks>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<Banks>(dataTable);
}
return lst;
}

public void DeleteByNRAccID(int NRAccID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@NRAccID", NRAccID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_Banks_DeleteByNRAccID",true,parameters);
}
public void DeleteVirtualByNRAccID(int NRAccID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@NRAccID", NRAccID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_Banks_DeleteVirtualByNRAccID" ,true,parameters);
}

public List<Banks> SelectByNRUnderCollectionAccID(int NRUnderCollectionAccID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@NRUnderCollectionAccID", NRUnderCollectionAccID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_Banks_SelectByNRUnderCollectionAccID",true,parameters);
List<Banks> lst = new List<Banks>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<Banks>(dataTable);
}
return lst;
}

public void DeleteByNRUnderCollectionAccID(int NRUnderCollectionAccID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@NRUnderCollectionAccID", NRUnderCollectionAccID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_Banks_DeleteByNRUnderCollectionAccID",true,parameters);
}
public void DeleteVirtualByNRUnderCollectionAccID(int NRUnderCollectionAccID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@NRUnderCollectionAccID", NRUnderCollectionAccID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_Banks_DeleteVirtualByNRUnderCollectionAccID" ,true,parameters);
}

public List<Banks> SelectByAccountID(int AccountID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@AccountID", AccountID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_Banks_SelectByAccountID",true,parameters);
List<Banks> lst = new List<Banks>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<Banks>(dataTable);
}
return lst;
}

public void DeleteByAccountID(int AccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@AccountID", AccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_Banks_DeleteByAccountID",true,parameters);
}
public void DeleteVirtualByAccountID(int AccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@AccountID", AccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_Banks_DeleteVirtualByAccountID" ,true,parameters);
}

public List<Banks> SelectBySubAccountID(int SubAccountID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_Banks_SelectBySubAccountID",true,parameters);
List<Banks> lst = new List<Banks>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<Banks>(dataTable);
}
return lst;
}

public void DeleteBySubAccountID(int SubAccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_Banks_DeleteBySubAccountID",true,parameters);
}
public void DeleteVirtualBySubAccountID(int SubAccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_Banks_DeleteVirtualBySubAccountID" ,true,parameters);
}

public List<Banks> SelectByBranchID(int BranchID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_Banks_SelectByBranchID",true,parameters);
List<Banks> lst = new List<Banks>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<Banks>(dataTable);
}
return lst;
}

public void DeleteByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_Banks_DeleteByBranchID",true,parameters);
}
public void DeleteVirtualByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_Banks_DeleteVirtualByBranchID" ,true,parameters);
}

public List<Banks> SelectByCurrencyID(int CurrencyID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CurrencyID", CurrencyID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_Banks_SelectByCurrencyID",true,parameters);
List<Banks> lst = new List<Banks>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<Banks>(dataTable);
}
return lst;
}

public void DeleteByCurrencyID(int CurrencyID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CurrencyID", CurrencyID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_Banks_DeleteByCurrencyID",true,parameters);
}
public void DeleteVirtualByCurrencyID(int CurrencyID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CurrencyID", CurrencyID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_Banks_DeleteVirtualByCurrencyID" ,true,parameters);
}

public List<Banks> SelectBySenderBankExpensesAccID(int SenderBankExpensesAccID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SenderBankExpensesAccID", SenderBankExpensesAccID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_Banks_SelectBySenderBankExpensesAccID",true,parameters);
List<Banks> lst = new List<Banks>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<Banks>(dataTable);
}
return lst;
}

public void DeleteBySenderBankExpensesAccID(int SenderBankExpensesAccID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SenderBankExpensesAccID", SenderBankExpensesAccID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_Banks_DeleteBySenderBankExpensesAccID",true,parameters);
}
public void DeleteVirtualBySenderBankExpensesAccID(int SenderBankExpensesAccID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SenderBankExpensesAccID", SenderBankExpensesAccID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_Banks_DeleteVirtualBySenderBankExpensesAccID" ,true,parameters);
}

}
}
