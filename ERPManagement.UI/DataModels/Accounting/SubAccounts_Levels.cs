using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.Accounting
{
public class SubAccounts_Levels
{

public int LevelID { get; set; }
public int Level { get; set; }

public int Width { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public bool  Deleted { get; set; }

public int? BranchID { get; set; }

public SubAccounts_Levels Clone()
{
return (SubAccounts_Levels)MemberwiseClone();
}
}
public class SubAccounts_LevelsService : Interfaces.IScopedService
{


        public int Insert_Update(SubAccounts_Levels obj, int UserID, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[5];
            parameters[0] = new SqlParameter("@LevelID", obj.LevelID);
            parameters[1] = new SqlParameter("@Width", obj.Width);
            parameters[2] = new SqlParameter("@Deleted", obj.Deleted);
            parameters[3] = new SqlParameter("@BranchID", obj.BranchID);
            parameters[4] = new SqlParameter("@UserID", UserID);
            DataTable dataTable =  main.ExecuteQuery_DataTable("A_SubAccounts_Levels_Insert_Update  ", true, parameters);
            return int.Parse(dataTable.Rows[0][0].ToString());
        }

public List<SubAccounts_Levels> Select(int LevelID, string BranchIDs, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[3];
parameters[0] = new SqlParameter("@LevelID", LevelID);
parameters[1] = new SqlParameter("@BranchIDs",BranchIDs);
parameters[2] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable =main.ExecuteQuery_DataTable("A_SubAccounts_Levels_Select",true,parameters);
List<SubAccounts_Levels> lst = new List<SubAccounts_Levels>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SubAccounts_Levels>(dataTable);
}
return lst;
}





}
}
