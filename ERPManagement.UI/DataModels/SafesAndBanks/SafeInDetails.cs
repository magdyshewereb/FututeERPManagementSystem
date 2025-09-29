using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataModels.Interfaces;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.SafesAndBanks
{
public class SafeInDetails
{

public int SafeInDetailsID { get; set; }

public int SafeInID { get; set; }

public decimal?  Value { get; set; }

public int AccountID { get; set; }

public int? SubAccountID { get; set; }

public int? CostCenterID { get; set; }

public int? SubCostCenterID { get; set; }

public bool? IsDocumented { get; set; }

public string? Notes { get; set; }

public int? BranchID { get; set; }

public bool  Deleted { get; set; }

public SafeInDetails Clone()
{
return (SafeInDetails)MemberwiseClone();
}
}
public class SafeInDetailsService : IScopedService
    {


public int Insert_Update(SafeInDetails obj,int UserID,Main main)
{
SqlParameter[] parameters = new SqlParameter[12];
parameters[0] = new SqlParameter("@SafeInDetailsID", obj.SafeInDetailsID== null ? DBNull.Value : obj.SafeInDetailsID);
parameters[1] = new SqlParameter("@SafeInID", obj.SafeInID== null ? DBNull.Value : obj.SafeInID);
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
DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeInDetails_Insert_Update",true,parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}

public List<SafeInDetails> Select(int SafeInDetailsID, string BranchIDs, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[3];
parameters[0] = new SqlParameter("@SafeInDetailsID", SafeInDetailsID);
parameters[1] = new SqlParameter("@BranchIDs",BranchIDs);
parameters[2] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable =main.ExecuteQuery_DataTable("SB_SafeInDetails_Select",true,parameters);
List<SafeInDetails> lst = new List<SafeInDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeInDetails>(dataTable);
}
return lst;
}

public void Delete(int SafeInDetailsID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SafeInDetailsID", SafeInDetailsID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeInDetails_Delete",true,parameters);
}
public void DeleteVirtual(int SafeInDetailsID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SafeInDetailsID", SafeInDetailsID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeInDetails_DeleteVirtual",true,parameters);
}


public List<SafeInDetails> SelectByAccountID(int AccountID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@AccountID", AccountID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeInDetails_SelectByAccountID",true,parameters);
List<SafeInDetails> lst = new List<SafeInDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeInDetails>(dataTable);
}
return lst;
}

public void DeleteByAccountID(int AccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@AccountID", AccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeInDetails_DeleteByAccountID",true,parameters);
}
public void DeleteVirtualByAccountID(int AccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@AccountID", AccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeInDetails_DeleteVirtualByAccountID" ,true,parameters);
}

public List<SafeInDetails> SelectByCostCenterID(int CostCenterID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CostCenterID", CostCenterID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeInDetails_SelectByCostCenterID",true,parameters);
List<SafeInDetails> lst = new List<SafeInDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeInDetails>(dataTable);
}
return lst;
}

public void DeleteByCostCenterID(int CostCenterID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CostCenterID", CostCenterID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeInDetails_DeleteByCostCenterID",true,parameters);
}
public void DeleteVirtualByCostCenterID(int CostCenterID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CostCenterID", CostCenterID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeInDetails_DeleteVirtualByCostCenterID" ,true,parameters);
}

public List<SafeInDetails> SelectBySubAccountID(int SubAccountID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeInDetails_SelectBySubAccountID",true,parameters);
List<SafeInDetails> lst = new List<SafeInDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeInDetails>(dataTable);
}
return lst;
}

public void DeleteBySubAccountID(int SubAccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeInDetails_DeleteBySubAccountID",true,parameters);
}
public void DeleteVirtualBySubAccountID(int SubAccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeInDetails_DeleteVirtualBySubAccountID" ,true,parameters);
}

public List<SafeInDetails> SelectBySubCostCenterID(int SubCostCenterID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubCostCenterID", SubCostCenterID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeInDetails_SelectBySubCostCenterID",true,parameters);
List<SafeInDetails> lst = new List<SafeInDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeInDetails>(dataTable);
}
return lst;
}

public void DeleteBySubCostCenterID(int SubCostCenterID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubCostCenterID", SubCostCenterID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeInDetails_DeleteBySubCostCenterID",true,parameters);
}
public void DeleteVirtualBySubCostCenterID(int SubCostCenterID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubCostCenterID", SubCostCenterID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeInDetails_DeleteVirtualBySubCostCenterID" ,true,parameters);
}

public List<SafeInDetails> SelectByBranchID(int BranchID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeInDetails_SelectByBranchID",true,parameters);
List<SafeInDetails> lst = new List<SafeInDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeInDetails>(dataTable);
}
return lst;
}

public void DeleteByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeInDetails_DeleteByBranchID",true,parameters);
}
public void DeleteVirtualByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeInDetails_DeleteVirtualByBranchID" ,true,parameters);
}

public List<SafeInDetails> SelectBySafeInID(int SafeInID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SafeInID", SafeInID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeInDetails_SelectBySafeInID",true,parameters);
List<SafeInDetails> lst = new List<SafeInDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeInDetails>(dataTable);
}
return lst;
}

public void DeleteBySafeInID(int SafeInID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SafeInID", SafeInID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeInDetails_DeleteBySafeInID",true,parameters);
}
public void DeleteVirtualBySafeInID(int SafeInID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SafeInID", SafeInID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeInDetails_DeleteVirtualBySafeInID" ,true,parameters);
}

}
}
