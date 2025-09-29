using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataAccess;

namespace ERPManagement.UI.DataModels.Defaults
{
    public class SystemOptions
    {

        public int OptionID { get; set; }

        public string? OptionEnName { get; set; }

        public string? OptionArName { get; set; }

        public bool? OptionValue { get; set; }

        public bool? ReadOnly { get; set; }

        public string? Description { get; set; }

    }
    public class SystemOptionsService : Interfaces.IScopedService
    {

        public int Insert_Update(SystemOptions obj, int UserID, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[7];
            parameters[0] = new SqlParameter("@OptionID", obj.OptionID);
            parameters[1] = new SqlParameter("@OptionEnName", obj.OptionEnName);
            parameters[2] = new SqlParameter("@OptionArName", obj.OptionArName);
            parameters[3] = new SqlParameter("@OptionValue", obj.OptionValue);
            parameters[4] = new SqlParameter("@ReadOnly", obj.ReadOnly);
            parameters[5] = new SqlParameter("@Description", obj.Description);
            parameters[7] = new SqlParameter("@UserID", UserID);
            DataTable dataTable = main.ExecuteQuery_DataTable(" SystemOptions_Insert_Update  ", true, parameters);
            return int.Parse(dataTable.Rows[0][0].ToString());
        }

        public List<SystemOptions> Select(int OptionID, string BranchIDs, bool IsArabic, Main main, bool IsFromServer)
        {
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@OptionID", OptionID);
            parameters[1] = new SqlParameter("@BranchIDs", BranchIDs);
            parameters[0] = new SqlParameter("@IsArabic", IsArabic);
            DataTable dataTable = IsFromServer ? main.SyncExecuteQuery_DataTable(" SystemOptions_Select  ", true, parameters) : main.ExecuteQuery_DataTable(" SystemOptions_Select  ", true, parameters);
            List<SystemOptions> lst = new List<SystemOptions>();
            if (dataTable != null)
            {
                lst = main.CreateListFromTable<SystemOptions>(dataTable);
            }
            return lst;
        }

    }
}
