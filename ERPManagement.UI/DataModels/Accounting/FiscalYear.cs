using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataModels.General;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.Accounting
{
public class FiscalYear
{

public string FiscalYearName { get; set; }

public bool  Confirmed { get; set; }

public bool  Closed { get; set; }

public bool  Deleted { get; set; }

public int? BranchID { get; set; }

public FiscalYear Clone()
{
return (FiscalYear)MemberwiseClone();
}
}
public class FiscalYearService : Interfaces.IScopedService
{
        public bool ChkForConfirmedFiscalYear(string Date,Main main)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@Date", Date);
            DataTable dataTable = main.ExecuteQuery_DataTable("A_Fiscal_Year_GetConfirmed", true, parameters);
            return dataTable.Rows.Count>0?true:false;
        }
        public bool ChkForClosingFsicalPeriod(string Date, string BranchID, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@Date", Date);
            parameters[1] = new SqlParameter("@BranchID", BranchID);
            DataTable dataTable = main.ExecuteQuery_DataTable("A_Fiscal_Year_GetClosedPeriod", true, parameters);
            return dataTable.Rows.Count > 0 ? true : false;
        }
        public string GetCode(Main main)
{
DataTable dataTable=main.ExecuteQuery_DataTable("A_FiscalYear_GetCode",true,null);
return dataTable.Rows[0][0].ToString();
}
public string GetCodeByBranchID(DateTime Date,string BranchID,Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@Date", Date);
parameters[1] = new SqlParameter("@BranchID",BranchID);
DataTable dataTable = main.ExecuteQuery_DataTable("A_FiscalYear_GetCodeByBranchID",true,parameters);
return dataTable.Rows[0][0].ToString();
}

public int Insert_Update(FiscalYear obj,int UserID,Main main)
{
SqlParameter[] parameters = new SqlParameter[6];
parameters[1] = new SqlParameter("@FiscalYearName", obj.FiscalYearName== null ? DBNull.Value : obj.FiscalYearName);
parameters[2] = new SqlParameter("@Confirmed", obj.Confirmed== null ? false : obj.Confirmed);
parameters[3] = new SqlParameter("@Closed", obj.Closed== null ? false : obj.Closed);
parameters[4] = new SqlParameter("@Deleted", obj.Deleted== null ? false : obj.Deleted);
parameters[5] = new SqlParameter("@BranchID", obj.BranchID== null ? DBNull.Value : obj.BranchID);
parameters[5] = new SqlParameter("@UserID",UserID);
DataTable dataTable = main.ExecuteQuery_DataTable("A_FiscalYear_Insert_Update",true,parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}

public List<FiscalYear> Select(int FiscalYearID, string BranchIDs, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[3];
parameters[0] = new SqlParameter("@FiscalYearID", FiscalYearID);
parameters[1] = new SqlParameter("@BranchIDs",BranchIDs);
parameters[2] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable =main.ExecuteQuery_DataTable("A_FiscalYear_Select",true,parameters);
List<FiscalYear> lst = new List<FiscalYear>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<FiscalYear>(dataTable);
}
return lst;
}

public void Delete(int FiscalYearID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@FiscalYearID", FiscalYearID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_FiscalYear_Delete",true,parameters);
}
public void DeleteVirtual(int FiscalYearID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@FiscalYearID", FiscalYearID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_FiscalYear_DeleteVirtual",true,parameters);
}


public List<FiscalYear> SelectByBranchID(int BranchID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("A_FiscalYear_SelectByBranchID",true,parameters);
List<FiscalYear> lst = new List<FiscalYear>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<FiscalYear>(dataTable);
}
return lst;
}

public void DeleteByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_FiscalYear_DeleteByBranchID",true,parameters);
}
public void DeleteVirtualByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_FiscalYear_DeleteVirtualByBranchID" ,true,parameters);
}

}
}
