using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataModels.Interfaces;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.SafesAndBanks
{
public class BankInDetails
{

public int BankInDetailsID { get; set; }

public int BankInID { get; set; }

public decimal?  Value { get; set; }

public int AccountID { get; set; }

public int? SubAccountID { get; set; }

public int? CostCenterID { get; set; }

public int? SubCostCenterID { get; set; }

public bool? IsDocumented { get; set; }

public string? Notes { get; set; }

public int? BranchID { get; set; }

public bool  Deleted { get; set; }

public BankInDetails Clone()
{
return (BankInDetails)MemberwiseClone();
}
}
public class BankInDetailsService : IScopedService
    {


public int Insert_Update(BankInDetails obj,int UserID,Main main)
{
SqlParameter[] parameters = new SqlParameter[12];
parameters[0] = new SqlParameter("@BankInDetailsID", obj.BankInDetailsID== null ? DBNull.Value : obj.BankInDetailsID);
parameters[1] = new SqlParameter("@BankInID", obj.BankInID== null ? DBNull.Value : obj.BankInID);
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
DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankInDetails_Insert_Update",true,parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}

public List<BankInDetails> Select(int BankInDetailsID, string BranchIDs, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[3];
parameters[0] = new SqlParameter("@BankInDetailsID", BankInDetailsID);
parameters[1] = new SqlParameter("@BranchIDs",BranchIDs);
parameters[2] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable =main.ExecuteQuery_DataTable("SB_BankInDetails_Select",true,parameters);
List<BankInDetails> lst = new List<BankInDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<BankInDetails>(dataTable);
}
return lst;
}

public void Delete(int BankInDetailsID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BankInDetailsID", BankInDetailsID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankInDetails_Delete",true,parameters);
}
public void DeleteVirtual(int BankInDetailsID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BankInDetailsID", BankInDetailsID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankInDetails_DeleteVirtual",true,parameters);
}


public List<BankInDetails> SelectByAccountID(int AccountID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@AccountID", AccountID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankInDetails_SelectByAccountID",true,parameters);
List<BankInDetails> lst = new List<BankInDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<BankInDetails>(dataTable);
}
return lst;
}

public void DeleteByAccountID(int AccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@AccountID", AccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankInDetails_DeleteByAccountID",true,parameters);
}
public void DeleteVirtualByAccountID(int AccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@AccountID", AccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankInDetails_DeleteVirtualByAccountID" ,true,parameters);
}

public List<BankInDetails> SelectByCostCenterID(int CostCenterID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CostCenterID", CostCenterID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankInDetails_SelectByCostCenterID",true,parameters);
List<BankInDetails> lst = new List<BankInDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<BankInDetails>(dataTable);
}
return lst;
}

public void DeleteByCostCenterID(int CostCenterID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CostCenterID", CostCenterID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankInDetails_DeleteByCostCenterID",true,parameters);
}
public void DeleteVirtualByCostCenterID(int CostCenterID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CostCenterID", CostCenterID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankInDetails_DeleteVirtualByCostCenterID" ,true,parameters);
}

public List<BankInDetails> SelectBySubAccountID(int SubAccountID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankInDetails_SelectBySubAccountID",true,parameters);
List<BankInDetails> lst = new List<BankInDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<BankInDetails>(dataTable);
}
return lst;
}

public void DeleteBySubAccountID(int SubAccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankInDetails_DeleteBySubAccountID",true,parameters);
}
public void DeleteVirtualBySubAccountID(int SubAccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankInDetails_DeleteVirtualBySubAccountID" ,true,parameters);
}

public List<BankInDetails> SelectBySubCostCenterID(int SubCostCenterID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubCostCenterID", SubCostCenterID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankInDetails_SelectBySubCostCenterID",true,parameters);
List<BankInDetails> lst = new List<BankInDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<BankInDetails>(dataTable);
}
return lst;
}

public void DeleteBySubCostCenterID(int SubCostCenterID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubCostCenterID", SubCostCenterID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankInDetails_DeleteBySubCostCenterID",true,parameters);
}
public void DeleteVirtualBySubCostCenterID(int SubCostCenterID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubCostCenterID", SubCostCenterID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankInDetails_DeleteVirtualBySubCostCenterID" ,true,parameters);
}

public List<BankInDetails> SelectByBranchID(int BranchID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankInDetails_SelectByBranchID",true,parameters);
List<BankInDetails> lst = new List<BankInDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<BankInDetails>(dataTable);
}
return lst;
}

public void DeleteByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankInDetails_DeleteByBranchID",true,parameters);
}
public void DeleteVirtualByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankInDetails_DeleteVirtualByBranchID" ,true,parameters);
}

public List<BankInDetails> SelectByBankInID(int BankInID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BankInID", BankInID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_BankInDetails_SelectByBankInID",true,parameters);
List<BankInDetails> lst = new List<BankInDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<BankInDetails>(dataTable);
}
return lst;
}

public void DeleteByBankInID(int BankInID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BankInID", BankInID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankInDetails_DeleteByBankInID",true,parameters);
}
public void DeleteVirtualByBankInID(int BankInID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BankInID", BankInID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_BankInDetails_DeleteVirtualByBankInID" ,true,parameters);
}

}
}
