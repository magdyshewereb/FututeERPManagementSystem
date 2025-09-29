using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.Accounting
{
public class SubAccounts_Details
{

public int SubAccountDetailID { get; set; }

public int SubAccountID { get; set; }

public int AccountID { get; set; }

public bool  Deleted { get; set; }

public int? BranchID { get; set; }

public SubAccounts_Details Clone()
{
return (SubAccounts_Details)MemberwiseClone();
}
}
public class SubAccounts_DetailsService : Interfaces.IScopedService
{

public string GetCode(Main main)
{
            DataTable dataTable = main.ExecuteQuery_DataTable("A_SubAccounts_Details_GetCode",true,null);
return dataTable.Rows[0][0].ToString();
}
public string GetCodeByBranchID(DateTime Date,string BranchID,Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@Date", Date);
parameters[1] = new SqlParameter("@BranchID",BranchID);
DataTable dataTable = main.ExecuteQuery_DataTable("A_SubAccounts_Details_GetCodeByBranchID",true,parameters);
return dataTable.Rows[0][0].ToString();
}

public int Insert_Update(SubAccounts_Details obj,int UserID,Main main)
{
SqlParameter[] parameters = new SqlParameter[6];
parameters[0] = new SqlParameter("@SubAccountDetailID", obj.SubAccountDetailID);
parameters[1] = new SqlParameter("@SubAccountID", obj.SubAccountID);
parameters[2] = new SqlParameter("@AccountID", obj.AccountID);
parameters[3] = new SqlParameter("@Deleted", obj.Deleted);
parameters[4] = new SqlParameter("@BranchID", obj.BranchID);
parameters[5] = new SqlParameter("@UserID",UserID);
DataTable dataTable = main.ExecuteQuery_DataTable("A_SubAccounts_Details_Insert_Update",true,parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}
        public void Insert_UpdateByAccIDs(int SubAccountID, string AccountIDs, string Deleted, string BranchID, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[4];
            parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
            parameters[1] = new SqlParameter("@AccountIDs", AccountIDs);
            parameters[2] = new SqlParameter("@Deleted", Deleted);
            parameters[3] = new SqlParameter("@BranchID", BranchID);

            DataTable dataTable =  main.ExecuteQuery_DataTable("A_SubAccounts_Details_Insert_UpdateByAccIDs", true, parameters);
            //return int.Parse(dataTable.Rows[0][0].ToString());
            //DataLayer.DatabaseLayer.CloseConnection(Main.Success);
        }
        public List<SubAccounts_Details> Select(int SubAccountDetailID, string BranchIDs, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[3];
parameters[0] = new SqlParameter("@SubAccountDetailID", SubAccountDetailID);
parameters[1] = new SqlParameter("@BranchIDs",BranchIDs);
parameters[2] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable =main.ExecuteQuery_DataTable("A_SubAccounts_Details_Select",true,parameters);
List<SubAccounts_Details> lst = new List<SubAccounts_Details>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SubAccounts_Details>(dataTable);
}
return lst;
}

public void Delete(int SubAccountDetailID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountDetailID", SubAccountDetailID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_SubAccounts_Details_Delete",true,parameters);
}
        public void DeleteBySubAccountID(int SubAccountID, int UserID, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
            parameters[1] = new SqlParameter("@UserID", UserID);
            main.ExecuteNonQuery("A_SubAccounts_Details_DeleteBySubAccountID", true, parameters);
        }
        public void DeleteVirtual(int SubAccountDetailID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountDetailID", SubAccountDetailID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_SubAccounts_Details_DeleteVirtual",true,parameters);
}








}
}
