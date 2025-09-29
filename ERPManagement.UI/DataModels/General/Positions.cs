using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataModels.Interfaces;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.General
{
public class Positions
{

public int PositionID { get; set; }

public string PositionNameAr { get; set; }

public string? PositionNameEn { get; set; }

public bool  Deleted { get; set; }

public int BranchID { get; set; }

public Positions Clone()
{
return (Positions)MemberwiseClone();
}
}
public class PositionsService : IScopedService
    {

        public DataTable FillCombo(bool IsArabic, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@IsArabic", IsArabic);
            DataTable dataTable = main.ExecuteQuery_DataTable("G_Positions_FillCombo", true, parameters);
            return dataTable;
        }
        public int Insert_Update(Positions obj,int UserID,Main main)
{
SqlParameter[] parameters = new SqlParameter[6];
parameters[0] = new SqlParameter("@PositionID", obj.PositionID== null ? DBNull.Value : obj.PositionID);
parameters[1] = new SqlParameter("@PositionNameAr", obj.PositionNameAr== null ? DBNull.Value : obj.PositionNameAr);
parameters[2] = new SqlParameter("@PositionNameEn", obj.PositionNameEn== null ? DBNull.Value : obj.PositionNameEn);
parameters[3] = new SqlParameter("@Deleted", obj.Deleted== null ? false : obj.Deleted);
parameters[4] = new SqlParameter("@BranchID", obj.BranchID== null ? DBNull.Value : obj.BranchID);
parameters[5] = new SqlParameter("@UserID",UserID);
DataTable dataTable = main.ExecuteQuery_DataTable("G_Positions_Insert_Update",true,parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}

public List<Positions> Select(int PositionID, string BranchIDs, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[3];
parameters[0] = new SqlParameter("@PositionID", PositionID);
parameters[1] = new SqlParameter("@BranchIDs",BranchIDs);
parameters[2] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable =main.ExecuteQuery_DataTable("G_Positions_Select",true,parameters);
List<Positions> lst = new List<Positions>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<Positions>(dataTable);
}
return lst;
}

public void Delete(int PositionID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@PositionID", PositionID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("G_Positions_Delete",true,parameters);
}
public void DeleteVirtual(int PositionID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@PositionID", PositionID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("G_Positions_DeleteVirtual",true,parameters);
}


public List<Positions> SelectByBranchID(int BranchID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("G_Positions_SelectByBranchID",true,parameters);
List<Positions> lst = new List<Positions>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<Positions>(dataTable);
}
return lst;
}

public void DeleteByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("G_Positions_DeleteByBranchID",true,parameters);
}
public void DeleteVirtualByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("G_Positions_DeleteVirtualByBranchID" ,true,parameters);
}

}
}
