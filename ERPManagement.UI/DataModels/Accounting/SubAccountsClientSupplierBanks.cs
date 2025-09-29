using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.Accounting
{
public class SubAccountsClientSupplierBanks
{

public int ClientSupplierBankID { get; set; }

public int? SubAccountID { get; set; }

public string? BankName { get; set; }

public string? Address { get; set; }

public string? BranchCode { get; set; }

public string? AccountNumber { get; set; }

public string? AccountName { get; set; }

public string? SwiftCode { get; set; }

public bool  Deleted { get; set; }

public int BranchID { get; set; }

public SubAccountsClientSupplierBanks Clone()
{
return (SubAccountsClientSupplierBanks)MemberwiseClone();
}
}
public class SubAccountsClientSupplierBanksService : Interfaces.IScopedService
{

        public int Insert_UpdateByTableXML(List<SubAccountsClientSupplierBanks> lst, int SubAccountID, int BranchID, int UserID, Main main)
        {
            string lstXML = main.ToXml(lst);
            string strSql = string.Empty;
            strSql += " A_SubAccountsClientSupplierBanks_Insert_UpdateXML '";
            strSql += lstXML + "',";
            strSql += SubAccountID + ",";
            strSql += BranchID + ",";
            strSql += UserID;

            DataTable dataTable = main.ExecuteQuery_DataTable(strSql, false, null);
            return int.Parse(dataTable.Rows[0][0].ToString());
        }
        public int Insert_Update(SubAccountsClientSupplierBanks obj,int UserID,Main main)
{
SqlParameter[] parameters = new SqlParameter[11];
parameters[0] = new SqlParameter("@ClientSupplierBankID", obj.ClientSupplierBankID== null ? DBNull.Value : obj.ClientSupplierBankID);
parameters[1] = new SqlParameter("@SubAccountID", obj.SubAccountID== null ? DBNull.Value : obj.SubAccountID);
parameters[2] = new SqlParameter("@BankName", obj.BankName== null ? DBNull.Value : obj.BankName);
parameters[3] = new SqlParameter("@Address", obj.Address== null ? DBNull.Value : obj.Address);
parameters[4] = new SqlParameter("@BranchCode", obj.BranchCode== null ? DBNull.Value : obj.BranchCode);
parameters[5] = new SqlParameter("@AccountNumber", obj.AccountNumber== null ? DBNull.Value : obj.AccountNumber);
parameters[6] = new SqlParameter("@AccountName", obj.AccountName== null ? DBNull.Value : obj.AccountName);
parameters[7] = new SqlParameter("@SwiftCode", obj.SwiftCode== null ? DBNull.Value : obj.SwiftCode);
parameters[8] = new SqlParameter("@Deleted", obj.Deleted== null ? false : obj.Deleted);
parameters[9] = new SqlParameter("@BranchID", obj.BranchID== null ? DBNull.Value : obj.BranchID);
parameters[10] = new SqlParameter("@UserID",UserID);
DataTable dataTable = main.ExecuteQuery_DataTable("A_SubAccountsClientSupplierBanks_Insert_Update",true,parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}

public List<SubAccountsClientSupplierBanks> Select(int ClientSupplierBankID, string BranchIDs, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[3];
parameters[0] = new SqlParameter("@ClientSupplierBankID", ClientSupplierBankID);
parameters[1] = new SqlParameter("@BranchIDs",BranchIDs);
parameters[2] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable =main.ExecuteQuery_DataTable("A_SubAccountsClientSupplierBanks_Select",true,parameters);
List<SubAccountsClientSupplierBanks> lst = new List<SubAccountsClientSupplierBanks>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SubAccountsClientSupplierBanks>(dataTable);
}
return lst;
}

public void Delete(int ClientSupplierBankID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@ClientSupplierBankID", ClientSupplierBankID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_SubAccountsClientSupplierBanks_Delete",true,parameters);
}
public void DeleteVirtual(int ClientSupplierBankID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@ClientSupplierBankID", ClientSupplierBankID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_SubAccountsClientSupplierBanks_DeleteVirtual",true,parameters);
}


public List<SubAccountsClientSupplierBanks> SelectBySubAccountID(int SubAccountID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("A_SubAccountsClientSupplierBanks_SelectBySubAccountID",true,parameters);
List<SubAccountsClientSupplierBanks> lst = new List<SubAccountsClientSupplierBanks>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SubAccountsClientSupplierBanks>(dataTable);
}
return lst;
}

public void DeleteBySubAccountID(int SubAccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_SubAccountsClientSupplierBanks_DeleteBySubAccountID",true,parameters);
}
public void DeleteVirtualBySubAccountID(int SubAccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_SubAccountsClientSupplierBanks_DeleteVirtualBySubAccountID" ,true,parameters);
}

public List<SubAccountsClientSupplierBanks> SelectByBranchID(int BranchID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("A_SubAccountsClientSupplierBanks_SelectByBranchID",true,parameters);
List<SubAccountsClientSupplierBanks> lst = new List<SubAccountsClientSupplierBanks>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SubAccountsClientSupplierBanks>(dataTable);
}
return lst;
}

public void DeleteByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_SubAccountsClientSupplierBanks_DeleteByBranchID",true,parameters);
}
public void DeleteVirtualByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_SubAccountsClientSupplierBanks_DeleteVirtualByBranchID" ,true,parameters);
}

}
}
