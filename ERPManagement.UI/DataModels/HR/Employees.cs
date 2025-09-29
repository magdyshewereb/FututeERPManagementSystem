using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataModels.Accounting;
using ERPManagement.UI.DataAccess;

namespace ERPManagement.UI.DataModels.HR
{
public class Employees
{

public int EmployeeID { get; set; }

public string EmployeeNo { get; set; }

public string? EmployeeOrder { get; set; }

public byte[]? EmployeeImage { get; set; }

public int SubAccountID { get; set; }

public bool  IsSalesMan { get; set; }

public int? SalesManDefaultStoreID { get; set; }

public decimal?  SalesCommissionRatio { get; set; }

public decimal?  CollectionCommissionRatio { get; set; }

public decimal?  PersonalIDNo { get; set; }

public DateTime? PersonalIDIssueDate { get; set; }

public DateTime? PersonalIDExpireDate { get; set; }

public DateTime? BirthDate { get; set; }

public int? GenderID { get; set; }

public int? CountryID { get; set; }

public int? CityID { get; set; }

public int? AreaID { get; set; }

public string? Address { get; set; }

public int? NationalityID { get; set; }

public int? MilitaryServiceID { get; set; }

public DateTime? PostponedToDate { get; set; }

public int? SocialStatusID { get; set; }

public int? QualificationNameID { get; set; }

public int? UniversityID { get; set; }

public int? SectionNameID { get; set; }

public int? QualificationYear { get; set; }

public string? SectionNameNotes { get; set; }

public string? EMail { get; set; }

public int? StateID { get; set; }

public int? AdministrativeStructureID { get; set; }

public int? AdministrativeLevelID { get; set; }

public int? DegreeID { get; set; }

public int? PositionID { get; set; }

public int? DirectManagerID { get; set; }

public DateTime? HireDate { get; set; }

public DateTime? EmployeeEndDate { get; set; }

public int? LeavingWorkReasonID { get; set; }

public string? LeavingWorkNotes { get; set; }

public DateTime? ContractFromDate { get; set; }

public DateTime? ContractToDate { get; set; }

public string? Notes { get; set; }

public bool  HasPassport { get; set; }

public string? PassportNo { get; set; }

public DateTime? PassportExpireDate { get; set; }

public int? ApplicantID { get; set; }

public int? DrivingLicenseID { get; set; }

public DateTime? DrivingLicenseExpireDate { get; set; }

public int? BankID { get; set; }

public string? BankAccountNo { get; set; }

public DateTime? FirstInssuranceDate { get; set; }

public DateTime? InssuranceEndDate { get; set; }

public string? SocialInssuranceNo { get; set; }

public decimal?  SocialInssuranceValue { get; set; }

public decimal?  InclusiveHealthInsuranceValue { get; set; }

public int? SocialInssuranceOfficeID { get; set; }

public DateTime? InssuranceDate { get; set; }

public bool  HasPrivateCar { get; set; }

public string? WorkOfficePermissionCode { get; set; }

public DateTime? WorkOfficeFromDate { get; set; }

public DateTime? WorkOfficeToDate { get; set; }

public DateTime? WorkOfficeSendDate { get; set; }

public bool  WorkOfficeIsSend { get; set; }

public decimal?  CustomVacationBalance { get; set; }

public decimal?  OccasionVacationBalance { get; set; }

public decimal?  CustomVacationBalanceLastYear { get; set; }

public decimal?  OccasionVacationBalanceLastYear { get; set; }

public bool  IsMonthlySalary { get; set; }

public bool  InSalaryTax { get; set; }

public bool  InServicePercent { get; set; }

public DateTime? ServicePercentFromDate { get; set; }

public decimal?  StartBasicSalary { get; set; }

public decimal?  StartVariantSalary { get; set; }

public decimal?  StartSalary { get; set; }

public decimal?  CurrentBasicSalary { get; set; }

public decimal?  CurrentVariantSalary { get; set; }

public decimal?  NotInsuranceVariantSalary { get; set; }

public decimal?  NetCurrentSalary { get; set; }

public bool  IsFixedSalaryTax { get; set; }

public decimal?  FixedSalaryTaxValue { get; set; }

public bool  IsFixedTaxPercentage { get; set; }

public bool  IsSalaryAtEndOfMonth { get; set; }

public int? HealthInsuranceTypeID { get; set; }

public DateTime? HealthInsuranceStartDate { get; set; }

public DateTime? HealthInsuranceEndDate { get; set; }

public int? ExtraTimeRuleID { get; set; }

public int? DelayRuleID { get; set; }

public int? AbsentRuleID { get; set; }

public int? PenaltyRuleID { get; set; }

public int? VacationRuleID { get; set; }

public int? LeavePermissionRuleID { get; set; }

public int? FeedingRuleID { get; set; }

public int? SalaryListID { get; set; }

public int? CurrencyID { get; set; }

public int? SalaryAccountID { get; set; }

public int? AdvanceAccountID { get; set; }

public int? PenaltyAccountID { get; set; }

public bool  Deleted { get; set; }

public int? BranchID { get; set; }

public Employees Clone()
{
return (Employees)MemberwiseClone();
}
}
public class EmployeesService : Interfaces.IScopedService
{

public string GetCode(Main main)
{
            DataTable dataTable = main.ExecuteQuery_DataTable("HR_Employees_GetCode",true,null);
return dataTable.Rows[0][0].ToString();
}
public string GetCodeByBranchID(DateTime Date,string BranchID,Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@Date", Date);
parameters[1] = new SqlParameter("@BranchID",BranchID);
DataTable dataTable = main.ExecuteQuery_DataTable("HR_Employees_GetCodeByBranchID",true,parameters);
return dataTable.Rows[0][0].ToString();
}

public int Insert_Update(Employees obj,int UserID,Main main)
{
SqlParameter[] parameters = new SqlParameter[97];
parameters[0] = new SqlParameter("@EmployeeNo", obj.EmployeeNo == null ? DBNull.Value : obj.EmployeeNo);
parameters[1] = new SqlParameter("@EmployeeOrder", obj.EmployeeOrder == null ? DBNull.Value : obj.EmployeeOrder);
parameters[2] = new SqlParameter("@SubAccountID", obj.SubAccountID == null ? DBNull.Value : obj.SubAccountID);
parameters[3] = new SqlParameter("@IsSalesMan", obj.IsSalesMan == null ? false : obj.IsSalesMan);
parameters[4] = new SqlParameter("@SalesManDefaultStoreID", obj.SalesManDefaultStoreID == null ? DBNull.Value : obj.SalesManDefaultStoreID);
parameters[5] = new SqlParameter("@SalesCommissionRatio", obj.SalesCommissionRatio == null ? DBNull.Value : obj.SalesCommissionRatio);
parameters[6] = new SqlParameter("@CollectionCommissionRatio", obj.CollectionCommissionRatio == null ? DBNull.Value : obj.CollectionCommissionRatio);
parameters[7] = new SqlParameter("@PersonalIDNo", obj.PersonalIDNo == null ? DBNull.Value : obj.PersonalIDNo);
parameters[8]  = new SqlParameter("@PersonalIDIssueDate", obj.PersonalIDIssueDate == null ? DBNull.Value : obj.PersonalIDIssueDate);
parameters[9]  = new SqlParameter("@PersonalIDExpireDate", obj.PersonalIDExpireDate == null ? DBNull.Value : obj.PersonalIDExpireDate);
parameters[10] = new SqlParameter("@BirthDate", obj.BirthDate == null ? DBNull.Value : obj.BirthDate);
parameters[11] = new SqlParameter("@GenderID", obj.GenderID == null ? DBNull.Value : obj.GenderID);
parameters[12] = new SqlParameter("@CountryID", obj.CountryID == null ? DBNull.Value : obj.CountryID);
parameters[13] = new SqlParameter("@CityID", obj.CityID == null ? DBNull.Value : obj.CityID);
parameters[14] = new SqlParameter("@AreaID", obj.AreaID == null ? DBNull.Value : obj.AreaID);
parameters[15] = new SqlParameter("@Address", obj.Address == null ? DBNull.Value : obj.Address);
parameters[16] = new SqlParameter("@NationalityID", obj.NationalityID == null ? DBNull.Value : obj.NationalityID);
parameters[17] = new SqlParameter("@MilitaryServiceID", obj.MilitaryServiceID == null ? DBNull.Value : obj.MilitaryServiceID);
parameters[18] = new SqlParameter("@PostponedToDate", obj.PostponedToDate == null ? DBNull.Value : obj.PostponedToDate);
parameters[19] = new SqlParameter("@SocialStatusID", obj.SocialStatusID == null ? DBNull.Value : obj.SocialStatusID);
parameters[20] = new SqlParameter("@QualificationNameID", obj.QualificationNameID == null ? DBNull.Value : obj.QualificationNameID);
parameters[21] = new SqlParameter("@UniversityID", obj.UniversityID == null ? DBNull.Value : obj.UniversityID);
parameters[22] = new SqlParameter("@SectionNameID", obj.SectionNameID == null ? DBNull.Value : obj.SectionNameID);
parameters[23] = new SqlParameter("@QualificationYear", obj.QualificationYear == null ? DBNull.Value : obj.QualificationYear);
parameters[24] = new SqlParameter("@SectionNameNotes", obj.SectionNameNotes == null ? DBNull.Value : obj.SectionNameNotes);
parameters[25] = new SqlParameter("@EMail", obj.EMail == null ? DBNull.Value : obj.EMail);
parameters[26] = new SqlParameter("@StateID", obj.StateID == null ? DBNull.Value : obj.StateID);
parameters[27] = new SqlParameter("@AdministrativeStructureID", obj.AdministrativeStructureID == null ? DBNull.Value : obj.AdministrativeStructureID);
parameters[28] = new SqlParameter("@AdministrativeLevelID", obj.AdministrativeLevelID == null ? DBNull.Value : obj.AdministrativeLevelID);
parameters[29] = new SqlParameter("@DegreeID", obj.DegreeID == null ? DBNull.Value : obj.DegreeID);
parameters[30] = new SqlParameter("@PositionID", obj.PositionID == null ? DBNull.Value : obj.PositionID);
parameters[31] = new SqlParameter("@DirectManagerID", obj.DirectManagerID == null ? DBNull.Value : obj.DirectManagerID);
parameters[32] = new SqlParameter("@HireDate", obj.HireDate == null ? DBNull.Value : obj.HireDate);
parameters[33] = new SqlParameter("@EmployeeEndDate", obj.EmployeeEndDate == null ? DBNull.Value : obj.EmployeeEndDate);
parameters[34] = new SqlParameter("@LeavingWorkReasonID", obj.LeavingWorkReasonID == null ? DBNull.Value : obj.LeavingWorkReasonID);
parameters[35] = new SqlParameter("@LeavingWorkNotes", obj.LeavingWorkNotes == null ? DBNull.Value : obj.LeavingWorkNotes);
parameters[36] = new SqlParameter("@ContractFromDate", obj.ContractFromDate == null ? DBNull.Value : obj.ContractFromDate);
parameters[37] = new SqlParameter("@ContractToDate", obj.ContractToDate == null ? DBNull.Value : obj.ContractToDate);
parameters[38] = new SqlParameter("@Notes", obj.Notes == null ? DBNull.Value : obj.Notes);
parameters[39] = new SqlParameter("@HasPassport", obj.HasPassport == null ? DBNull.Value : obj.HasPassport);
parameters[40] = new SqlParameter("@PassportNo", obj.PassportNo == null ? DBNull.Value : obj.PassportNo);
parameters[41] = new SqlParameter("@PassportExpireDate", obj.PassportExpireDate == null ? DBNull.Value : obj.PassportExpireDate);
parameters[42] = new SqlParameter("@ApplicantID", obj.ApplicantID == null ? DBNull.Value : obj.ApplicantID);
parameters[43] = new SqlParameter("@DrivingLicenseID", obj.DrivingLicenseID == null ? DBNull.Value : obj.DrivingLicenseID);
parameters[44] = new SqlParameter("@DrivingLicenseExpireDate", obj.DrivingLicenseExpireDate == null ? DBNull.Value : obj.DrivingLicenseExpireDate);
parameters[45] = new SqlParameter("@BankID", obj.BankID == null ? DBNull.Value : obj.BankID);
parameters[46] = new SqlParameter("@BankAccountNo", obj.BankAccountNo == null ? DBNull.Value : obj.BankAccountNo);
parameters[47] = new SqlParameter("@FirstInssuranceDate", obj.FirstInssuranceDate == null ? DBNull.Value : obj.FirstInssuranceDate);
parameters[48] = new SqlParameter("@InssuranceEndDate", obj.InssuranceEndDate == null ? DBNull.Value : obj.InssuranceEndDate);
parameters[49] = new SqlParameter("@SocialInssuranceNo", obj.SocialInssuranceNo == null ? DBNull.Value : obj.SocialInssuranceNo);
parameters[50] = new SqlParameter("@SocialInssuranceValue", obj.SocialInssuranceValue == null ? DBNull.Value : obj.SocialInssuranceValue);
parameters[51] = new SqlParameter("@InclusiveHealthInsuranceValue", obj.InclusiveHealthInsuranceValue == null ? DBNull.Value : obj.InclusiveHealthInsuranceValue);
parameters[52] = new SqlParameter("@SocialInssuranceOfficeID", obj.SocialInssuranceOfficeID == null ? DBNull.Value : obj.SocialInssuranceOfficeID);
parameters[53] = new SqlParameter("@InssuranceDate", obj.InssuranceDate == null ? DBNull.Value : obj.InssuranceDate);
parameters[54] = new SqlParameter("@HasPrivateCar", obj.HasPrivateCar == null ? 0 : obj.HasPrivateCar);
parameters[55] = new SqlParameter("@WorkOfficePermissionCode", obj.WorkOfficePermissionCode == null ? DBNull.Value : obj.WorkOfficePermissionCode);
parameters[56] = new SqlParameter("@WorkOfficeFromDate", obj.WorkOfficeFromDate == null ? DBNull.Value : obj.WorkOfficeFromDate);
parameters[57] = new SqlParameter("@WorkOfficeToDate", obj.WorkOfficeToDate == null ? DBNull.Value : obj.WorkOfficeToDate);
parameters[58] = new SqlParameter("@WorkOfficeSendDate", obj.WorkOfficeSendDate == null ? DBNull.Value : obj.WorkOfficeSendDate);
parameters[59] = new SqlParameter("@WorkOfficeIsSend", obj.WorkOfficeIsSend == null ? DBNull.Value : obj.WorkOfficeIsSend);
parameters[60] = new SqlParameter("@CustomVacationBalance", obj.CustomVacationBalance == null ? DBNull.Value : obj.CustomVacationBalance);
parameters[61] = new SqlParameter("@OccasionVacationBalance", obj.OccasionVacationBalance == null ? DBNull.Value : obj.OccasionVacationBalance);
parameters[62] = new SqlParameter("@CustomVacationBalanceLastYear", obj.CustomVacationBalanceLastYear == null ? DBNull.Value : obj.CustomVacationBalanceLastYear);
parameters[63] = new SqlParameter("@OccasionVacationBalanceLastYear", obj.OccasionVacationBalanceLastYear == null ? DBNull.Value : obj.OccasionVacationBalanceLastYear);
parameters[64] = new SqlParameter("@IsMonthlySalary", obj.IsMonthlySalary == null ? false : obj.IsMonthlySalary);
parameters[65] = new SqlParameter("@InSalaryTax", obj.InSalaryTax == null ? false : obj.InSalaryTax);
parameters[66] = new SqlParameter("@InServicePercent", obj.InServicePercent == null ? false : obj.InServicePercent);
parameters[67] = new SqlParameter("@ServicePercentFromDate", obj.ServicePercentFromDate == null ? DBNull.Value : obj.ServicePercentFromDate);
parameters[68] = new SqlParameter("@StartBasicSalary", obj.StartBasicSalary == null ? DBNull.Value : obj.StartBasicSalary);
parameters[69] = new SqlParameter("@StartVariantSalary", obj.StartVariantSalary == null ? DBNull.Value : obj.StartVariantSalary);
parameters[70] = new SqlParameter("@StartSalary", obj.StartSalary == null ? DBNull.Value : obj.StartSalary);
parameters[71] = new SqlParameter("@CurrentBasicSalary", obj.CurrentBasicSalary == null ? DBNull.Value : obj.CurrentBasicSalary);
parameters[72] = new SqlParameter("@CurrentVariantSalary", obj.CurrentVariantSalary == null ? DBNull.Value : obj.CurrentVariantSalary);
parameters[73] = new SqlParameter("@NotInsuranceVariantSalary", obj.NotInsuranceVariantSalary == null ? DBNull.Value : obj.NotInsuranceVariantSalary);
parameters[74] = new SqlParameter("@NetCurrentSalary", obj.NetCurrentSalary == null ? DBNull.Value : obj.NetCurrentSalary);
parameters[75] = new SqlParameter("@IsFixedSalaryTax", obj.IsFixedSalaryTax == null ? false : obj.IsFixedSalaryTax);
parameters[76] = new SqlParameter("@FixedSalaryTaxValue", obj.FixedSalaryTaxValue == null ? DBNull.Value : obj.FixedSalaryTaxValue);
parameters[77] = new SqlParameter("@IsFixedTaxPercentage", obj.IsFixedTaxPercentage == null ? DBNull.Value : obj.IsFixedTaxPercentage);
parameters[78] = new SqlParameter("@IsSalaryAtEndOfMonth", obj.IsSalaryAtEndOfMonth == null ? DBNull.Value : obj.IsSalaryAtEndOfMonth);
parameters[79] = new SqlParameter("@HealthInsuranceTypeID", obj.HealthInsuranceTypeID == null ? DBNull.Value : obj.HealthInsuranceTypeID);
parameters[80] = new SqlParameter("@HealthInsuranceStartDate", obj.HealthInsuranceStartDate == null ? DBNull.Value : obj.HealthInsuranceStartDate);
parameters[81] = new SqlParameter("@HealthInsuranceEndDate", obj.HealthInsuranceEndDate == null ? DBNull.Value : obj.HealthInsuranceEndDate);
parameters[82] = new SqlParameter("@ExtraTimeRuleID", obj.ExtraTimeRuleID == null ? DBNull.Value : obj.ExtraTimeRuleID);
parameters[83] = new SqlParameter("@DelayRuleID", obj.DelayRuleID == null ? DBNull.Value : obj.DelayRuleID);
parameters[84] = new SqlParameter("@AbsentRuleID", obj.AbsentRuleID == null ? DBNull.Value : obj.AbsentRuleID);
parameters[85] = new SqlParameter("@PenaltyRuleID", obj.PenaltyRuleID == null ? DBNull.Value : obj.PenaltyRuleID);
parameters[86] = new SqlParameter("@VacationRuleID", obj.VacationRuleID == null ? DBNull.Value : obj.VacationRuleID);
parameters[87] = new SqlParameter("@LeavePermissionRuleID", obj.LeavePermissionRuleID == null ? DBNull.Value : obj.LeavePermissionRuleID);
parameters[88] = new SqlParameter("@FeedingRuleID", obj.FeedingRuleID == null ? DBNull.Value : obj.FeedingRuleID);
parameters[89] = new SqlParameter("@SalaryListID", obj.SalaryListID == null ? DBNull.Value : obj.SalaryListID);
parameters[90] = new SqlParameter("@CurrencyID", obj.CurrencyID == null ? DBNull.Value : obj.CurrencyID);
parameters[91] = new SqlParameter("@SalaryAccountID", obj.SalaryAccountID == null ? DBNull.Value : obj.SalaryAccountID);
parameters[92] = new SqlParameter("@AdvanceAccountID", obj.AdvanceAccountID == null ? DBNull.Value : obj.AdvanceAccountID);
parameters[93] = new SqlParameter("@PenaltyAccountID", obj.PenaltyAccountID == null ? DBNull.Value : obj.PenaltyAccountID);
parameters[94] = new SqlParameter("@Deleted", obj.Deleted == null ? false : obj.Deleted);
parameters[95] = new SqlParameter("@BranchID", obj.BranchID == null ? DBNull.Value : obj.BranchID);
parameters[96] = new SqlParameter("@UserID", UserID == null ? DBNull.Value : UserID);
            
DataTable dataTable = main.ExecuteQuery_DataTable("HR_Employees_Insert_Update",true,parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}
        public int Insert_Update2(Employees obj, int UserID, Main main)

        {
            SqlParameter[] parameters = new SqlParameter[6];
            parameters[0] = new SqlParameter("@EmployeeNo", obj.EmployeeNo == null ? DBNull.Value : obj.EmployeeNo);
            parameters[1] = new SqlParameter("@BranchID", obj.BranchID == null ? DBNull.Value : obj.BranchID);
            parameters[2] = new SqlParameter("@SubAccountID", obj.SubAccountID == null ? DBNull.Value : obj.SubAccountID);
            parameters[3] = new SqlParameter("@IsSalesMan", obj.IsSalesMan == null ? false : obj.IsSalesMan);
            parameters[4] = new SqlParameter("@SalesManDefaultStoreID", obj.SalesManDefaultStoreID == null ? DBNull.Value : obj.SalesManDefaultStoreID);
            parameters[5] = new SqlParameter("@UserID", UserID);
            DataTable dataTable = main.ExecuteQuery_DataTable("HR_Employees_Insert_Update2", true, parameters);
            return int.Parse(dataTable.Rows[0][0].ToString());
        }
        public List<Employees> Select(int EmployeeID, string BranchIDs, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[3];
parameters[0] = new SqlParameter("@EmployeeID", EmployeeID);
parameters[1] = new SqlParameter("@BranchIDs",BranchIDs);
parameters[2] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable =main.ExecuteQuery_DataTable("HR_Employees_Select",true,parameters);
List<Employees> lst = new List<Employees>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<Employees>(dataTable);
}
return lst;
}
        public List<Employees> SelectBySubAccountID(int SubAccountID,string BranchIDs, int IsArabic, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@SubAccountID", SubAccountID);
            parameters[1] = new SqlParameter("@BranchIDs", BranchIDs);
            parameters[2] = new SqlParameter("@IsArabic", IsArabic);
            DataTable dataTable = main.ExecuteQuery_DataTable("HR_Employees_SelectBySubAccountID", true, parameters);
            List<Employees> lst = new List<Employees>();
            if (dataTable != null)
            {
                lst = main.CreateListFromTable<Employees>(dataTable);
            }
            return lst;
        }
        public void Delete(int EmployeeID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@EmployeeID", EmployeeID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("HR_Employees_Delete",true,parameters);
}
public void DeleteVirtual(int EmployeeID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@EmployeeID", EmployeeID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("HR_Employees_DeleteVirtual",true,parameters);
}








































































}
}
