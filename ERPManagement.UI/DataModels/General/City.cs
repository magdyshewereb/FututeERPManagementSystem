using System.Data;
using ERPManagement.UI.DataAccess;
using ERPManagement.UI.DataModels.Interfaces;
using ERPManagement.UI.DataModels.Accounting;
using Microsoft.Data.SqlClient;

namespace ERPManagement.UI.DataModels.General
{
public class City
{

public int CityID { get; set; }

public int? CountryID { get; set; }

public string? CityCode { get; set; }

public string? CityNameAr { get; set; }

public string? CityNameEn { get; set; }

public bool? Deleted { get; set; }

public int? BranchID { get; set; }
        public City Clone()
        {
            return (City)MemberwiseClone();
        }

    }
public class CityService : IScopedService
    {

public string GetCode(Main main, bool IsFromServer)
{
DataTable dataTable = IsFromServer ? main.SyncExecuteQuery_DataTable("G_Cities_GetCode" , false, null) : main.ExecuteQuery_DataTable("G_Cities_GetCode",false, null);
return dataTable.Rows[0][0].ToString();
}
public string GetCodeByBranchID(DateTime Date,int BranchID,Main main,bool IsFromServer)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@Date", Date);
parameters[1] = new SqlParameter("@BranchID",BranchID);
DataTable dataTable = IsFromServer ? main.SyncExecuteQuery_DataTable("G_Cities_GetCodeByBranchID" , true,parameters) : main.ExecuteQuery_DataTable("G_Cities_GetCodeByBranchID", true, parameters);
return dataTable.Rows[0][0].ToString();
}

public int Insert_Update(City obj,int UserID,Main main,bool IsFromServer)
{
SqlParameter[] parameters = new SqlParameter[8];
parameters[0] = new SqlParameter("@CityID", obj.CityID);
parameters[1] = new SqlParameter("@CountryID", obj.CountryID);
parameters[2] = new SqlParameter("@CityCode", obj.CityCode);
parameters[3] = new SqlParameter("@CityNameAr", obj.CityNameAr);
parameters[4] = new SqlParameter("@CityNameEn", obj.CityNameEn);
parameters[5] = new SqlParameter("@Deleted", obj.Deleted);
parameters[6] = new SqlParameter("@BranchID", obj.BranchID);
parameters[7] = new SqlParameter("@UserID",UserID);
 DataTable dataTable = IsFromServer ? main.SyncExecuteQuery_DataTable("G_Cities_Insert_Update"  ,true, parameters) : main.ExecuteQuery_DataTable("G_Cities_Insert_Update", true, parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}

public List<City> Select(int CityID, string BranchIDs, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[3];
parameters[0] = new SqlParameter("@CityID", CityID);
parameters[1] = new SqlParameter("@BranchIDs",BranchIDs);
parameters[2] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable =  main.ExecuteQuery_DataTable("G_Cities_Select", true, parameters);
List<City> lst = new List<City>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<City>(dataTable);
}
return lst;
}

public void Delete(int CityID, int UserID, Main main, bool IsFromServer)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CityID", CityID);
parameters[1] = new SqlParameter("@UserID",UserID);
if (IsFromServer)
main.SyncExecuteNonQuery("G_Cities_Delete"  , true, parameters); 
else
main.ExecuteNonQuery("G_Cities_Delete", true, parameters);
}
public void DeleteVirtual(int CityID, int UserID, Main main, bool IsFromServer)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CityID", CityID);
parameters[1] = new SqlParameter("@UserID",UserID);
if (IsFromServer)
main.SyncExecuteNonQuery("G_Cities_DeleteVirtual"  , true, parameters); 
else
main.ExecuteNonQuery("G_Cities_DeleteVirtual", true, parameters);
}


public List<City> SelectByBranchID(int BranchID, bool IsArabic, Main main, bool IsFromServer)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = IsFromServer ? main.SyncExecuteQuery_DataTable("G_Cities_SelectByBranchID"  , true, parameters) : main.ExecuteQuery_DataTable("G_Cities_SelectBy BranchID", true, parameters);
List<City> lst = new List<City>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<City>(dataTable);
}
return lst;
}


public List<City> SelectByCountryID(int CountryID, bool IsArabic, Main main, bool IsFromServer)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CountryID", CountryID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = IsFromServer ? main.SyncExecuteQuery_DataTable(" G_Cities_SelectByCountryID "  , true, parameters) : main.ExecuteQuery_DataTable(" G_Cities_SelectBy CountryID", true, parameters);
List<City> lst = new List<City>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<City>(dataTable);
}
return lst;
}


}
}
