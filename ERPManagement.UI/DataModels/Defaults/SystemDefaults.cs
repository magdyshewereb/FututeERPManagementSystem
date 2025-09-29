using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.Defaults
{
    public class SystemDefaults
    {

        public int DefaultID { get; set; }

        public string DefaultEnName { get; set; }

        public string? DefaultArName { get; set; }

        public string? DefaultValue { get; set; }

    }
    public class SystemDefaultsService : Interfaces.IScopedService
    {

        public List<SystemDefaults> Select(int DefaultID, string BranchIDs, bool IsArabic, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@DefaultID", DefaultID);
            parameters[1] = new SqlParameter("@BranchIDs", BranchIDs);
            parameters[0] = new SqlParameter("@IsArabic", IsArabic);
            DataTable dataTable = main.ExecuteQuery_DataTable(" SystemDefaults_Select  ", true, parameters);
            List<SystemDefaults> lst = new List<SystemDefaults>();
            if (dataTable != null)
            {
                lst = main.CreateListFromTable<SystemDefaults>(dataTable);
            }
            return lst;
        }
    }
}
