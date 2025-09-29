using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.Accounting
{
public class SuAccountsTypes
{

public int SubAccountTypeID { get; set; }

public string SubAccountTypeNameAr { get; set; }

public string? SubAccountTypeNameEn { get; set; }

public bool  Deleted { get; set; }

public int? BranchID { get; set; }

public SuAccountsTypes Clone()
{
return (SuAccountsTypes)MemberwiseClone();
}
}
public class SuAccountsTypesService : Interfaces.IScopedService
{

public string GetCodeByBranchID(DateTime Date,string BranchID,Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@Date", Date);
parameters[1] = new SqlParameter("@BranchID",BranchID);
DataTable dataTable = main.ExecuteQuery_DataTable("A_SuAccountsTypes_GetCodeByBranchID",true,parameters);
return dataTable.Rows[0][0].ToString();
}

        public int Insert_Update(SuAccountsTypes obj, int UserID, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[6];
            parameters[0] = new SqlParameter("@SubAccountTypeID", obj.SubAccountTypeID);
            parameters[1] = new SqlParameter("@SubAccountTypeNameAr", obj.SubAccountTypeNameAr);
            parameters[2] = new SqlParameter("@SubAccountTypeNameEn", obj.SubAccountTypeNameEn);
            parameters[3] = new SqlParameter("@Deleted", obj.Deleted);
            parameters[4] = new SqlParameter("@BranchID", obj.BranchID);
            parameters[5] = new SqlParameter("@UserID", UserID);
            DataTable dataTable = main.ExecuteQuery_DataTable("A_SuAccountsTypes_Insert_Update", true, parameters);
            return int.Parse(dataTable.Rows[0][0].ToString());
        }

        public List<SuAccountsTypes> Select(int SubAccountTypeID, string BranchIDs, bool IsArabic, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@SubAccountTypeID", SubAccountTypeID);
            parameters[1] = new SqlParameter("@BranchIDs", BranchIDs);
            parameters[2] = new SqlParameter("@IsArabic", IsArabic);
            DataTable dataTable = main.ExecuteQuery_DataTable("A_SuAccountsTypes_Select", true, parameters);
            List<SuAccountsTypes> lst = new List<SuAccountsTypes>();
            if (dataTable != null)
            {
                lst = main.CreateListFromTable<SuAccountsTypes>(dataTable);
            }
            return lst;
        }

public void Delete(int SubAccountTypeID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountTypeID", SubAccountTypeID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_SuAccountsTypes_Delete",true,parameters);
}
public void DeleteVirtual(int SubAccountTypeID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountTypeID", SubAccountTypeID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_SuAccountsTypes_DeleteVirtual",true,parameters);
}


}
}
