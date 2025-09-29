using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.Accounting
{
public class SubAccountsClientCards
{

public int SubAccountClientCardID { get; set; }

public string? CardCode { get; set; }

public bool  IsActive { get; set; }

public string? Notes { get; set; }

public int SubAccountID { get; set; }

public DateTime? InsertDate { get; set; }

public bool  Deleted { get; set; }

public int BranchID { get; set; }

public SubAccountsClientCards Clone()
{
return (SubAccountsClientCards)MemberwiseClone();
}
}
public class SubAccountsClientCardsService : Interfaces.IScopedService
{

        public int Insert_UpdateByTableXML(List<SubAccountsClientCards> lst, int SubAccountID, int BranchID, int UserID, Main main)
        {
            string lstXML = main.ToXml(lst);
            string strSql = string.Empty;
            strSql += " A_SubAccountsClientCards_Insert_UpdateXML '";
            strSql += lstXML + "',";
            strSql += SubAccountID + ",";
            strSql += BranchID + ",";
            strSql += UserID;

            DataTable dataTable = main.ExecuteQuery_DataTable(strSql, false, null);
            return int.Parse(dataTable.Rows[0][0].ToString());
        }
        public int Insert_Update(SubAccountsClientCards obj,int UserID,Main main)
{
SqlParameter[] parameters = new SqlParameter[9];
parameters[0] = new SqlParameter("@SubAccountClientCardID", obj.SubAccountClientCardID== null ? DBNull.Value : obj.SubAccountClientCardID);
parameters[1] = new SqlParameter("@CardCode", obj.CardCode== null ? DBNull.Value : obj.CardCode);
parameters[2] = new SqlParameter("@IsActive", obj.IsActive== null ? false : obj.IsActive);
parameters[3] = new SqlParameter("@Notes", obj.Notes== null ? DBNull.Value : obj.Notes);
parameters[4] = new SqlParameter("@SubAccountID", obj.SubAccountID== null ? DBNull.Value : obj.SubAccountID);
parameters[5] = new SqlParameter("@InsertDate", obj.InsertDate== null ? DBNull.Value : obj.InsertDate);
parameters[6] = new SqlParameter("@Deleted", obj.Deleted== null ? false : obj.Deleted);
parameters[7] = new SqlParameter("@BranchID", obj.BranchID== null ? DBNull.Value : obj.BranchID);
parameters[8] = new SqlParameter("@UserID",UserID);
DataTable dataTable = main.ExecuteQuery_DataTable("A_SubAccountsClientCards_Insert_Update",true,parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}

public List<SubAccountsClientCards> Select(int SubAccountClientCardID, string BranchIDs, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[3];
parameters[0] = new SqlParameter("@SubAccountClientCardID", SubAccountClientCardID);
parameters[1] = new SqlParameter("@BranchIDs",BranchIDs);
parameters[2] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable =main.ExecuteQuery_DataTable("A_SubAccountsClientCards_Select",true,parameters);
List<SubAccountsClientCards> lst = new List<SubAccountsClientCards>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SubAccountsClientCards>(dataTable);
}
return lst;
}

public void Delete(int SubAccountClientCardID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountClientCardID", SubAccountClientCardID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_SubAccountsClientCards_Delete",true,parameters);
}
public void DeleteVirtual(int SubAccountClientCardID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountClientCardID", SubAccountClientCardID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_SubAccountsClientCards_DeleteVirtual",true,parameters);
}


public List<SubAccountsClientCards> SelectBySubAccountID(int SubAccountID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("A_SubAccountsClientCards_SelectBySubAccountID",true,parameters);
List<SubAccountsClientCards> lst = new List<SubAccountsClientCards>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SubAccountsClientCards>(dataTable);
}
return lst;
}

public void DeleteBySubAccountID(int SubAccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_SubAccountsClientCards_DeleteBySubAccountID",true,parameters);
}
public void DeleteVirtualBySubAccountID(int SubAccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_SubAccountsClientCards_DeleteVirtualBySubAccountID" ,true,parameters);
}

public List<SubAccountsClientCards> SelectByBranchID(int BranchID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("A_SubAccountsClientCards_SelectByBranchID",true,parameters);
List<SubAccountsClientCards> lst = new List<SubAccountsClientCards>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SubAccountsClientCards>(dataTable);
}
return lst;
}

public void DeleteByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_SubAccountsClientCards_DeleteByBranchID",true,parameters);
}
public void DeleteVirtualByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_SubAccountsClientCards_DeleteVirtualByBranchID" ,true,parameters);
}

}
}
