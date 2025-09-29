using System.Data;
using ERPManagement.UI.DataAccess;
using ERPManagement.UI.DataModels.Interfaces;
using ERPManagement.UI.DataModels.Accounting;
using Microsoft.Data.SqlClient;

namespace ERPManagement.UI.DataModels.General
{
public class Country
{

public int CountryID { get; set; }

public string? CountryCode { get; set; }

public string? CountryNameAr { get; set; }

public string? CountryNameEn { get; set; }

public int? EINVCountryID { get; set; }

public bool InEuropeanCommunity { get; set; }

public bool InGatt { get; set; }

public bool Deleted { get; set; }

public int BranchID { get; set; }
        public Country Clone()
        {
            return (Country)MemberwiseClone();
        }

    }
public class CountryService : IScopedService
    {

public string GetCode(Main main, bool IsFromServer)
{
DataTable dataTable = IsFromServer ? main.SyncExecuteQuery_DataTable(" G_Countries_GetCode" ,false, null) : main.ExecuteQuery_DataTable(" G_Countries_GetCode", false, null);
return dataTable.Rows[0][0].ToString();
}
public string GetCodeByBranchID(DateTime Date,int BranchID,Main main,bool IsFromServer)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@Date", Date);
parameters[1] = new SqlParameter("@BranchID",BranchID);
DataTable dataTable = IsFromServer ? main.SyncExecuteQuery_DataTable(" G_Countries_GetCodeByBranchID" ,true, parameters) : main.ExecuteQuery_DataTable(" G_Countries_GetCodeByBranchID", true, parameters);
return dataTable.Rows[0][0].ToString();
}

public int Insert_Update(Country obj,int UserID,Main main,bool IsFromServer)
{
SqlParameter[] parameters = new SqlParameter[10];
parameters[0] = new SqlParameter("@CountryID", obj.CountryID);
parameters[1] = new SqlParameter("@CountryCode", obj.CountryCode);
parameters[2] = new SqlParameter("@CountryNameAr", obj.CountryNameAr);
parameters[3] = new SqlParameter("@CountryNameEn", obj.CountryNameEn);
parameters[4] = new SqlParameter("@EINVCountryID", obj.EINVCountryID);
parameters[5] = new SqlParameter("@InEuropeanCommunity", obj.InEuropeanCommunity);
parameters[6] = new SqlParameter("@InGatt", obj.InGatt);
parameters[7] = new SqlParameter("@Deleted", obj.Deleted);
parameters[8] = new SqlParameter("@BranchID", obj.BranchID);
parameters[9] = new SqlParameter("@UserID",UserID);
 DataTable dataTable = IsFromServer ? main.SyncExecuteQuery_DataTable(" G_Countries_Insert_Update"  , true, parameters) : main.ExecuteQuery_DataTable(" G_Countries_Insert_Update", true, parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}

public List<Country> Select(int CountryID, string BranchIDs, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[3];
parameters[0] = new SqlParameter("@CountryID", CountryID);
parameters[1] = new SqlParameter("@BranchIDs",BranchIDs);
parameters[2] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable =main.ExecuteQuery_DataTable("G_Countries_Select", true, parameters);
List<Country> lst = new List<Country>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<Country>(dataTable);
}
return lst;
}

public void Delete(int CountryID, int UserID, Main main, bool IsFromServer)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CountryID", CountryID);
parameters[1] = new SqlParameter("@UserID",UserID);
if (IsFromServer)
main.SyncExecuteNonQuery("G_Countries_Delete  "  , true, parameters); 
else
main.ExecuteNonQuery("G_Countries_Delete  ", true, parameters);
}
public void DeleteVirtual(int CountryID, int UserID, Main main, bool IsFromServer)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CountryID", CountryID);
parameters[1] = new SqlParameter("@UserID",UserID);
if (IsFromServer)
main.SyncExecuteNonQuery("G_Countries_DeleteVirtual"  , true, parameters); 
else
main.ExecuteNonQuery("G_Countries_DeleteVirtual", true, parameters);
}


public List<Country> SelectByEINVCountryID(int EINVCountryID, bool IsArabic, Main main, bool IsFromServer)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@EINVCountryID", EINVCountryID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = IsFromServer ? main.SyncExecuteQuery_DataTable("G_Countries_SelectByEINVCountryID"  , true, parameters) : main.ExecuteQuery_DataTable("G_Countries_SelectBy EINVCountryID", true, parameters);
List<Country> lst = new List<Country>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<Country>(dataTable);
}
return lst;
}


public List<Country> SelectByBranchID(int BranchID, bool IsArabic, Main main, bool IsFromServer)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = IsFromServer ? main.SyncExecuteQuery_DataTable("G_Countries_SelectByBranchID"  , true, parameters) : main.ExecuteQuery_DataTable("G_Countries_SelectBy BranchID", true, parameters);
List<Country> lst = new List<Country>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<Country>(dataTable);
}
return lst;
}


}
}
