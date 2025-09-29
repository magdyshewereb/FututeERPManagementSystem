using System.Data;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using ERPManagement.UI.DataModels.Interfaces;
using ERPManagement.UI.DataAccess;

namespace ERPManagement.UI.DataModels.General
{
    public class Branch
    {

        public int BranchID { get; set; }

        public string BranchCode { get; set; }

        public string BranchNameAr { get; set; }

        public string? BranchNameEn { get; set; }

        public int? BranchAccountID { get; set; }

        public int? BranchSubAccountID { get; set; }

        public bool? IsMainBranch { get; set; }

        public string? CommercialRegistrationNo { get; set; }

        public string? TaxNo { get; set; }

        public string? TaxFileNo { get; set; }

        public string? TaxOfficeName { get; set; }

        public string? LicenseNo { get; set; }

        public int? CityID { get; set; }

        public int? AreaID { get; set; }

        public string? BuildingNumber { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }

        public string? Fax { get; set; }

        public string? EMail { get; set; }

        public string? EINVBranchCode { get; set; }

        public int? EINVTaxActivityTypeID { get; set; }

        public bool Deleted { get; set; }

    }
    public class BranchesService :IScopedService
    {

  
        public List<Branch> Select(int BranchID, string BranchIDs, int IsArabic, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@BranchID", BranchID);
            parameters[1] = new SqlParameter("@BranchIDs", BranchIDs);
            parameters[2] = new SqlParameter("@IsArabic", IsArabic);
            DataTable dataTable =  main.ExecuteQuery_DataTable("G_Branches_Select  ", true, parameters);
            List<Branch> lst = new List<Branch>();
            if (dataTable != null)
            {
                lst = main.CreateListFromTable<Branch>(dataTable);
            }
            return lst;
        }
        public DataTable GetCompanyData(string TableName, string Columns, string Condition, Main main)
        {
            //MessageBox.Show(strQuery);
            DataTable dataTable = new DataTable();
            string strQuery = "Exec FillCombo '" + TableName + "','" + Columns.Replace("'", "''") + "'";
            strQuery += Condition.Length > 0 ? ",'" + Condition + "'" : "";
            try
            {
                dataTable = main.ExecuteQuery_DataTable(strQuery,false, null);
            }
            // Catch SqlExceptions and generate the appropraiate error message
            catch 
            {
                
            }
            
            return dataTable;
        }




    }
}
