using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataModels.Interfaces;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.SafesAndBanks
{
public class BankOutDetails
{

public int BankOutDetailsID { get; set; }

public int BankOutID { get; set; }

public decimal?  Value { get; set; }

public int AccountID { get; set; }

public int? SubAccountID { get; set; }

public int? CostCenterID { get; set; }

public int? SubCostCenterID { get; set; }

public bool? IsDocumented { get; set; }

public string? Notes { get; set; }

public int? BranchID { get; set; }

public bool  Deleted { get; set; }

public BankOutDetails Clone()
{
return (BankOutDetails)MemberwiseClone();
}
}
public class BankOutDetailsService : IScopedService
    {


public int Insert_Update(BankOutDetails obj,int UserID,Main main)
{
SqlParameter[] parameters = new SqlParameter[12];
parameters[0] = new SqlParameter("@BankOutDetailsID", obj.BankOutDetailsID== null ? DBNull.Value : obj.BankOutDetailsID);
parameters[1] = new SqlParameter("@BankOutID", obj.BankOutID== null ? DBNull.Value : obj.BankOutID);
parameters[2] = new SqlParameter("@Value", obj.Value== null ? DBNull.Value : obj.Value);
parameters[3] = new SqlParameter("@AccountID", obj.AccountID== null ? DBNull.Value : obj.AccountID);
parameters[4] = new SqlParameter("@SubAccountID", obj.SubAccountID== null ? DBNull.Value : obj.SubAccountID);
parameters[5] = new SqlParameter("@CostCenterID", obj.CostCenterID== null ? DBNull.Value : obj.CostCenterID);
parameters[6] = new SqlParameter("@SubCostCenterID", obj.SubCostCenterID== null ? DBNull.Value : obj.SubCostCenterID);
parameters[7] = new SqlParameter("@IsDocumented", obj.IsDocumented== null ? false : obj.IsDocumented);
parameters[8] = new SqlParameter("@Notes", obj.Notes== null ? DBNull.Value : obj.Notes);
parameters[9] = new SqlParameter("@BranchID", obj.BranchID== null ? DBNull.Value : obj.BranchID);
parameters[10] = new SqlParameter("@Deleted", obj.Deleted== null ? false : obj.Deleted);
parameters[11] = new SqlParameter("@UserID",UserID);
DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankOutDetails_Insert_Update",true,parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}

public List<BankOutDetails> Select(int BankOutDetailsID, string BranchIDs, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[3];
parameters[0] = new SqlParameter("@BankOutDetailsID", BankOutDetailsID);
parameters[1] = new SqlParameter("@BranchIDs",BranchIDs);
parameters[2] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable =main.ExecuteQuery_DataTable("SB_BankOutDetails_Select",true,parameters);
List<BankOutDetails> lst = new List<BankOutDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<BankOutDetails>(dataTable);
}
return lst;
}

public void Delete(int BankOutDetailsID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BankOutDetailsID", BankOutDetailsID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankOutDetails_Delete",true,parameters);
}
public void DeleteVirtual(int BankOutDetailsID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BankOutDetailsID", BankOutDetailsID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankOutDetails_DeleteVirtual",true,parameters);
}


public List<BankOutDetails> SelectByAccountID(int AccountID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@AccountID", AccountID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankOutDetails_SelectByAccountID",true,parameters);
List<BankOutDetails> lst = new List<BankOutDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<BankOutDetails>(dataTable);
}
return lst;
}

public void DeleteByAccountID(int AccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@AccountID", AccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankOutDetails_DeleteByAccountID",true,parameters);
}
public void DeleteVirtualByAccountID(int AccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@AccountID", AccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankOutDetails_DeleteVirtualByAccountID" ,true,parameters);
}

public List<BankOutDetails> SelectByCostCenterID(int CostCenterID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CostCenterID", CostCenterID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankOutDetails_SelectByCostCenterID",true,parameters);
List<BankOutDetails> lst = new List<BankOutDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<BankOutDetails>(dataTable);
}
return lst;
}

public void DeleteByCostCenterID(int CostCenterID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CostCenterID", CostCenterID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankOutDetails_DeleteByCostCenterID",true,parameters);
}
public void DeleteVirtualByCostCenterID(int CostCenterID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CostCenterID", CostCenterID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankOutDetails_DeleteVirtualByCostCenterID" ,true,parameters);
}

public List<BankOutDetails> SelectBySubAccountID(int SubAccountID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankOutDetails_SelectBySubAccountID",true,parameters);
List<BankOutDetails> lst = new List<BankOutDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<BankOutDetails>(dataTable);
}
return lst;
}

public void DeleteBySubAccountID(int SubAccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankOutDetails_DeleteBySubAccountID",true,parameters);
}
public void DeleteVirtualBySubAccountID(int SubAccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankOutDetails_DeleteVirtualBySubAccountID" ,true,parameters);
}

public List<BankOutDetails> SelectBySubCostCenterID(int SubCostCenterID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubCostCenterID", SubCostCenterID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankOutDetails_SelectBySubCostCenterID",true,parameters);
List<BankOutDetails> lst = new List<BankOutDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<BankOutDetails>(dataTable);
}
return lst;
}

public void DeleteBySubCostCenterID(int SubCostCenterID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubCostCenterID", SubCostCenterID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankOutDetails_DeleteBySubCostCenterID",true,parameters);
}
public void DeleteVirtualBySubCostCenterID(int SubCostCenterID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubCostCenterID", SubCostCenterID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankOutDetails_DeleteVirtualBySubCostCenterID" ,true,parameters);
}

public List<BankOutDetails> SelectByBranchID(int BranchID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankOutDetails_SelectByBranchID",true,parameters);
List<BankOutDetails> lst = new List<BankOutDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<BankOutDetails>(dataTable);
}
return lst;
}

public void DeleteByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankOutDetails_DeleteByBranchID",true,parameters);
}
public void DeleteVirtualByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankOutDetails_DeleteVirtualByBranchID" ,true,parameters);
}

public List<BankOutDetails> SelectByBankOutID(int BankOutID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BankOutID", BankOutID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankOutDetails_SelectByBankOutID",true,parameters);
List<BankOutDetails> lst = new List<BankOutDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<BankOutDetails>(dataTable);
}
return lst;
}

public void DeleteByBankOutID(int BankOutID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BankOutID", BankOutID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankOutDetails_DeleteByBankOutID",true,parameters);
}
public void DeleteVirtualByBankOutID(int BankOutID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BankOutID", BankOutID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankOutDetails_DeleteVirtualByBankOutID" ,true,parameters);
}

}
}
