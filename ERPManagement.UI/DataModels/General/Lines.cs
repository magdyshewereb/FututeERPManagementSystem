using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataModels.Interfaces;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.General
{
public class Lines
{

public int LineID { get; set; }

public string? LineCode { get; set; }

public string? LineNameAr { get; set; }

public string? LineNameEn { get; set; }

public int? LineBranchID { get; set; }

public bool  Deleted { get; set; }

public int BranchID { get; set; }

public Lines Clone()
{
return (Lines)MemberwiseClone();
}
}
public class LinesService : IScopedService
    {

public string GetCode(Main main)
{
            DataTable dataTable = main.ExecuteQuery_DataTable("G_Lines_GetCode",true,null);
return dataTable.Rows[0][0].ToString();
}
public string GetCodeByBranchID(DateTime Date,string BranchID,Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@Date", Date);
parameters[1] = new SqlParameter("@BranchID",BranchID);
DataTable dataTable = main.ExecuteQuery_DataTable("G_Lines_GetCodeByBranchID",true,parameters);
return dataTable.Rows[0][0].ToString();
}

public int Insert_Update(Lines obj,int UserID,Main main)
{
SqlParameter[] parameters = new SqlParameter[8];
parameters[0] = new SqlParameter("@LineID", obj.LineID);
parameters[1] = new SqlParameter("@LineCode", obj.LineCode);
parameters[2] = new SqlParameter("@LineNameAr", obj.LineNameAr);
parameters[3] = new SqlParameter("@LineNameEn", obj.LineNameEn);
parameters[4] = new SqlParameter("@LineBranchID", obj.LineBranchID);
parameters[5] = new SqlParameter("@Deleted", obj.Deleted);
parameters[6] = new SqlParameter("@BranchID", obj.BranchID);
parameters[7] = new SqlParameter("@UserID",UserID);
DataTable dataTable = main.ExecuteQuery_DataTable("G_Lines_Insert_Update",true,parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}
        public  DataTable FillCombo(bool IsArabic, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@IsArabic", IsArabic);
            DataTable dataTable = main.ExecuteQuery_DataTable("G_Lines_FillCombo", true, parameters);
            return dataTable;
        }
        public List<Lines> Select(int LineID, string BranchIDs, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[3];
parameters[0] = new SqlParameter("@LineID", LineID);
parameters[1] = new SqlParameter("@BranchIDs",BranchIDs);
parameters[2] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable =main.ExecuteQuery_DataTable("G_Lines_Select",true,parameters);
List<Lines> lst = new List<Lines>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<Lines>(dataTable);
}
return lst;
}

public void Delete(int LineID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@LineID", LineID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("G_Lines_Delete",true,parameters);
}
public void DeleteVirtual(int LineID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@LineID", LineID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("G_Lines_DeleteVirtual",true,parameters);
}






}
}
