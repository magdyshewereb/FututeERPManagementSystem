using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.Accounting
{
public class SubAccounts
{

public int SubAccountID { get; set; }

public string SubAccountNumber { get; set; }

public string SubAccountNameAr { get; set; }

public string? SubAccountNameEn { get; set; }

public int? ParentID { get; set; }

public bool  IsMain { get; set; }

public int LevelID { get; set; }

public int? SubAccountTypeID { get; set; }

public bool  ForAllBranches { get; set; }

public bool  Deleted { get; set; }

public int? BranchID { get; set; }

public SubAccounts Clone()
{
return (SubAccounts)MemberwiseClone();
}
}
public class SubAccountsService : Interfaces.IScopedService
{
        public string SelectRelations(string SubAccountID, string IsArabic, Main main)
        {
            string strSql = string.Empty;
            strSql += " A_SubAccounts_Relations ";
            strSql += SubAccountID + ",";
            strSql += IsArabic;
            DataTable dataTable = main.ExecuteQuery_DataTable(strSql, false, null);
            return dataTable.Rows[0][0].ToString();
        }
        public string GetCode(string ParentID, Main main)
        {
            string strSql = string.Empty;
            strSql += "  ";
            strSql += " select dbo.A_SubAccounts_GetCode(   ";
            strSql += ParentID + ") as NewCode";	//ParentID
            DataTable dataTable = main.ExecuteQuery_DataTable(strSql, false, null);
            return dataTable.Rows[0][0].ToString();
        }
public string GetCodeByBranchID(DateTime Date,string BranchID,Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@Date", Date);
parameters[1] = new SqlParameter("@BranchID",BranchID);
DataTable dataTable = main.ExecuteQuery_DataTable("A_SubAccounts_GetCodeByBranchID",true,parameters);
return dataTable.Rows[0][0].ToString();
}
        public  DataTable SelectBySubAccountIDWithAccountName(int SubAccountID, bool IsArabic, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
            parameters[1] = new SqlParameter("@IsArabic", IsArabic);
            DataTable dataTable = main.ExecuteQuery_DataTable("A_SubAccounts_Details_SelectBySubAccountIDWithAccountName", true, parameters);
            return dataTable;
        }
        public int Insert_Update(SubAccounts obj,int UserID,Main main)
{
SqlParameter[] parameters = new SqlParameter[12];
parameters[0] = new SqlParameter("@SubAccountID", obj.SubAccountID);
parameters[1] = new SqlParameter("@SubAccountNumber", obj.SubAccountNumber == null ? DBNull.Value : obj.SubAccountNumber);
parameters[2] = new SqlParameter("@SubAccountNameAr", obj.SubAccountNameAr == null ? DBNull.Value : obj.SubAccountNameAr);
parameters[3] = new SqlParameter("@SubAccountNameEn", obj.SubAccountNameEn == null ? obj.SubAccountNameAr : obj.SubAccountNameEn);
parameters[4] = new SqlParameter("@ParentID", obj.ParentID == null ? DBNull.Value : obj.ParentID);
parameters[5] = new SqlParameter("@IsMain", obj.IsMain == null ? false : obj.IsMain);
parameters[6] = new SqlParameter("@LevelID", obj.LevelID == null ? DBNull.Value : obj.LevelID);
parameters[7] = new SqlParameter("@SubAccountTypeID", obj.SubAccountTypeID == null ? DBNull.Value : obj.SubAccountTypeID);
parameters[8] = new SqlParameter("@ForAllBranches", obj.ForAllBranches == null ? false : obj.ForAllBranches);
parameters[9] = new SqlParameter("@Deleted", obj.Deleted == null ? false : obj.Deleted);
parameters[10] = new SqlParameter("@BranchID", obj.BranchID == null ? DBNull.Value : obj.BranchID);
parameters[11] = new SqlParameter("@UserID",UserID);
            DataTable dataTable = main.ExecuteQuery_DataTable("A_SubAccounts_Insert_Update",true,parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}

        public List<SubAccounts> Select(int SubAccountID, string BranchIDs, bool IsArabic, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
            parameters[1] = new SqlParameter("@BranchIDs", BranchIDs);
            parameters[2] = new SqlParameter("@IsArabic", IsArabic);
            DataTable dataTable = main.ExecuteQuery_DataTable("A_SubAccounts_Select", true, parameters);
            List<SubAccounts> lst = new List<SubAccounts>();
            if (dataTable != null)
            {
                lst = main.CreateListFromTable<SubAccounts>(dataTable);
            }
            return lst;
        }
public DataTable SelectByAccountID(int AccountID, bool IsArabic, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@Account_ID", AccountID);
            parameters[1] = new SqlParameter("@IsArabic", IsArabic);
            DataTable dataTable = main.ExecuteQuery_DataTable("A_SubAccounts_Fill_Combo_By_Account", true, parameters);
            //List<SubAccounts> lst = new List<SubAccounts>();
            //if (dataTable != null)
            //{
            //    lst = main.CreateListFromTable<SubAccounts>(dataTable);
            //}
            return dataTable;
        }
public void Delete(int SubAccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_SubAccounts_Delete",true,parameters);
}
public void DeleteVirtual(int SubAccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_SubAccounts_DeleteVirtual",true,parameters);
}

        public void SaveAccountsToChilds(int SubAccountID, string AccountIDs, int ConfirmUpdatechildAcc, int BranchID, int UserID, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[5];
            parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
            parameters[1] = new SqlParameter("@AccountIDs", AccountIDs);
            parameters[2] = new SqlParameter("@ConfirmUpdatechildAcc", ConfirmUpdatechildAcc);
            parameters[3] = new SqlParameter("@BranchID", BranchID);
            parameters[4] = new SqlParameter("@UserID", UserID);
           
            main.ExecuteQuery_DataTable("A_SubAccounts_SaveAccountsToChilds", true, parameters);
            //return dataTable;
        }
        public DataTable SelectBySubAccountTypeIDs(string SubAccountTypeIDs, bool IsArabic, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@SubAccountTypeIDs", SubAccountTypeIDs);
            parameters[1] = new SqlParameter("@IsArabic", IsArabic);
            DataTable dataTable = main.ExecuteQuery_DataTable("A_SubAccounts_SelectBySubAccountTypeIDs", true, parameters);
            return dataTable;
        }






    }
}
