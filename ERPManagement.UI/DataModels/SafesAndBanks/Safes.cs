using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataModels.Interfaces;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.SafesAndBanks
{
public class Safes
{

public int SafeID { get; set; }

public string? SafeCode { get; set; }

public string SafeNameAr { get; set; }

public string? SafeNameEn { get; set; }

public int AccountID { get; set; }

public int? SubAccountID { get; set; }

public decimal?  SafeMinimumLimit { get; set; }

public int? BranchID { get; set; }

public bool  Deleted { get; set; }

public Safes Clone()
{
return (Safes)MemberwiseClone();
}
}
public class SafesService : IScopedService
    {

        public  DataTable FillComboBySafeIDs(string SafeIDs, bool IsArabic, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[10];
            parameters[0] = new SqlParameter("@SafeIDs", SafeIDs);
            parameters[1] = new SqlParameter("@IsArabic", IsArabic);
            DataTable dataTable = main.ExecuteQuery_DataTable("SB_Safes_FillComboBySafeIDs", true, parameters);
            return dataTable;
        }
        public int Insert_Update(Safes obj,int UserID,Main main)
{
SqlParameter[] parameters = new SqlParameter[10];
parameters[0] = new SqlParameter("@SafeID", obj.SafeID== null ? DBNull.Value : obj.SafeID);
parameters[1] = new SqlParameter("@SafeCode", obj.SafeCode== null ? DBNull.Value : obj.SafeCode);
parameters[2] = new SqlParameter("@SafeNameAr", obj.SafeNameAr== null ? DBNull.Value : obj.SafeNameAr);
parameters[3] = new SqlParameter("@SafeNameEn", obj.SafeNameEn== null ? DBNull.Value : obj.SafeNameEn);
parameters[4] = new SqlParameter("@AccountID", obj.AccountID== null ? DBNull.Value : obj.AccountID);
parameters[5] = new SqlParameter("@SubAccountID", obj.SubAccountID== null ? DBNull.Value : obj.SubAccountID);
parameters[6] = new SqlParameter("@SafeMinimumLimit", obj.SafeMinimumLimit== null ? DBNull.Value : obj.SafeMinimumLimit);
parameters[7] = new SqlParameter("@BranchID", obj.BranchID== null ? DBNull.Value : obj.BranchID);
parameters[8] = new SqlParameter("@Deleted", obj.Deleted== null ? false : obj.Deleted);
parameters[9] = new SqlParameter("@UserID",UserID);
DataTable dataTable = main.ExecuteQuery_DataTable("SB_Safes_Insert_Update",true,parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}

public List<Safes> Select(int SafeID, string BranchIDs, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[3];
parameters[0] = new SqlParameter("@SafeID", SafeID);
parameters[1] = new SqlParameter("@BranchIDs",BranchIDs);
parameters[2] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable =main.ExecuteQuery_DataTable("SB_Safes_Select",true,parameters);
List<Safes> lst = new List<Safes>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<Safes>(dataTable);
}
return lst;
}

public void Delete(int SafeID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SafeID", SafeID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_Safes_Delete",true,parameters);
}
public void DeleteVirtual(int SafeID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SafeID", SafeID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_Safes_DeleteVirtual",true,parameters);
}


public List<Safes> SelectByAccountID(int AccountID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@AccountID", AccountID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_Safes_SelectByAccountID",true,parameters);
List<Safes> lst = new List<Safes>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<Safes>(dataTable);
}
return lst;
}

public void DeleteByAccountID(int AccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@AccountID", AccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_Safes_DeleteByAccountID",true,parameters);
}
public void DeleteVirtualByAccountID(int AccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@AccountID", AccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_Safes_DeleteVirtualByAccountID" ,true,parameters);
}

public List<Safes> SelectBySubAccountID(int SubAccountID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_Safes_SelectBySubAccountID",true,parameters);
List<Safes> lst = new List<Safes>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<Safes>(dataTable);
}
return lst;
}

public void DeleteBySubAccountID(int SubAccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_Safes_DeleteBySubAccountID",true,parameters);
}
public void DeleteVirtualBySubAccountID(int SubAccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_Safes_DeleteVirtualBySubAccountID" ,true,parameters);
}

public List<Safes> SelectByBranchID(int BranchID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_Safes_SelectByBranchID",true,parameters);
List<Safes> lst = new List<Safes>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<Safes>(dataTable);
}
return lst;
}

public void DeleteByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_Safes_DeleteByBranchID",true,parameters);
}
public void DeleteVirtualByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_Safes_DeleteVirtualByBranchID" ,true,parameters);
}

}
}
