using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataModels.Interfaces;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.General
{
public class Titles
{

public int TitleID { get; set; }

public string TitleNameAr { get; set; }

public string? TitleNameEn { get; set; }

public bool  Deleted { get; set; }

public int BranchID { get; set; }

public Titles Clone()
{
return (Titles)MemberwiseClone();
}
}
public class TitlesService : IScopedService
    {

public string GetCode(Main main)
{
            DataTable dataTable = main.ExecuteQuery_DataTable("G_Titles_GetCode",true,null);
return dataTable.Rows[0][0].ToString();
}
public string GetCodeByBranchID(DateTime Date,string BranchID,Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@Date", Date);
parameters[1] = new SqlParameter("@BranchID",BranchID);
DataTable dataTable = main.ExecuteQuery_DataTable("G_Titles_GetCodeByBranchID",true,parameters);
return dataTable.Rows[0][0].ToString();
}

public int Insert_Update(Titles obj,int UserID,Main main)
{
SqlParameter[] parameters = new SqlParameter[6];
parameters[0] = new SqlParameter("@TitleID", obj.TitleID);
parameters[1] = new SqlParameter("@TitleNameAr", obj.TitleNameAr);
parameters[2] = new SqlParameter("@TitleNameEn", obj.TitleNameEn);
parameters[3] = new SqlParameter("@Deleted", obj.Deleted);
parameters[4] = new SqlParameter("@BranchID", obj.BranchID);
parameters[5] = new SqlParameter("@UserID",UserID);
DataTable dataTable = main.ExecuteQuery_DataTable("G_Titles_Insert_Update",true,parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}

public List<Titles> Select(int TitleID, string BranchIDs, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[3];
parameters[0] = new SqlParameter("@TitleID", TitleID);
parameters[1] = new SqlParameter("@BranchIDs",BranchIDs);
parameters[2] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable =main.ExecuteQuery_DataTable("G_Titles_Select",true,parameters);
List<Titles> lst = new List<Titles>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<Titles>(dataTable);
}
return lst;
}

public void Delete(int TitleID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@TitleID", TitleID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("G_Titles_Delete",true,parameters);
}
public void DeleteVirtual(int TitleID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@TitleID", TitleID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("G_Titles_DeleteVirtual",true,parameters);
}




}
}
