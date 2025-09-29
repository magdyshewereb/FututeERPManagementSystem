using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataModels.Interfaces;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.SafesAndBanks
{
public class SafeOutRequests
{

public int SafeOutRequestID { get; set; }

public string SafeOutRequestNo { get; set; }

public DateTime SafeOutRequestDate { get; set; }

public int? CurrencyID { get; set; }

public string? Notes { get; set; }

public int? SafeOutID { get; set; }

public bool  IsRefused { get; set; }

public bool  Approved { get; set; }

public int? BranchID { get; set; }

public bool  Deleted { get; set; }

public SafeOutRequests Clone()
{
return (SafeOutRequests)MemberwiseClone();
}
}
public class SafeOutRequestsService : IScopedService
    {
        public DataTable Search(string BranchIDs, string FromDate, string ToDate, int Approved, bool Deleted, bool IsArabic, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[6];
            parameters[0] = new SqlParameter("@BranchIDs", BranchIDs);
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@Approved", Approved);
            parameters[4] = new SqlParameter("@Deleted", Deleted);
            parameters[5] = new SqlParameter("@IsArabic", IsArabic);
            DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeOutRequests_Search", true, parameters);
            return dataTable;
        }
        public string GetCode(Main main)
{
DataTable dataTable =main.ExecuteQuery_DataTable("SB_SafeOutRequests_GetCode",true,null);
return dataTable.Rows[0][0].ToString();
}
public string GetCodeByBranchID(string Date,string BranchID,Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@Date", Date);
parameters[1] = new SqlParameter("@BranchID",BranchID);
DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeOutRequests_GetCodeByBranchID",true,parameters);
return dataTable.Rows[0][0].ToString();
}

public int Insert_Update(SafeOutRequests obj,int UserID,Main main)
{
SqlParameter[] parameters = new SqlParameter[10];
parameters[0] = new SqlParameter("@SafeOutRequestID", obj.SafeOutRequestID== null ? DBNull.Value : obj.SafeOutRequestID);
parameters[1] = new SqlParameter("@SafeOutRequestNo", obj.SafeOutRequestNo== null ? DBNull.Value : obj.SafeOutRequestNo);
parameters[2] = new SqlParameter("@SafeOutRequestDate", obj.SafeOutRequestDate== null ? DBNull.Value : obj.SafeOutRequestDate);
parameters[3] = new SqlParameter("@CurrencyID", obj.CurrencyID== null ? DBNull.Value : obj.CurrencyID);
parameters[4] = new SqlParameter("@Notes", obj.Notes== null ? DBNull.Value : obj.Notes);
parameters[5] = new SqlParameter("@IsRefused", obj.IsRefused== null ? false : obj.IsRefused);
parameters[6] = new SqlParameter("@Approved", obj.Approved== null ? false : obj.Approved);
parameters[7] = new SqlParameter("@BranchID", obj.BranchID== null ? DBNull.Value : obj.BranchID);
parameters[8] = new SqlParameter("@Deleted", obj.Deleted== null ? false : obj.Deleted);
parameters[9] = new SqlParameter("@UserID",UserID);
DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeOutRequests_Insert_Update",true,parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}

public DataTable Select(int SafeOutRequestID, string BranchIDs, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[3];
parameters[0] = new SqlParameter("@SafeOutRequestID", SafeOutRequestID);
parameters[1] = new SqlParameter("@BranchIDs",BranchIDs);
parameters[2] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable =main.ExecuteQuery_DataTable("SB_SafeOutRequests_Select",true,parameters);
//List<SafeOutRequests> lst = new List<SafeOutRequests>();
//if (dataTable != null)
//{
// lst =main.CreateListFromTable<SafeOutRequests>(dataTable);
//}
return dataTable;
}

public void Delete(int SafeOutRequestID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SafeOutRequestID", SafeOutRequestID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutRequests_Delete",true,parameters);
}
public void DeleteVirtual(int SafeOutRequestID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SafeOutRequestID", SafeOutRequestID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutRequests_DeleteVirtual",true,parameters);
}


public List<SafeOutRequests> SelectByFiscalYearID(int FiscalYearID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@FiscalYearID", FiscalYearID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeOutRequests_SelectByFiscalYearID",true,parameters);
List<SafeOutRequests> lst = new List<SafeOutRequests>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeOutRequests>(dataTable);
}
return lst;
}

public void DeleteByFiscalYearID(int FiscalYearID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@FiscalYearID", FiscalYearID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutRequests_DeleteByFiscalYearID",true,parameters);
}
public void DeleteVirtualByFiscalYearID(int FiscalYearID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@FiscalYearID", FiscalYearID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutRequests_DeleteVirtualByFiscalYearID" ,true,parameters);
}

public List<SafeOutRequests> SelectByCurrencyID(int CurrencyID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CurrencyID", CurrencyID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeOutRequests_SelectByCurrencyID",true,parameters);
List<SafeOutRequests> lst = new List<SafeOutRequests>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeOutRequests>(dataTable);
}
return lst;
}

public void DeleteByCurrencyID(int CurrencyID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CurrencyID", CurrencyID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutRequests_DeleteByCurrencyID",true,parameters);
}
public void DeleteVirtualByCurrencyID(int CurrencyID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CurrencyID", CurrencyID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutRequests_DeleteVirtualByCurrencyID" ,true,parameters);
}

public List<SafeOutRequests> SelectByBranchID(int BranchID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeOutRequests_SelectByBranchID",true,parameters);
List<SafeOutRequests> lst = new List<SafeOutRequests>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeOutRequests>(dataTable);
}
return lst;
}

public void DeleteByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutRequests_DeleteByBranchID",true,parameters);
}
public void DeleteVirtualByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutRequests_DeleteVirtualByBranchID" ,true,parameters);
}

public List<SafeOutRequests> SelectBySafeOutID(int SafeOutID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SafeOutID", SafeOutID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeOutRequests_SelectBySafeOutID",true,parameters);
List<SafeOutRequests> lst = new List<SafeOutRequests>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeOutRequests>(dataTable);
}
return lst;
}

public void DeleteBySafeOutID(int SafeOutID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SafeOutID", SafeOutID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutRequests_DeleteBySafeOutID",true,parameters);
}
public void DeleteVirtualBySafeOutID(int SafeOutID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SafeOutID", SafeOutID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutRequests_DeleteVirtualBySafeOutID" ,true,parameters);
}

}
}
