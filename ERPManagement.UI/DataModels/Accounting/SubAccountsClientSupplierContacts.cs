using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Xml.Serialization;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.Accounting
{
public class SubAccountsClientSupplierContacts
{

public int ClientSupplierContactID { get; set; }

public string ContactNo { get; set; }

public int SubAccountID { get; set; }

public string ContactName { get; set; }
        [XmlIgnore]
        public int? PositionID { get; set; }
        [XmlAttribute(AttributeName = "Position")]
        public string PositionSerializable
        {
            get
            {
                if (PositionID != null)
                {
                    return PositionID.ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                if (PositionSerializable != null)
                {
                    PositionID = int.Parse(PositionSerializable);
                }
            }
        }
        public DateTime? BirthDate { get; set; }

public string? ContactAddress { get; set; }

public string? ContactTel { get; set; }

public string? ContactFax { get; set; }

public string? ContactMobile { get; set; }

public string? ContactEMail { get; set; }

public bool  Deleted { get; set; }

public int BranchID { get; set; }

public SubAccountsClientSupplierContacts Clone()
{
return (SubAccountsClientSupplierContacts)MemberwiseClone();
}
}
public class SubAccountsClientSupplierContactsService : Interfaces.IScopedService
{

public string GetCode(Main main)
{
            DataTable dataTable = main.ExecuteQuery_DataTable("A_SubAccountsClientSupplierContacts_GetCode",true,null);
return dataTable.Rows[0][0].ToString();
}
public string GetCodeByBranchID(DateTime Date,string BranchID,Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@Date", Date);
parameters[1] = new SqlParameter("@BranchID",BranchID);
DataTable dataTable = main.ExecuteQuery_DataTable("A_SubAccountsClientSupplierContacts_GetCodeByBranchID",true,parameters);
return dataTable.Rows[0][0].ToString();
}

public int Insert_Update(SubAccountsClientSupplierContacts obj,int UserID,Main main)
{
SqlParameter[] parameters = new SqlParameter[14];
parameters[0] = new SqlParameter("@ClientSupplierContactID", obj.ClientSupplierContactID== null ? DBNull.Value : obj.ClientSupplierContactID);
parameters[1] = new SqlParameter("@ContactNo", obj.ContactNo== null ? DBNull.Value : obj.ContactNo);
parameters[2] = new SqlParameter("@SubAccountID", obj.SubAccountID== null ? DBNull.Value : obj.SubAccountID);
parameters[3] = new SqlParameter("@ContactName", obj.ContactName== null ? DBNull.Value : obj.ContactName);
parameters[4] = new SqlParameter("@PositionID", obj.PositionID== null ? DBNull.Value : obj.PositionID);
parameters[5] = new SqlParameter("@BirthDate", obj.BirthDate== null ? DBNull.Value : obj.BirthDate);
parameters[6] = new SqlParameter("@ContactAddress", obj.ContactAddress== null ? DBNull.Value : obj.ContactAddress);
parameters[7] = new SqlParameter("@ContactTel", obj.ContactTel== null ? DBNull.Value : obj.ContactTel);
parameters[8] = new SqlParameter("@ContactFax", obj.ContactFax== null ? DBNull.Value : obj.ContactFax);
parameters[9] = new SqlParameter("@ContactMobile", obj.ContactMobile== null ? DBNull.Value : obj.ContactMobile);
parameters[10] = new SqlParameter("@ContactEMail", obj.ContactEMail== null ? DBNull.Value : obj.ContactEMail);
parameters[11] = new SqlParameter("@Deleted", obj.Deleted== null ? false : obj.Deleted);
parameters[12] = new SqlParameter("@BranchID", obj.BranchID== null ? DBNull.Value : obj.BranchID);
parameters[13] = new SqlParameter("@UserID",UserID);
DataTable dataTable = main.ExecuteQuery_DataTable("A_SubAccountsClientSupplierContacts_Insert_Update",true,parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}
        public int Insert_UpdateByTableXML(List<SubAccountsClientSupplierContacts> lst, int SubAccountID, int BranchID, int UserID, Main main)
        {
            string lstXML = main.ToXml(lst);
            string strSql = string.Empty;
            strSql += " A_SubAccountsClientSupplierContacts_Insert_UpdateXML '";
            strSql += lstXML + "',";
            strSql += SubAccountID + ",";
            strSql += BranchID + ",";
            strSql += UserID ;
            
            DataTable dataTable =  main.ExecuteQuery_DataTable(strSql, false, null);
            return int.Parse(dataTable.Rows[0][0].ToString());
        }
        //public int Insert_UpdateByTable(List<SubAccountsClientSupplierContacts> lst, int UserID, Main main)
        //{
        //    string ClientSupplierContactID;
        //    string ContactNo;
        //    string SubAccountID;
        //    string ContactName;
        //    string PositionID;
        //    string BirthDate;
        //    string ContactAddress;
        //    string ContactTel;
        //    string ContactFax;
        //    string ContactMobile;
        //    string ContactEMail;
        //    string Deleted;
        //    string BranchID;
        //    string strSql = string.Empty;
        //    for (int i = 0; i < lst.Count; i++)
        //    {
        //        //Setting Parameters
        //        ClientSupplierContactID = lst[i].ClientSupplierContactID.ToString();
        //        ContactNo = lst[i].ContactNo.ToString();
        //        SubAccountID = lst[i].SubAccountID.ToString();
        //        ContactName = lst[i].ContactName.ToString();
        //        PositionID = lst[i].PositionID == null ? "" : lst[i].PositionID.ToString()  ;
        //        BirthDate = lst[i].BirthDate == null ? "" : lst[i].BirthDate.ToString();
        //        ContactAddress = lst[i].ContactAddress == null ? "" : lst[i].ContactAddress.ToString();
        //        ContactTel = lst[i].ContactTel == null ? "" : lst[i].ContactTel.ToString();
        //        ContactFax = lst[i].ContactFax == null ? "" : lst[i].ContactFax.ToString();
        //        ContactMobile = lst[i].ContactMobile == null ? "" : lst[i].ContactMobile.ToString();
        //        ContactEMail = lst[i].ContactEMail == null ? "" : lst[i].ContactEMail.ToString();
        //        Deleted = lst[i].Deleted.ToString();
        //        BranchID = lst[i].BranchID.ToString();
        //        strSql += "exec A_SubAccountsClientSupplierContacts_Insert_Update ";

        //        strSql += ClientSupplierContactID.Equals("") ? "NULL," : ClientSupplierContactID.Equals("True") ? "1," : ClientSupplierContactID.Equals("False") ? "0," : "'" + ClientSupplierContactID + "',";
        //        strSql += ContactNo.Equals("") ? "NULL," : ContactNo.Equals("True") ? "1," : ContactNo.Equals("False") ? "0," : "'" + ContactNo + "',";
        //        strSql += SubAccountID.Equals("") ? "NULL," : SubAccountID.Equals("True") ? "1," : SubAccountID.Equals("False") ? "0," : "'" + SubAccountID + "',";
        //        strSql += ContactName.Equals("") ? "NULL," : ContactName.Equals("True") ? "1," : ContactName.Equals("False") ? "0," : "'" + ContactName + "',";
        //        strSql += PositionID.Equals("") ? "NULL," : PositionID.Equals("True") ? "1," : PositionID.Equals("False") ? "0," : "'" + PositionID + "',";
        //        strSql += BirthDate.Equals("") ? "NULL," : BirthDate.Equals("True") ? "1," : BirthDate.Equals("False") ? "0," : "'" + DateTime.Parse(BirthDate).ToString("MM/dd/yyyy") + "',";
        //        strSql += ContactAddress.Equals("") ? "NULL," : ContactAddress.Equals("True") ? "1," : ContactAddress.Equals("False") ? "0," : "'" + ContactAddress + "',";
        //        strSql += ContactTel.Equals("") ? "NULL," : ContactTel.Equals("True") ? "1," : ContactTel.Equals("False") ? "0," : "'" + ContactTel + "',";
        //        strSql += ContactFax.Equals("") ? "NULL," : ContactFax.Equals("True") ? "1," : ContactFax.Equals("False") ? "0," : "'" + ContactFax + "',";
        //        strSql += ContactMobile.Equals("") ? "NULL," : ContactMobile.Equals("True") ? "1," : ContactMobile.Equals("False") ? "0," : "'" + ContactMobile + "',";
        //        strSql += ContactEMail.Equals("") ? "NULL," : ContactEMail.Equals("True") ? "1," : ContactEMail.Equals("False") ? "0," : "'" + ContactEMail + "',";
        //        strSql += Deleted.Equals("") ? "0," : Deleted.Equals("True") ? "1," : Deleted.Equals("False") ? "0," : "'" + Deleted + "',";
        //        strSql += BranchID.Equals("") ? "NULL," : BranchID.Equals("True") ? "1," : BranchID.Equals("False") ? "0," : "'" + BranchID + "',";
        //        strSql += UserID;
        //        strSql += ";";
        //    }
        //    DataTable dataTable =  main.ExecuteQuery_DataTable(strSql, false, null);
        //    return int.Parse(dataTable.Rows[0][0].ToString());
        //}

        public List<SubAccountsClientSupplierContacts> Select(int ClientSupplierContactID, string BranchIDs, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[3];
parameters[0] = new SqlParameter("@ClientSupplierContactID", ClientSupplierContactID);
parameters[1] = new SqlParameter("@BranchIDs",BranchIDs);
parameters[2] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable =main.ExecuteQuery_DataTable("A_SubAccountsClientSupplierContacts_Select",true,parameters);
List<SubAccountsClientSupplierContacts> lst = new List<SubAccountsClientSupplierContacts>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SubAccountsClientSupplierContacts>(dataTable);
}
return lst;
}

public void Delete(int ClientSupplierContactID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@ClientSupplierContactID", ClientSupplierContactID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_SubAccountsClientSupplierContacts_Delete",true,parameters);
}
public void DeleteVirtual(int ClientSupplierContactID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@ClientSupplierContactID", ClientSupplierContactID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_SubAccountsClientSupplierContacts_DeleteVirtual",true,parameters);
}


public List<SubAccountsClientSupplierContacts> SelectBySubAccountID(int SubAccountID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("A_SubAccountsClientSupplierContacts_SelectBySubAccountID",true,parameters);
List<SubAccountsClientSupplierContacts> lst = new List<SubAccountsClientSupplierContacts>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SubAccountsClientSupplierContacts>(dataTable);
}
return lst;
}

public void DeleteBySubAccountID(int SubAccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_SubAccountsClientSupplierContacts_DeleteBySubAccountID",true,parameters);
}
public void DeleteVirtualBySubAccountID(int SubAccountID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_SubAccountsClientSupplierContacts_DeleteVirtualBySubAccountID" ,true,parameters);
}

public List<SubAccountsClientSupplierContacts> SelectByBranchID(int BranchID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("A_SubAccountsClientSupplierContacts_SelectByBranchID",true,parameters);
List<SubAccountsClientSupplierContacts> lst = new List<SubAccountsClientSupplierContacts>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SubAccountsClientSupplierContacts>(dataTable);
}
return lst;
}

public void DeleteByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_SubAccountsClientSupplierContacts_DeleteByBranchID",true,parameters);
}
public void DeleteVirtualByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_SubAccountsClientSupplierContacts_DeleteVirtualByBranchID" ,true,parameters);
}

public List<SubAccountsClientSupplierContacts> SelectByPositionID(int PositionID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@PositionID", PositionID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("A_SubAccountsClientSupplierContacts_SelectByPositionID",true,parameters);
List<SubAccountsClientSupplierContacts> lst = new List<SubAccountsClientSupplierContacts>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SubAccountsClientSupplierContacts>(dataTable);
}
return lst;
}

public void DeleteByPositionID(int PositionID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@PositionID", PositionID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_SubAccountsClientSupplierContacts_DeleteByPositionID",true,parameters);
}
public void DeleteVirtualByPositionID(int PositionID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@PositionID", PositionID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_SubAccountsClientSupplierContacts_DeleteVirtualByPositionID" ,true,parameters);
}

}
}
