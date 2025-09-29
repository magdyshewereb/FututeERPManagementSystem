using ERPManagement.UI.DataAccess;


//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ERPManagement.UI.DataModels.Accounting.MasterData.Currency
{
    public class Currency : ICloneable
    {
        public int CurrencyID { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyNameAr { get; set; }
        public string CurrencyNameEn { get; set; }
        public string ChangeNameAr { get; set; }
        public string ChangeNameEn { get; set; }
        public int? EINVCurrencyID { get; set; }
        public bool Deleted { get; set; }
        public int? BranchID { get; set; }

        public Currency Clone()
        {
            return (Currency)MemberwiseClone();
        }

        object ICloneable.Clone()
        {
            //return Clone();
            return this.MemberwiseClone();
        }
    }
    public class CurrencyService : Interfaces.IScopedService
    {


        public string GetCode(Main main, bool IsFromServer)
        {
            DataTable dataTable = IsFromServer ? main.SyncExecuteQuery_DataTable(" Currency_GetCode ", true, null) : main.ExecuteQuery_DataTable("Currency_GetCode ", true, null);
            return dataTable.Rows[0][0].ToString();
        }


        public List<Currency> SelectByBranchID(int BranchID, bool IsArabic, Main main, bool IsFromServer)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@BranchID", BranchID);
            parameters[1] = new SqlParameter("@IsArabic", IsArabic);
            DataTable dataTable = IsFromServer ? main.SyncExecuteQuery_DataTable(" Currency_SelectByBranchID ", true, parameters) : main.ExecuteQuery_DataTable("Currency_SelectByBranchID", true, parameters);
            List<Currency> lst = new List<Currency>();
            if (dataTable != null)
            {
                lst = main.CreateListFromTable<Currency>(dataTable);
            }
            return lst;
        }
        public List<Currency> Select(int CurrencyId, string BranchIDs, bool IsArabic, Main main, bool IsFromServer)
        {
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@CurrencyID", CurrencyId);
            parameters[1] = new SqlParameter("@BranchIDs", BranchIDs);
            parameters[2] = new SqlParameter("@IsArabic", IsArabic);
            DataTable dataTable = IsFromServer ? main.SyncExecuteQuery_DataTable("Currency_Select", true, parameters) : main.ExecuteQuery_DataTable("Currency_Select", true, parameters);
            return main.CreateListFromTable<Currency>(dataTable);
        }


        public DataTable FillCurrencyByDate(string Date, bool IsArabic, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@Date", Date);
            parameters[1] = new SqlParameter("@IsArabic", IsArabic);
            DataTable dataTable = main.SyncExecuteQuery_DataTable("Currency_Fill_Combo_ByDate", true, parameters);
            return dataTable;
        }

        #region New
        ///////// New Methods For DataTable  //////////
        public DataTable SelectDataTable(int CurrencyId, string BranchIDs, bool IsArabic, Main main, bool IsFromServer)
        {
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@CurrencyID", CurrencyId);
            parameters[1] = new SqlParameter("@BranchIDs", BranchIDs);
            parameters[2] = new SqlParameter("@IsArabic", IsArabic);
            DataTable dataTable = IsFromServer ? main.SyncExecuteQuery_DataTable("Currency_Select", true, parameters) : main.ExecuteQuery_DataTable("Currency_Select", true, parameters);
            return dataTable;
        }
        public Currency MapRowToCurrency(DataRow row)
        {
            return new Currency
            {
                CurrencyID = int.Parse(row["CurrencyID"].ToString()),
                CurrencyCode = row["CurrencyCode"]?.ToString(),
                CurrencyNameAr = row["CurrencyNameAr"]?.ToString(),
                CurrencyNameEn = row["CurrencyNameEn"]?.ToString(),
                ChangeNameAr = row["ChangeNameAr"]?.ToString(),
                ChangeNameEn = row["ChangeNameEn"]?.ToString(),
                EINVCurrencyID = row["EINVCurrencyID"] == DBNull.Value ? null : Convert.ToInt32(row["EINVCurrencyID"]),
                BranchID = row["BranchID"] == DBNull.Value ? null : Convert.ToInt32(row["BranchID"])
            };
        }

        #endregion

        #region Update
        public int Insert_Update(Currency cur, int UserID, Main main, bool IsFromServer)
        {
            SqlParameter[] parameters = new SqlParameter[10];

            parameters[0] = new SqlParameter("@CurrencyID", cur.CurrencyID);
            parameters[1] = new SqlParameter("@CurrencyCode", cur.CurrencyCode);
            parameters[2] = new SqlParameter("@CurrencyNameAr", cur.CurrencyNameAr);
            parameters[3] = new SqlParameter("@CurrencyNameEn", cur.CurrencyNameEn);
            parameters[4] = new SqlParameter("@ChangeNameAr", cur.ChangeNameAr);
            parameters[5] = new SqlParameter("@ChangeNameEn", cur.ChangeNameEn);
            parameters[6] = new SqlParameter("@EINVCurrencyID", SqlDbType.Int);
            //////////////////////////////////////////
            //parameters[6].Value = cur.EINVCurrencyID;
            parameters[6].Value = cur.EINVCurrencyID ?? (object)DBNull.Value; // Handle null value
            parameters[7] = new SqlParameter("@Deleted", cur.Deleted);
            parameters[8] = new SqlParameter("@BranchID", cur.BranchID ?? (object)DBNull.Value);
            parameters[9] = new SqlParameter("@UserID", UserID);

            DataTable dataTable = IsFromServer ? main.SyncExecuteQuery_DataTable("Currency_Insert_Update", true, parameters) : main.ExecuteQuery_DataTable("Currency_Insert_Update", true, parameters);
            return int.Parse(dataTable.Rows[0][0].ToString());

        }

        public bool Delete(int CurrencyId, int UserID, Main main, bool IsFromServer)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@CurrencyID", CurrencyId);
            parameters[1] = new SqlParameter("@UserID", UserID);

            if (IsFromServer)
            {
                main.SyncExecuteNonQuery(" Currency_Delete ", true, parameters);
                return true;
            }
            else
            {
                main.ExecuteNonQuery("Currency_Delete", true, parameters);
                return true;
            }
        }
        #endregion

    }
}
