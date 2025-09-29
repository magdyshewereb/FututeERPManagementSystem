using System.Data;
using ERPManagement.UI.DataAccess;
using ERPManagement.UI.DataModels.Interfaces;
using ERPManagement.UI.DataModels.Accounting;
using Microsoft.Data.SqlClient;

namespace ERPManagement.UI.DataModels.General
{
public class Area
{

public int AreaID { get; set; }

public string AreaNameAr { get; set; }

public string? AreaNameEn { get; set; }

public int CountryID { get; set; }

public int CityID { get; set; }

public decimal? DeliveryFees { get; set; }

public bool Deleted { get; set; }

public int BranchID { get; set; }
        public Area Clone()
        {
            return (Area)MemberwiseClone();
        }

    }
public class AreaService : IScopedService
    {

public string GetCode(Main main, bool IsFromServer)
{
DataTable dataTable = IsFromServer ? main.SyncExecuteQuery_DataTable(" G_Areas_GetCode " , false,null) : main.ExecuteQuery_DataTable(" G_Areas_GetCode ",false, null);
return dataTable.Rows[0][0].ToString();
}
public string GetCodeByBranchID(DateTime Date,int BranchID,Main main,bool IsFromServer)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@Date", Date);
parameters[1] = new SqlParameter("@BranchID",BranchID);
DataTable dataTable = IsFromServer ? main.SyncExecuteQuery_DataTable(" G_Areas_GetCodeByBranchID  " , true,parameters) : main.ExecuteQuery_DataTable(" G_Areas_GetCodeByBranchID ", true,parameters);
return dataTable.Rows[0][0].ToString();
}

public int Insert_Update(Area obj,int UserID,Main main,bool IsFromServer)
{
SqlParameter[] parameters = new SqlParameter[9];
parameters[0] = new SqlParameter("@AreaID", obj.AreaID);
parameters[1] = new SqlParameter("@AreaNameAr", obj.AreaNameAr);
parameters[2] = new SqlParameter("@AreaNameEn", obj.AreaNameEn);
parameters[3] = new SqlParameter("@CountryID", obj.CountryID);
parameters[4] = new SqlParameter("@CityID", obj.CityID);
parameters[5] = new SqlParameter("@DeliveryFees", obj.DeliveryFees);
parameters[6] = new SqlParameter("@Deleted", obj.Deleted);
parameters[7] = new SqlParameter("@BranchID", obj.BranchID);
parameters[8] = new SqlParameter("@UserID",UserID);
 DataTable dataTable = IsFromServer ? main.SyncExecuteQuery_DataTable(" G_Areas_Insert_Update  "  ,true, parameters) : main.ExecuteQuery_DataTable(" G_Areas_Insert_Update  ", true, parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}

public List<Area> Select(int AreaID, string BranchIDs, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[3];
parameters[0] = new SqlParameter("@AreaID", AreaID);
parameters[1] = new SqlParameter("@BranchIDs",BranchIDs);
parameters[2] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("G_Areas_Select", true, parameters);
List<Area> lst = new List<Area>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<Area>(dataTable);
}
return lst;
}

public void Delete(int AreaID, int UserID, Main main, bool IsFromServer)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@AreaID", AreaID);
parameters[1] = new SqlParameter("@UserID",UserID);
if (IsFromServer)
main.SyncExecuteNonQuery(" G_Areas_Delete  "  ,true, parameters); 
else
main.ExecuteNonQuery(" G_Areas_Delete  ", true, parameters);
}
public void DeleteVirtual(int AreaID, int UserID, Main main, bool IsFromServer)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@AreaID", AreaID);
parameters[1] = new SqlParameter("@UserID",UserID);
if (IsFromServer)
main.SyncExecuteNonQuery(" G_Areas_DeleteVirtual  "  ,true, parameters); 
else
main.ExecuteNonQuery(" G_Areas_DeleteVirtual  ", true, parameters);
}


public List<Area> SelectByBranchID(int BranchID, bool IsArabic, Main main, bool IsFromServer)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = IsFromServer ? main.SyncExecuteQuery_DataTable(" G_Areas_SelectByBranchID  "  ,true, parameters) : main.ExecuteQuery_DataTable(" G_Areas_SelectBy BranchID   ", true, parameters);
List<Area> lst = new List<Area>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<Area>(dataTable);
}
return lst;
}

public void DeleteByBranchID(int BranchID, int UserID, Main main, bool IsFromServer)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
if (IsFromServer)
main.SyncExecuteNonQuery(" G_Areas_DeleteByBranchID   "  ,true, parameters); 
else
main.ExecuteNonQuery(" G_Areas_DeleteByBranchID  ", true, parameters);
}
public void DeleteVirtualByBranchID(int BranchID, int UserID, Main main, bool IsFromServer)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
if (IsFromServer)
main.SyncExecuteNonQuery(" G_Areas_DeleteVirtualByBranchID   "  ,true, parameters); 
else
main.ExecuteNonQuery(" G_Areas_DeleteVirtualByBranchID  ", true, parameters);
}

public List<Area> SelectByCityID(int CityID, bool IsArabic, Main main, bool IsFromServer)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CityID", CityID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = IsFromServer ? main.SyncExecuteQuery_DataTable(" G_Areas_SelectByCityID  "  ,true, parameters) : main.ExecuteQuery_DataTable(" G_Areas_SelectBy CityID   ", true, parameters);
List<Area> lst = new List<Area>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<Area>(dataTable);
}
return lst;
}

public void DeleteByCityID(int CityID, int UserID, Main main, bool IsFromServer)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CityID", CityID);
parameters[1] = new SqlParameter("@UserID",UserID);
if (IsFromServer)
main.SyncExecuteNonQuery(" G_Areas_DeleteByCityID   "  ,true, parameters); 
else
main.ExecuteNonQuery(" G_Areas_DeleteByCityID  ", true, parameters);
}
public void DeleteVirtualByCityID(int CityID, int UserID, Main main, bool IsFromServer)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CityID", CityID);
parameters[1] = new SqlParameter("@UserID",UserID);
if (IsFromServer)
main.SyncExecuteNonQuery(" G_Areas_DeleteVirtualByCityID   "  ,true, parameters); 
else
main.ExecuteNonQuery(" G_Areas_DeleteVirtualByCityID  ", true, parameters);
}

public List<Area> SelectByCountryID(int CountryID, bool IsArabic, Main main, bool IsFromServer)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CountryID", CountryID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = IsFromServer ? main.SyncExecuteQuery_DataTable(" G_Areas_SelectByCountryID  "  ,true, parameters) : main.ExecuteQuery_DataTable(" G_Areas_SelectBy CountryID   ", true,  parameters);
List<Area> lst = new List<Area>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<Area>(dataTable);
}
return lst;
}

public void DeleteByCountryID(int CountryID, int UserID, Main main, bool IsFromServer)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CountryID", CountryID);
parameters[1] = new SqlParameter("@UserID",UserID);
if (IsFromServer)
main.SyncExecuteNonQuery(" G_Areas_DeleteByCountryID   "  ,true, parameters); 
else
main.ExecuteNonQuery(" G_Areas_DeleteByCountryID  ", true, parameters);
}
public void DeleteVirtualByCountryID(int CountryID, int UserID, Main main, bool IsFromServer)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@CountryID", CountryID);
parameters[1] = new SqlParameter("@UserID",UserID);
if (IsFromServer)
main.SyncExecuteNonQuery(" G_Areas_DeleteVirtualByCountryID   "  ,true, parameters); 
else
main.ExecuteNonQuery(" G_Areas_DeleteVirtualByCountryID  ", true, parameters);
}

}
}
