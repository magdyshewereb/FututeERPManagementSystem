using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataModels.Interfaces;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.SafesAndBanks
{
public class SafeOutRequestDetails
{

public int SafeOutRequestDetailID { get; set; }

public int SafeOutRequestID { get; set; }

public decimal?  Value { get; set; }

public int AccountID { get; set; }

public int? SubAccountID { get; set; }

public int? CostCenterID { get; set; }

public int? SubCostCenterID { get; set; }

public bool? IsDocumented { get; set; }

public string? Notes { get; set; }

public int? BranchID { get; set; }

public bool  Deleted { get; set; }

public SafeOutRequestDetails Clone()
{
return (SafeOutRequestDetails)MemberwiseClone();
}
}
public class SafeOutRequestDetailsService : IScopedService
    {

public string GetCode(Main main)
{
DataTable dataTable =main.ExecuteQuery_DataTable("SB_SafeOutRequestDetails_GetCode",true,null);
return dataTable.Rows[0][0].ToString();
}
public string GetCodeByBranchID(DateTime Date,string BranchID,Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@Date", Date);
parameters[1] = new SqlParameter("@BranchID",BranchID);
DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeOutRequestDetails_GetCodeByBranchID",true,parameters);
return dataTable.Rows[0][0].ToString();
}

public int Insert_Update(SafeOutRequestDetails obj,int UserID,Main main)
{
SqlParameter[] parameters = new SqlParameter[12];
parameters[0] = new SqlParameter("@SafeOutRequestDetailID", obj.SafeOutRequestDetailID== null ? DBNull.Value : obj.SafeOutRequestDetailID);
parameters[1] = new SqlParameter("@SafeOutRequestID", obj.SafeOutRequestID== null ? DBNull.Value : obj.SafeOutRequestID);
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
DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeOutRequestDetails_Insert_Update",true,parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}
        public int Insert_UpdateByTable(List<SafeOutRequestDetails> lst,int SafeOutRequestID,int BranchID, string UserID, Main main)
        {
            
            string SafeOutRequestDetailID;
            string Value;
            string AccountID;
            string SubAccountID;
            string CostCenterID;
            string SubCostCenterID;
            string IsDocumented;
            string Notes;
            string Deleted;
            string strSql = string.Empty;
            for (int i = 0; i < lst.Count; i++)
            {
                //Setting Parameters
                SafeOutRequestDetailID = lst[i].SafeOutRequestDetailID.ToString();
                Value = lst[i].Value.ToString();
                AccountID = lst[i].AccountID.ToString();
                SubAccountID = lst[i].SubAccountID.ToString();
                CostCenterID = lst[i].CostCenterID.ToString();
                SubCostCenterID = lst[i].SubCostCenterID.ToString();
                IsDocumented = lst[i].IsDocumented.ToString();
                Notes =lst[i].Notes == null ? "" : lst[i].Notes;
                Deleted = lst[i].Deleted.ToString();
                strSql += "exec SB_SafeOutRequestDetails_Insert_Update ";

                strSql += SafeOutRequestDetailID.Equals("") ? "NULL," : SafeOutRequestDetailID.Equals("True") ? "1," : SafeOutRequestDetailID.Equals("False") ? "0," : "'" + SafeOutRequestDetailID + "',";
                strSql += SafeOutRequestID.Equals("") ? "NULL," : SafeOutRequestID.Equals("True") ? "1," : SafeOutRequestID.Equals("False") ? "0," : "'" + SafeOutRequestID + "',";
                strSql += Value.Equals("") ? "NULL," : Value.Equals("True") ? "1," : Value.Equals("False") ? "0," : "'" + Value + "',";
                strSql += AccountID.Equals("") ? "NULL," : AccountID.Equals("True") ? "1," : AccountID.Equals("False") ? "0," : "'" + AccountID + "',";
                strSql += SubAccountID.Equals("") ? "NULL," : SubAccountID.Equals("True") ? "1," : SubAccountID.Equals("False") ? "0," : "'" + SubAccountID + "',";
                strSql += CostCenterID.Equals("") ? "NULL," : CostCenterID.Equals("True") ? "1," : CostCenterID.Equals("False") ? "0," : "'" + CostCenterID + "',";
                strSql += SubCostCenterID.Equals("") ? "NULL," : SubCostCenterID.Equals("True") ? "1," : SubCostCenterID.Equals("False") ? "0," : "'" + SubCostCenterID + "',";
                strSql += IsDocumented.Equals("") ? "0," : IsDocumented.Equals("True") ? "1," : IsDocumented.Equals("False") ? "0," : "'" + IsDocumented + "',";
                strSql += Notes.Equals("") ? "NULL," : Notes.Equals("True") ? "1," : Notes.Equals("False") ? "0," : "'" + Notes + "',";
                strSql += BranchID.Equals("") ? "NULL," : BranchID.Equals("True") ? "1," : BranchID.Equals("False") ? "0," : "'" + BranchID + "',";
                strSql += Deleted.Equals("") ? "0," : Deleted.Equals("True") ? "1," : Deleted.Equals("False") ? "0," : "'" + Deleted + "',";
                strSql += UserID;
                strSql += ";";
            }
            DataTable dataTable = main.ExecuteQuery_DataTable(strSql, false, null);
           
            return int.Parse(dataTable.Rows[0][0].ToString());
        }

        public List<SafeOutRequestDetails> Select(int SafeOutRequestDetailID, string BranchIDs, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[3];
parameters[0] = new SqlParameter("@SafeOutRequestDetailID", SafeOutRequestDetailID);
parameters[1] = new SqlParameter("@BranchIDs",BranchIDs);
parameters[2] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable =main.ExecuteQuery_DataTable("SB_SafeOutRequestDetails_Select",true,parameters);
List<SafeOutRequestDetails> lst = new List<SafeOutRequestDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeOutRequestDetails>(dataTable);
}
return lst;
}

public void Delete(int SafeOutRequestDetailID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SafeOutRequestDetailID", SafeOutRequestDetailID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutRequestDetails_Delete",true,parameters);
}
public void DeleteVirtual(int SafeOutRequestDetailID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SafeOutRequestDetailID", SafeOutRequestDetailID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutRequestDetails_DeleteVirtual",true,parameters);
}


public List<SafeOutRequestDetails> SelectByAccountID(int AccountID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@AccountID", AccountID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeOutRequestDetails_SelectByAccountID",true,parameters);
List<SafeOutRequestDetails> lst = new List<SafeOutRequestDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeOutRequestDetails>(dataTable);
}
return lst;
}

public void DeleteByAccountID(int AccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@AccountID", AccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutRequestDetails_DeleteByAccountID",true,parameters);
}
public void DeleteVirtualByAccountID(int AccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@AccountID", AccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutRequestDetails_DeleteVirtualByAccountID" ,true,parameters);
}

public List<SafeOutRequestDetails> SelectByCostCenterID(int CostCenterID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CostCenterID", CostCenterID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeOutRequestDetails_SelectByCostCenterID",true,parameters);
List<SafeOutRequestDetails> lst = new List<SafeOutRequestDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeOutRequestDetails>(dataTable);
}
return lst;
}

public void DeleteByCostCenterID(int CostCenterID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CostCenterID", CostCenterID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutRequestDetails_DeleteByCostCenterID",true,parameters);
}
public void DeleteVirtualByCostCenterID(int CostCenterID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CostCenterID", CostCenterID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutRequestDetails_DeleteVirtualByCostCenterID" ,true,parameters);
}

public List<SafeOutRequestDetails> SelectBySubAccountID(int SubAccountID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeOutRequestDetails_SelectBySubAccountID",true,parameters);
List<SafeOutRequestDetails> lst = new List<SafeOutRequestDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeOutRequestDetails>(dataTable);
}
return lst;
}

public void DeleteBySubAccountID(int SubAccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutRequestDetails_DeleteBySubAccountID",true,parameters);
}
public void DeleteVirtualBySubAccountID(int SubAccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutRequestDetails_DeleteVirtualBySubAccountID" ,true,parameters);
}

public List<SafeOutRequestDetails> SelectBySubCostCenterID(int SubCostCenterID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubCostCenterID", SubCostCenterID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeOutRequestDetails_SelectBySubCostCenterID",true,parameters);
List<SafeOutRequestDetails> lst = new List<SafeOutRequestDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeOutRequestDetails>(dataTable);
}
return lst;
}

public void DeleteBySubCostCenterID(int SubCostCenterID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubCostCenterID", SubCostCenterID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutRequestDetails_DeleteBySubCostCenterID",true,parameters);
}
public void DeleteVirtualBySubCostCenterID(int SubCostCenterID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubCostCenterID", SubCostCenterID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutRequestDetails_DeleteVirtualBySubCostCenterID" ,true,parameters);
}

public List<SafeOutRequestDetails> SelectByBranchID(int BranchID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeOutRequestDetails_SelectByBranchID",true,parameters);
List<SafeOutRequestDetails> lst = new List<SafeOutRequestDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeOutRequestDetails>(dataTable);
}
return lst;
}

public void DeleteByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutRequestDetails_DeleteByBranchID",true,parameters);
}
public void DeleteVirtualByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutRequestDetails_DeleteVirtualByBranchID" ,true,parameters);
}

public List<SafeOutRequestDetails> SelectBySafeOutRequestID(int SafeOutRequestID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SafeOutRequestID", SafeOutRequestID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("SB_SafeOutRequestDetails_SelectBySafeOutRequestID",true,parameters);
List<SafeOutRequestDetails> lst = new List<SafeOutRequestDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SafeOutRequestDetails>(dataTable);
}
return lst;
}

public void DeleteBySafeOutRequestID(int SafeOutRequestID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SafeOutRequestID", SafeOutRequestID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutRequestDetails_DeleteBySafeOutRequestID",true,parameters);
}
public void DeleteVirtualBySafeOutRequestID(int SafeOutRequestID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SafeOutRequestID", SafeOutRequestID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SB_SafeOutRequestDetails_DeleteVirtualBySafeOutRequestID" ,true,parameters);
}

}
}
