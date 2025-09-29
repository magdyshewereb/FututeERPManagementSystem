using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.StockControl
{
public class Stores
{

public int StoreID { get; set; }

public string StoreCode { get; set; }

public string StoreNameAr { get; set; }

public string? StoreNameEn { get; set; }

public int? CostCenterID { get; set; }

public int StoreAccID { get; set; }

public int? AddedSettlementAccountID { get; set; }

public int? SubtractSettlementAccountID { get; set; }

public int? AddedStoreTakingAccountID { get; set; }

public int? SubtractStoreTakingAccountID { get; set; }

public bool  Locked { get; set; }

public decimal?  Weight { get; set; }

public int BranchID { get; set; }

public bool  Deleted { get; set; }

public Stores Clone()
{
return (Stores)MemberwiseClone();
}
}
public class StoresService : Interfaces.IScopedService
{

public string GetCode(Main main)
{
            DataTable dataTable = main.ExecuteQuery_DataTable("SC_Stores_GetCode",true,null);
return dataTable.Rows[0][0].ToString();
}
public string GetCodeByBranchID(DateTime Date,string BranchID,Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@Date", Date);
parameters[1] = new SqlParameter("@BranchID",BranchID);
DataTable dataTable = main.ExecuteQuery_DataTable("SC_Stores_GetCodeByBranchID",true,parameters);
return dataTable.Rows[0][0].ToString();
}

public int Insert_Update(Stores obj,int UserID,Main main)
{
SqlParameter[] parameters = new SqlParameter[15];
parameters[0] = new SqlParameter("@StoreID", obj.StoreID);
parameters[1] = new SqlParameter("@StoreCode", obj.StoreCode);
parameters[2] = new SqlParameter("@StoreNameAr", obj.StoreNameAr);
parameters[3] = new SqlParameter("@StoreNameEn", obj.StoreNameEn);
parameters[4] = new SqlParameter("@CostCenterID", obj.CostCenterID);
parameters[5] = new SqlParameter("@StoreAccID", obj.StoreAccID);
parameters[6] = new SqlParameter("@AddedSettlementAccountID", obj.AddedSettlementAccountID);
parameters[7] = new SqlParameter("@SubtractSettlementAccountID", obj.SubtractSettlementAccountID);
parameters[8] = new SqlParameter("@AddedStoreTakingAccountID", obj.AddedStoreTakingAccountID);
parameters[9] = new SqlParameter("@SubtractStoreTakingAccountID", obj.SubtractStoreTakingAccountID);
parameters[10] = new SqlParameter("@Locked", obj.Locked);
parameters[11] = new SqlParameter("@Weight", obj.Weight);
parameters[12] = new SqlParameter("@BranchID", obj.BranchID);
parameters[13] = new SqlParameter("@Deleted", obj.Deleted);
parameters[14] = new SqlParameter("@UserID",UserID);
DataTable dataTable = main.ExecuteQuery_DataTable("SC_Stores_Insert_Update",true,parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}

public List<Stores> Select(int StoreID, string BranchIDs, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[3];
parameters[0] = new SqlParameter("@StoreID", StoreID);
parameters[1] = new SqlParameter("@BranchIDs",BranchIDs);
parameters[2] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable =main.ExecuteQuery_DataTable("SC_Stores_Select",true,parameters);
List<Stores> lst = new List<Stores>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<Stores>(dataTable);
}
return lst;
}

        public  DataTable FillCombo(int ID, int Locked, string BranchIDs, bool IsArabic, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[4];
            parameters[0] = new SqlParameter("@StoreID",ID);
            parameters[1] = new SqlParameter("@Locked", Locked);
            parameters[2] = new SqlParameter("@BranchIDs", BranchIDs);
            parameters[3] = new SqlParameter("@IsArabic", IsArabic);
            DataTable dataTable = main.ExecuteQuery_DataTable("SC_Stores_FillCombo", true, parameters);
            return dataTable;
        }















    }
}
