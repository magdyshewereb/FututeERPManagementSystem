using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.StockControl
{
public class PricesTypes
{

public int PriceTypeID { get; set; }

public string PriceNameAr { get; set; }

public string? PriceNameEn { get; set; }

public bool  IsLastPSPrice { get; set; }

public bool  IsCostPrice { get; set; }

public bool  Deleted { get; set; }

public int? BranchID { get; set; }

public PricesTypes Clone()
{
return (PricesTypes)MemberwiseClone();
}
}
public class PricesTypesService : Interfaces.IScopedService
{

public string GetCode(Main main)
{
            DataTable dataTable = main.ExecuteQuery_DataTable("SC_PricesTypes_GetCode",true,null);
return dataTable.Rows[0][0].ToString();
}
public string GetCodeByBranchID(DateTime Date,string BranchID,Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@Date", Date);
parameters[1] = new SqlParameter("@BranchID",BranchID);
DataTable dataTable = main.ExecuteQuery_DataTable("SC_PricesTypes_GetCodeByBranchID",true,parameters);
return dataTable.Rows[0][0].ToString();
}

public int Insert_Update(PricesTypes obj,int UserID,Main main)
{
SqlParameter[] parameters = new SqlParameter[8];
parameters[0] = new SqlParameter("@PriceTypeID", obj.PriceTypeID);
parameters[1] = new SqlParameter("@PriceNameAr", obj.PriceNameAr);
parameters[2] = new SqlParameter("@PriceNameEn", obj.PriceNameEn);
parameters[3] = new SqlParameter("@IsLastPSPrice", obj.IsLastPSPrice);
parameters[4] = new SqlParameter("@IsCostPrice", obj.IsCostPrice);
parameters[5] = new SqlParameter("@Deleted", obj.Deleted);
parameters[6] = new SqlParameter("@BranchID", obj.BranchID);
parameters[7] = new SqlParameter("@UserID",UserID);
DataTable dataTable = main.ExecuteQuery_DataTable("SC_PricesTypes_Insert_Update",true,parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}

public List<PricesTypes> Select(int PriceTypeID, string BranchIDs, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[3];
parameters[0] = new SqlParameter("@PriceTypeID", PriceTypeID);
parameters[1] = new SqlParameter("@BranchIDs",BranchIDs);
parameters[2] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable =main.ExecuteQuery_DataTable("SC_PricesTypes_Select",true,parameters);
List<PricesTypes> lst = new List<PricesTypes>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<PricesTypes>(dataTable);
}
return lst;
}

public void Delete(int PriceTypeID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@PriceTypeID", PriceTypeID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SC_PricesTypes_Delete",true,parameters);
}
public void DeleteVirtual(int PriceTypeID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@PriceTypeID", PriceTypeID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("SC_PricesTypes_DeleteVirtual",true,parameters);
}




}
}
