using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.Accounting.MasterData.CostCenter
{
public class CostCenter
{

public int CostCenterID { get; set; }

public string? CostCenterNumber { get; set; }

public string? CostCenterNameAr { get; set; }

public string? CostCenterNameEn { get; set; }

public int? ParentID { get; set; }

public bool  IsMain { get; set; }

public int LevelID { get; set; }

public bool  Deleted { get; set; }

public int? BranchID { get; set; }

public CostCenter Clone()
{
return (CostCenter)MemberwiseClone();
}
}
public class CostCenterService : Interfaces.IScopedService
{

public string GetCode(Main main)
{
            DataTable dataTable = main.ExecuteQuery_DataTable("A_CostCenters_GetCode",true,null);
return dataTable.Rows[0][0].ToString();
}
        public string GetCode(string ParentID, Main main)
        {
            string strSql = string.Empty;
            strSql += "  ";
            strSql += " select dbo.A_CostCenters_GetCode(   ";
            strSql += ParentID + ") as NewCode";	//ParentID
            DataTable dataTable =  main.ExecuteQuery_DataTable(strSql, false, null);
            return dataTable.Rows[0][0].ToString();
        }

public int Insert_Update(CostCenter obj,int UserID,Main main)
{
SqlParameter[] parameters = new SqlParameter[10];
parameters[0] = new SqlParameter("@CostCenterID", obj.CostCenterID);
parameters[1] = new SqlParameter("@CostCenterNumber", obj.CostCenterNumber);
parameters[2] = new SqlParameter("@CostCenterNameAr", obj.CostCenterNameAr);
parameters[3] = new SqlParameter("@CostCenterNameEn", obj.CostCenterNameEn);
parameters[4] = new SqlParameter("@ParentID", obj.ParentID==null?DBNull.Value:obj.ParentID);
parameters[5] = new SqlParameter("@IsMain", obj.IsMain);
parameters[6] = new SqlParameter("@LevelID", obj.LevelID);
parameters[7] = new SqlParameter("@Deleted", obj.Deleted);
parameters[8] = new SqlParameter("@BranchID", obj.BranchID);
parameters[9] = new SqlParameter("@UserID",UserID);
            DataTable dataTable = main.ExecuteQuery_DataTable("A_CostCenters_Insert_Update",true,parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}
        public DataTable FillCombo(bool IsArabic, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@IsArabic", IsArabic);
            DataTable dataTable = main.ExecuteQuery_DataTable("A_CostCenters_Fill_Combo", true, parameters);
            return dataTable;
        }
        public List<CostCenter> Select(int CostCenterID, string BranchIDs, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[3];
parameters[0] = new SqlParameter("@CostCenterID", CostCenterID);
parameters[1] = new SqlParameter("@BranchIDs",BranchIDs);
parameters[2] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable =main.ExecuteQuery_DataTable("A_CostCenters_Select",true,parameters);
List<CostCenter> lst = new List<CostCenter>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<CostCenter>(dataTable);
}
return lst;
}

public void Delete(int CostCenterID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CostCenterID", CostCenterID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_CostCenters_Delete",true,parameters);
}
public void DeleteVirtual(int CostCenterID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CostCenterID", CostCenterID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_CostCenters_DeleteVirtual",true,parameters);
}








}
}
