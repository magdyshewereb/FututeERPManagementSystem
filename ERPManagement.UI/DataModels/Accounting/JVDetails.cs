using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.Accounting
{
public class JVDetails
{

public decimal JVDetailID { get; set; }

public int JVID { get; set; }

public int AccountID { get; set; }

public int? SubAccountID { get; set; }

public int? CostCenterID { get; set; }

public int? SubCostCenterID { get; set; }

[System.ComponentModel.DefaultValue(1)]
public decimal? Debit { get; set; }

public decimal?  Credit { get; set; }

public int CurrencyID { get; set; }
        //        private decimal _ExchangeRate;
        //public decimal ExchangeRate
        //        {
        //            get { return _ExchangeRate; }
        //            set
        //            {
        //                if (value == null|| value == 0)
        //                {
        //                    _ExchangeRate =1;
        //                }
        //            }
        //        }
public decimal ExchangeRate { get; set; }
public decimal LocalDebit { get; set; }

public decimal LocalCredit { get; set; }

public bool  IsDocumented { get; set; }

public string? Notes { get; set; }

public bool  Deleted { get; set; }

public int? BranchID { get; set; }

public JVDetails Clone()
{
return (JVDetails)MemberwiseClone();
}
}
public class JVDetailsService : Interfaces.IScopedService
{

public string GetCode(Main main)
{
            DataTable dataTable = main.ExecuteQuery_DataTable("A_JVDetails_GetCode",true,null);
return dataTable.Rows[0][0].ToString();
}
public string GetCodeByBranchID(DateTime Date,string BranchID,Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@Date", Date);
parameters[1] = new SqlParameter("@BranchID",BranchID);
DataTable dataTable = main.ExecuteQuery_DataTable("A_JVDetails_GetCodeByBranchID",true,parameters);
return dataTable.Rows[0][0].ToString();
}

public int Insert_Update(JVDetails obj,int UserID,Main main)
{
SqlParameter[] parameters = new SqlParameter[17];
parameters[0] = new SqlParameter("@JVDetailID", obj.JVDetailID== null ? DBNull.Value : obj.JVDetailID);
parameters[1] = new SqlParameter("@JVID", obj.JVID== null ? DBNull.Value : obj.JVID);
parameters[2] = new SqlParameter("@AccountID", obj.AccountID== null ? DBNull.Value : obj.AccountID);
parameters[3] = new SqlParameter("@SubAccountID", obj.SubAccountID== null ? DBNull.Value : obj.SubAccountID);
parameters[4] = new SqlParameter("@CostCenterID", obj.CostCenterID== null ? DBNull.Value : obj.CostCenterID);
parameters[5] = new SqlParameter("@SubCostCenterID", obj.SubCostCenterID== null ? DBNull.Value : obj.SubCostCenterID);
parameters[6] = new SqlParameter("@Debit", obj.Debit== null ? DBNull.Value : obj.Debit);
parameters[7] = new SqlParameter("@Credit", obj.Credit== null ? DBNull.Value : obj.Credit);
parameters[8] = new SqlParameter("@CurrencyID", obj.CurrencyID== null ? DBNull.Value : obj.CurrencyID);
parameters[9] = new SqlParameter("@ExchangeRate", obj.ExchangeRate== null ? DBNull.Value : obj.ExchangeRate);
parameters[10] = new SqlParameter("@LocalDebit", obj.LocalDebit== null ? DBNull.Value : obj.LocalDebit);
parameters[11] = new SqlParameter("@LocalCredit", obj.LocalCredit== null ? DBNull.Value : obj.LocalCredit);
parameters[12] = new SqlParameter("@IsDocumented", obj.IsDocumented== null ? false : obj.IsDocumented);
parameters[13] = new SqlParameter("@Notes", obj.Notes== null ? DBNull.Value : obj.Notes);
parameters[14] = new SqlParameter("@Deleted", obj.Deleted== null ? false : obj.Deleted);
parameters[15] = new SqlParameter("@BranchID", obj.BranchID== null ? DBNull.Value : obj.BranchID);
parameters[16] = new SqlParameter("@UserID",UserID);
DataTable dataTable = main.ExecuteQuery_DataTable("A_JVDetails_Insert_Update",true,parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}

public List<JVDetails> Select(int JVDetailID, string BranchIDs, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[3];
parameters[0] = new SqlParameter("@JVDetailID", JVDetailID);
parameters[1] = new SqlParameter("@BranchIDs",BranchIDs);
parameters[2] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable =main.ExecuteQuery_DataTable("A_JVDetails_Select",true,parameters);
List<JVDetails> lst = new List<JVDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<JVDetails>(dataTable);
}
return lst;
}

public void Delete(int JVDetailID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JVDetailID", JVDetailID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_JVDetails_Delete",true,parameters);
}
public void DeleteVirtual(int JVDetailID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JVDetailID", JVDetailID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_JVDetails_DeleteVirtual",true,parameters);
}


public List<JVDetails> SelectByAccountID(int AccountID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@AccountID", AccountID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("A_JVDetails_SelectByAccountID",true,parameters);
List<JVDetails> lst = new List<JVDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<JVDetails>(dataTable);
}
return lst;
}

public void DeleteByAccountID(int AccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@AccountID", AccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_JVDetails_DeleteByAccountID",true,parameters);
}
public void DeleteVirtualByAccountID(int AccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@AccountID", AccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_JVDetails_DeleteVirtualByAccountID" ,true,parameters);
}

public List<JVDetails> SelectByCostCenterID(int CostCenterID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CostCenterID", CostCenterID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("A_JVDetails_SelectByCostCenterID",true,parameters);
List<JVDetails> lst = new List<JVDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<JVDetails>(dataTable);
}
return lst;
}

public void DeleteByCostCenterID(int CostCenterID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CostCenterID", CostCenterID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_JVDetails_DeleteByCostCenterID",true,parameters);
}
public void DeleteVirtualByCostCenterID(int CostCenterID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CostCenterID", CostCenterID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_JVDetails_DeleteVirtualByCostCenterID" ,true,parameters);
}

public List<JVDetails> SelectByJVID(int JVID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JVID", JVID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("A_JVDetails_SelectByJVID",true,parameters);
List<JVDetails> lst = new List<JVDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<JVDetails>(dataTable);
}
return lst;
}

public void DeleteByJVID(int JVID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JVID", JVID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_JVDetails_DeleteByJVID",true,parameters);
}
public void DeleteVirtualByJVID(int JVID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JVID", JVID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_JVDetails_DeleteVirtualByJVID" ,true,parameters);
}

public List<JVDetails> SelectBySubAccountID(int SubAccountID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("A_JVDetails_SelectBySubAccountID",true,parameters);
List<JVDetails> lst = new List<JVDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<JVDetails>(dataTable);
}
return lst;
}

public void DeleteBySubAccountID(int SubAccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_JVDetails_DeleteBySubAccountID",true,parameters);
}
public void DeleteVirtualBySubAccountID(int SubAccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_JVDetails_DeleteVirtualBySubAccountID" ,true,parameters);
}

public List<JVDetails> SelectBySubCostCenterID(int SubCostCenterID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubCostCenterID", SubCostCenterID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("A_JVDetails_SelectBySubCostCenterID",true,parameters);
List<JVDetails> lst = new List<JVDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<JVDetails>(dataTable);
}
return lst;
}

public void DeleteBySubCostCenterID(int SubCostCenterID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubCostCenterID", SubCostCenterID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_JVDetails_DeleteBySubCostCenterID",true,parameters);
}
public void DeleteVirtualBySubCostCenterID(int SubCostCenterID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubCostCenterID", SubCostCenterID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_JVDetails_DeleteVirtualBySubCostCenterID" ,true,parameters);
}

public List<JVDetails> SelectByCurrencyID(int CurrencyID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CurrencyID", CurrencyID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("A_JVDetails_SelectByCurrencyID",true,parameters);
List<JVDetails> lst = new List<JVDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<JVDetails>(dataTable);
}
return lst;
}

public void DeleteByCurrencyID(int CurrencyID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CurrencyID", CurrencyID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_JVDetails_DeleteByCurrencyID",true,parameters);
}
public void DeleteVirtualByCurrencyID(int CurrencyID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CurrencyID", CurrencyID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_JVDetails_DeleteVirtualByCurrencyID" ,true,parameters);
}

public List<JVDetails> SelectByBranchID(int BranchID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("A_JVDetails_SelectByBranchID",true,parameters);
List<JVDetails> lst = new List<JVDetails>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<JVDetails>(dataTable);
}
return lst;
}

public void DeleteByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_JVDetails_DeleteByBranchID",true,parameters);
}
public void DeleteVirtualByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_JVDetails_DeleteVirtualByBranchID" ,true,parameters);
}

}
}
