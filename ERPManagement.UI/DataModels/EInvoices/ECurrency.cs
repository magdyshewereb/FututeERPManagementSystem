using ERPManagement.UI.DataAccess;
using ERPManagement.UI.DataModels.Interfaces;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ERPManagement.UI.DataModels.EInvoices
{
    public class EINV_Currency
    {
        public int EINVCurrencyID { get; set; }
        public string EINVCurrencyCode { get; set; }
        public string EINVCurrencyNameAr { get; set; }
        public string EINVCurrencyNameEn { get; set; }
        public string EINVCurrencyName { get; set; }
        public bool Deleted { get; set; }
        public int BranchID { get; set; }

    }
    public class CurrencyService : IScopedService
    {
        public void Delete(int ID, int UserID, Main main, bool IsFromServer)
        {
            throw new NotImplementedException();
        }

        public void DeleteByBranchID(int BranchID, int UserID, Main main, bool IsFromServer)
        {
            throw new NotImplementedException();
        }

        public List<EINV_Currency> FillCombo(int ID, bool IsArabic, Main main, bool IsFromServer)
        {
            SqlParameter[] parameters = new SqlParameter[2];

            parameters[0] = new SqlParameter("@EINVCurrencyID", ID);
            parameters[1] = new SqlParameter("@IsArabic", IsArabic);

            DataTable dataTable = IsFromServer ? main.SyncExecuteQuery_DataTable("EINV_Currency_FillCombo", true, parameters)
                : main.ExecuteQuery_DataTable("EINV_Currency_FillCombo", true, parameters);
            return main.CreateListFromTable<EINV_Currency>(dataTable);
        }

        internal List<EINV_Currency> FillCombo(int v1, bool v2, Task<Main> task, bool v3)
        {
            throw new NotImplementedException();
        }
    }
}
