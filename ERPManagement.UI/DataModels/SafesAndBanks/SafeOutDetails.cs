using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataModels.Interfaces;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.SafesAndBanks
{
public class SafeOutDetails
{

public int SafeOutDetailsID { get; set; }

public int SafeOutID { get; set; }

public decimal?  Value { get; set; }

public int AccountID { get; set; }

public int? SubAccountID { get; set; }

public int? CostCenterID { get; set; }

public int? SubCostCenterID { get; set; }

public bool? IsDocumented { get; set; }

public string? Notes { get; set; }

public int? BranchID { get; set; }

public bool  Deleted { get; set; }

public SafeOutDetails Clone()
{
return (SafeOutDetails)MemberwiseClone();
}
}
public class SafeOutDetailsService : IScopedService
    {


public int Insert_Update(SafeOutDetails obj,int UserID,Main main)
{
SqlParameter[] parameters = new SqlParameter[12];
parameters[0] = new SqlParameter("@SafeOutDetailsID", obj.SafeOutDetailsID== null ? DBNull.Value : obj.SafeOutDetailsID);
parameters[1] = new SqlParameter("@SafeOutID", obj.SafeOutID== null ? DBNull.Value : obj.SafeOutID);
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
DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeOutDetails_Insert_Update",true,parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}

public List<SafeOutDetails> Select(int SafeOutDetailsID, string BranchIDs, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[3];
parameters[0] = new SqlParameter("@SafeOutDetailsID", SafeOutDetailsID);
parameters[1] = new SqlParameter("@BranchIDs",BranchIDs);
parameters[2] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable =main.ExecuteQuery_DataTable("SB_SafeOutDetails_Select",true,parameters);
List<SafeOutDetails> lst = new List<SafeOutDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeOutDetails>(dataTable);
}
return lst;
}

public void Delete(int SafeOutDetailsID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SafeOutDetailsID", SafeOutDetailsID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutDetails_Delete",true,parameters);
}
public void DeleteVirtual(int SafeOutDetailsID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SafeOutDetailsID", SafeOutDetailsID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutDetails_DeleteVirtual",true,parameters);
}


public List<SafeOutDetails> SelectByAccountID(int AccountID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@AccountID", AccountID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeOutDetails_SelectByAccountID",true,parameters);
List<SafeOutDetails> lst = new List<SafeOutDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeOutDetails>(dataTable);
}
return lst;
}

public void DeleteByAccountID(int AccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@AccountID", AccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutDetails_DeleteByAccountID",true,parameters);
}
public void DeleteVirtualByAccountID(int AccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@AccountID", AccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutDetails_DeleteVirtualByAccountID" ,true,parameters);
}

public List<SafeOutDetails> SelectByCostCenterID(int CostCenterID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CostCenterID", CostCenterID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeOutDetails_SelectByCostCenterID",true,parameters);
List<SafeOutDetails> lst = new List<SafeOutDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeOutDetails>(dataTable);
}
return lst;
}

public void DeleteByCostCenterID(int CostCenterID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CostCenterID", CostCenterID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutDetails_DeleteByCostCenterID",true,parameters);
}
public void DeleteVirtualByCostCenterID(int CostCenterID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CostCenterID", CostCenterID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutDetails_DeleteVirtualByCostCenterID" ,true,parameters);
}

public List<SafeOutDetails> SelectBySubAccountID(int SubAccountID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeOutDetails_SelectBySubAccountID",true,parameters);
List<SafeOutDetails> lst = new List<SafeOutDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeOutDetails>(dataTable);
}
return lst;
}

public void DeleteBySubAccountID(int SubAccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutDetails_DeleteBySubAccountID",true,parameters);
}
public void DeleteVirtualBySubAccountID(int SubAccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutDetails_DeleteVirtualBySubAccountID" ,true,parameters);
}

public List<SafeOutDetails> SelectBySubCostCenterID(int SubCostCenterID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubCostCenterID", SubCostCenterID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeOutDetails_SelectBySubCostCenterID",true,parameters);
List<SafeOutDetails> lst = new List<SafeOutDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeOutDetails>(dataTable);
}
return lst;
}

public void DeleteBySubCostCenterID(int SubCostCenterID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubCostCenterID", SubCostCenterID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutDetails_DeleteBySubCostCenterID",true,parameters);
}
public void DeleteVirtualBySubCostCenterID(int SubCostCenterID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubCostCenterID", SubCostCenterID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutDetails_DeleteVirtualBySubCostCenterID" ,true,parameters);
}

public List<SafeOutDetails> SelectByBranchID(int BranchID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeOutDetails_SelectByBranchID",true,parameters);
List<SafeOutDetails> lst = new List<SafeOutDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeOutDetails>(dataTable);
}
return lst;
}

public void DeleteByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutDetails_DeleteByBranchID",true,parameters);
}
public void DeleteVirtualByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutDetails_DeleteVirtualByBranchID" ,true,parameters);
}

public List<SafeOutDetails> SelectBySafeOutID(int SafeOutID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SafeOutID", SafeOutID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeOutDetails_SelectBySafeOutID",true,parameters);
List<SafeOutDetails> lst = new List<SafeOutDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeOutDetails>(dataTable);
}
return lst;
}

public void DeleteBySafeOutID(int SafeOutID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SafeOutID", SafeOutID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutDetails_DeleteBySafeOutID",true,parameters);
}
public void DeleteVirtualBySafeOutID(int SafeOutID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SafeOutID", SafeOutID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutDetails_DeleteVirtualBySafeOutID" ,true,parameters);
}

}
}
