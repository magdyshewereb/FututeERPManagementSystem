using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.Accounting
{
public class CostCenterLevel
{

public int LevelID { get; set; }
        public int Level { get; set; }

        public int Width { get; set; }
        public int Start { get; set; }
        public int End { get; set; }


        public bool  Deleted { get; set; }

public int? BranchID { get; set; }

public CostCenterLevel Clone()
{
return (CostCenterLevel)MemberwiseClone();
}
}
public class CostCenterLevelService : Interfaces.IScopedService
{

public string GetCode(Main main)
{
            DataTable dataTable = main.ExecuteQuery_DataTable("A_CostCenterLevels_GetCode",true,null);
return dataTable.Rows[0][0].ToString();
}
public string GetCodeByBranchID(DateTime Date,string BranchID,Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@Date", Date);
parameters[1] = new SqlParameter("@BranchID",BranchID);
DataTable dataTable = main.ExecuteQuery_DataTable("A_CostCenterLevels_GetCodeByBranchID",true,parameters);
return dataTable.Rows[0][0].ToString();
}

public int Insert_Update(CostCenterLevel obj,int UserID,Main main)
{
SqlParameter[] parameters = new SqlParameter[5];
parameters[0] = new SqlParameter("@LevelID", obj.LevelID);
parameters[1] = new SqlParameter("@Width", obj.Width);
parameters[2] = new SqlParameter("@Deleted", obj.Deleted);
parameters[3] = new SqlParameter("@BranchID", obj.BranchID);
parameters[4] = new SqlParameter("@UserID",UserID);
            DataTable dataTable = main.ExecuteQuery_DataTable("A_CostCenterLevels_Insert_Update",true,parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}

public List<CostCenterLevel> Select(int LevelID, string BranchIDs, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[3];
parameters[0] = new SqlParameter("@LevelID", LevelID);
parameters[1] = new SqlParameter("@BranchIDs",BranchIDs);
parameters[2] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable =main.ExecuteQuery_DataTable("A_CostCenterLevels_Select",true,parameters);
List<CostCenterLevel> lst = new List<CostCenterLevel>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<CostCenterLevel>(dataTable);
}
return lst;
}

public void Delete(int LevelID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@LevelID", LevelID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_CostCenterLevels_Delete",true,parameters);
}
public void DeleteVirtual(int LevelID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@LevelID", LevelID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_CostCenterLevels_DeleteVirtual",true,parameters);
}




}
}
