using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using ERPManagement.UI.GeneralClasses;
using ERPManagement.UI.DataAccess;

namespace ERPManagement.UI.DataModels.Accounting
{
    public class SubAccountsClientSupplier
{

public int ClientSupplierID { get; set; }

public string ClientSupplierNo { get; set; }

public int? TitleID { get; set; }

public int SubAccountID { get; set; }

[Custom(IsDateonly = true)]
public DateTime?  BirthDate  { get ; set; }

public int? CityID { get; set; }

public int? AreaID { get; set; }

public string? BuildingNumber { get; set; }

public string? Address { get; set; }

public string? Tel { get; set; }

public string? Fax { get; set; }

public string? Mobile { get; set; }

public string? EMail { get; set; }

public int? LineID { get; set; }

public decimal?  CreditLimit { get; set; }

public decimal?  DiscountPercentage { get; set; }

public decimal?  DiscountPercentageAfterTax { get; set; }

public decimal?  BlanksProductionDiscountPercentage { get; set; }

public int? DefaultPaymentMethodID { get; set; }

public int? DefaultClientAccountID { get; set; }

public int? DefaultSupplierAccountID { get; set; }

public int? PriceTypeID { get; set; }

public int? SubAccountClassificationID { get; set; }

public string? CompanyName { get; set; }

public string? CommercialRegistrationNo { get; set; }

public int? EINVSubAccountTypeID { get; set; }

public string? TaxNo { get; set; }

public string? LicenseNo { get; set; }

public int? EmployeeID { get; set; }

public bool  IsDiscountTax { get; set; }

public bool  IsAddedTax { get; set; }

public bool  IsActive { get; set; }

public DateTime? StopDate { get; set; }

public string? Notes { get; set; }

public bool  Deleted { get; set; }

public int? BranchID { get; set; }

public SubAccountsClientSupplier Clone()
{
return (SubAccountsClientSupplier)MemberwiseClone();
}
}
public class SubAccountsClientSupplierService : Interfaces.IScopedService
{


public int Insert_Update(SubAccountsClientSupplier obj,int UserID,Main main)
{
SqlParameter[] parameters = new SqlParameter[37];
parameters[0] = new SqlParameter("@ClientSupplierID", obj.ClientSupplierID);
parameters[1] = new SqlParameter("@ClientSupplierNo", obj.ClientSupplierNo == null ? DBNull.Value : obj.ClientSupplierNo);
parameters[2] = new SqlParameter("@TitleID", obj.TitleID == null ? DBNull.Value : obj.TitleID);
parameters[3] = new SqlParameter("@SubAccountID", obj.SubAccountID == null ? DBNull.Value : obj.SubAccountID);
parameters[4] = new SqlParameter("@BirthDate", obj.BirthDate == null ? DBNull.Value : obj.BirthDate);
parameters[5] = new SqlParameter("@CityID", obj.CityID == null ? DBNull.Value :obj.CityID );
parameters[6] = new SqlParameter("@AreaID", obj.AreaID == null ? DBNull.Value :obj.AreaID);
parameters[7] = new SqlParameter("@BuildingNumber", obj.BuildingNumber == null ? DBNull.Value : obj.BuildingNumber);
parameters[8] = new SqlParameter("@Address", obj.Address == null ? DBNull.Value : obj.Address);
parameters[9] = new SqlParameter("@Tel", obj.Tel == null ? DBNull.Value : obj.Tel);
parameters[10] = new SqlParameter("@Fax", obj.Fax == null ? DBNull.Value : obj.Fax);
parameters[11] = new SqlParameter("@Mobile", obj.Mobile == null ? DBNull.Value : obj.Mobile);
parameters[12] = new SqlParameter("@EMail", obj.EMail == null ? DBNull.Value : obj.EMail);
parameters[13] = new SqlParameter("@LineID", obj.LineID == null ? DBNull.Value : obj.LineID);
parameters[14] = new SqlParameter("@CreditLimit", obj.CreditLimit == null ? DBNull.Value : obj.CreditLimit);
parameters[15] = new SqlParameter("@DiscountPercentage", obj.DiscountPercentage == null ? DBNull.Value :obj.DiscountPercentage);
parameters[16] = new SqlParameter("@DiscountPercentageAfterTax", obj.DiscountPercentageAfterTax == null ? DBNull.Value : obj.DiscountPercentageAfterTax);
parameters[17] = new SqlParameter("@BlanksProductionDiscountPercentage", obj.BlanksProductionDiscountPercentage == null ? DBNull.Value : obj.BlanksProductionDiscountPercentage);
parameters[18] = new SqlParameter("@DefaultPaymentMethodID", obj.DefaultPaymentMethodID == null ? DBNull.Value : obj.DefaultPaymentMethodID);
parameters[19] = new SqlParameter("@DefaultClientAccountID", obj.DefaultClientAccountID == null ? DBNull.Value : obj.DefaultClientAccountID);
parameters[20] = new SqlParameter("@DefaultSupplierAccountID", obj.DefaultSupplierAccountID == null ? DBNull.Value : obj.DefaultSupplierAccountID);
parameters[21] = new SqlParameter("@PriceTypeID", obj.PriceTypeID == null ? DBNull.Value : obj.PriceTypeID);
parameters[22] = new SqlParameter("@SubAccountClassificationID", obj.SubAccountClassificationID == null ? DBNull.Value : obj.SubAccountClassificationID);
parameters[23] = new SqlParameter("@CompanyName", obj.CompanyName == null ? DBNull.Value : obj.CompanyName);
parameters[24] = new SqlParameter("@CommercialRegistrationNo", obj.CommercialRegistrationNo == null ? DBNull.Value : obj.CommercialRegistrationNo);
parameters[25] = new SqlParameter("@EINVSubAccountTypeID", obj.EINVSubAccountTypeID == null ? DBNull.Value :obj.EINVSubAccountTypeID);
parameters[26] = new SqlParameter("@TaxNo", obj.TaxNo == null ? DBNull.Value : obj.TaxNo);
parameters[27] = new SqlParameter("@LicenseNo", obj.LicenseNo == null ? DBNull.Value : obj.LicenseNo);
parameters[28] = new SqlParameter("@EmployeeID", obj.EmployeeID == null ? DBNull.Value : obj.EmployeeID);
parameters[29] = new SqlParameter("@IsDiscountTax", obj.IsDiscountTax == null ? false : obj.IsDiscountTax);
parameters[30] = new SqlParameter("@IsAddedTax", obj.IsAddedTax == null ? DBNull.Value : obj.IsAddedTax);
parameters[31] = new SqlParameter("@IsActive", obj.IsActive == null ? false : obj.IsActive);
parameters[32] = new SqlParameter("@StopDate", obj.StopDate == null ? DBNull.Value : obj.StopDate);
parameters[33] = new SqlParameter("@Notes", obj.Notes == null ? DBNull.Value : obj.Notes);
parameters[34] = new SqlParameter("@Deleted", obj.Deleted == null ? false : obj.Deleted);
parameters[35] = new SqlParameter("@BranchID", obj.BranchID == null ? DBNull.Value : obj.BranchID);
parameters[36] = new SqlParameter("@UserID",UserID);
DataTable dataTable = main.ExecuteQuery_DataTable("A_SubAccountsClientSupplier_Insert_Update",true,parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}
        public string GetCode( Main main)
        {
            string strSql = string.Empty;
            strSql += "A_SubAccountsClientSupplier_GetCode";
            DataTable dataTable = main.SyncExecuteQuery_DataTable(strSql,false,null);
            return dataTable.Rows[0][0].ToString();
        }
        public List<SubAccountsClientSupplier> SelectBySubAccountID(int SubAccountID, int IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable =main.ExecuteQuery_DataTable("A_SubAccountsClientSupplier_SelectBySubAccountID", true,parameters);
List<SubAccountsClientSupplier> lst = new List<SubAccountsClientSupplier>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<SubAccountsClientSupplier>(dataTable);
}
return lst;
}

        public void DeleteBySubAccountID(int SubAccountID, int UserID, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
            parameters[1] = new SqlParameter("@UserID", UserID);
            main.ExecuteNonQuery("A_SubAccountsClientSupplier_DeleteBySubAccountID", true, parameters);
        }
        


























}
}
