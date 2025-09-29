using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataModels.General;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.Accounting
{
public class JV
{

        public int JVID { get; set; } 

public string JVNo { get; set; }

public DateTime JVDate { get; set; }

public decimal TotalDebit { get; set; }

public decimal TotalCredit { get; set; }

public int TransTypeID { get; set; }

public int? VoucherID { get; set; }

public string? ReceiptNo { get; set; }

public string? Notes { get; set; }

public bool  IsOpenningJv { get; set; }

public bool  Approved { get; set; }

public bool  Deleted { get; set; }

public int? BranchID { get; set; }

public bool IsInternalJV { get; set; }

public JV Clone()
{
return (JV)MemberwiseClone();
}
}
public class JVService : Interfaces.IScopedService
{

public string GetCode(string Date, string BranchID, int TransTypeID,Main main)
{
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@Date", Date);
            parameters[1] = new SqlParameter("@BranchID", BranchID);
            parameters[2] = new SqlParameter("@TransTypeID", TransTypeID == null ? DBNull.Value :TransTypeID);
            DataTable dataTable = main.ExecuteQuery_DataTable("A_JV_GetCode2", true, parameters);
            return dataTable.Rows[0][0].ToString();
}
        public bool Check_Code(int JVID, string Date, string Code, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@JVID", JVID);
            parameters[1] = new SqlParameter("@Date", Date);
            parameters[2] = new SqlParameter("@Code", Code);
            DataTable dataTable = main.ExecuteQuery_DataTable("A_JV_Check_Code", true, parameters);
            return dataTable.Rows.Count > 0;
        }
        public string GetCodeByBranchID(DateTime Date,string BranchID,Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@Date", Date);
parameters[1] = new SqlParameter("@BranchID",BranchID);
DataTable dataTable = main.ExecuteQuery_DataTable("A_JV_GetCodeByBranchID",true,parameters);
return dataTable.Rows[0][0].ToString();
}

public int Insert_Update(JV obj,int UserID,Main main)
{
SqlParameter[] parameters = new SqlParameter[15];
parameters[0] = new SqlParameter("@JVID", obj.JVID== null ? DBNull.Value : obj.JVID);
parameters[1] = new SqlParameter("@JVNo", obj.JVNo== null ? DBNull.Value : obj.JVNo);
parameters[2] = new SqlParameter("@JVDate", obj.JVDate== null ? DBNull.Value : obj.JVDate);
parameters[3] = new SqlParameter("@TotalDebit", obj.TotalDebit== null ? DBNull.Value : obj.TotalDebit);
parameters[4] = new SqlParameter("@TotalCredit", obj.TotalCredit== null ? DBNull.Value : obj.TotalCredit);
parameters[5] = new SqlParameter("@TransTypeID", obj.TransTypeID== null ? DBNull.Value : obj.TransTypeID);
parameters[6] = new SqlParameter("@VoucherID", obj.VoucherID== null ? DBNull.Value : obj.VoucherID);
parameters[7] = new SqlParameter("@ReceiptNo", obj.ReceiptNo== null ? DBNull.Value : obj.ReceiptNo);
parameters[8] = new SqlParameter("@Notes", obj.Notes== null ? DBNull.Value : obj.Notes);
parameters[9] = new SqlParameter("@IsOpenningJv", obj.IsOpenningJv== null ? false : obj.IsOpenningJv);
parameters[10] = new SqlParameter("@Approved", obj.Approved== null ? false : obj.Approved);
parameters[11] = new SqlParameter("@Deleted", obj.Deleted== null ? false : obj.Deleted);
parameters[12] = new SqlParameter("@BranchID", obj.BranchID== null ? DBNull.Value : obj.BranchID);
parameters[13] = new SqlParameter("@IsInternalJV", obj.IsInternalJV== null ? false : obj.IsInternalJV);
parameters[14] = new SqlParameter("@UserID",UserID);
DataTable dataTable = main.ExecuteQuery_DataTable("A_JV_Insert_Update",true,parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}
        public int GenerateJV_Insert(JV obj , List<JVDetails> lstJvDetails,  string BranchID,  string UserID, Main main)
        {
            main.strMessageDetail = "";
            int ErrorCounter = 0;
            decimal TotalDebit = 0;
            decimal TotalCredit = 0;

            #region Details

            string AccountID;
            string SubAccountID;
            string CostCenterID;
            string SubCostCenterID;
            string Debit;
            string Credit;
            string CurrencyID;
            string ExchangeRate;
            string LocalDebit;
            string LocalCredit;
            string IsDocumented;
            string Notes;
            string strSql = string.Empty;

            for (int i = 0; i < lstJvDetails.Count; i++)
            {
                if (lstJvDetails[i].LocalDebit==null) lstJvDetails[i].LocalDebit  = 0.0m;
                if (lstJvDetails[i].LocalCredit==null) lstJvDetails[i].LocalCredit = 0.0m;
                if (lstJvDetails[i].LocalDebit.Equals(0.0) && lstJvDetails[i].LocalCredit.Equals(0.0))
                {
                    main.strMessageDetail += "Value In Row Number " + (i + 1) + " Should Not Be 0 \n";
                    ErrorCounter++;
                }
                if (lstJvDetails[i].LocalDebit < 0 || lstJvDetails[i].LocalCredit < 0)
                {
                    main.strMessageDetail += "Value In Row Number " + (i + 1) + " Should Not Be Less Than 0 \n";
                    ErrorCounter++;
                }
                if (lstJvDetails[i].AccountID.Equals(DBNull.Value))
                {
                    main.strMessageDetail += "Account In Row Number " + (i + 1) + " Should Not Be Null  \n";
                    ErrorCounter++;
                }
                if (lstJvDetails[i].CurrencyID.Equals(DBNull.Value))
                {
                    main.strMessageDetail += "Currency In Row Number " + (i + 1) + " Should Not Be Null  \n";
                    ErrorCounter++;
                }
                if (lstJvDetails[i].ExchangeRate.Equals(DBNull.Value) || lstJvDetails[i].ExchangeRate.Equals(0.0))
                {
                    main.strMessageDetail += "Currency In Row Number " + (i + 1) + " Should Not Be Null Or 0 \n";
                    ErrorCounter++;
                }
                if (ErrorCounter > 0) continue;

                AccountID = lstJvDetails[i].AccountID.ToString();
                SubAccountID = lstJvDetails[i].SubAccountID.ToString();
                CostCenterID = lstJvDetails[i].CostCenterID.ToString();
                SubCostCenterID = lstJvDetails[i].SubCostCenterID.ToString();
                Debit = lstJvDetails[i].Debit.ToString();
                Credit = lstJvDetails[i].Credit.ToString();
                CurrencyID = lstJvDetails[i].CurrencyID.ToString();
                ExchangeRate = lstJvDetails[i].ExchangeRate.ToString();
                LocalDebit = lstJvDetails[i].LocalDebit.ToString();
                LocalCredit = lstJvDetails[i].LocalCredit.ToString();
                IsDocumented = lstJvDetails[i].IsDocumented.ToString();
                Notes = lstJvDetails[i].Notes == null ? "" :lstJvDetails[i].Notes.ToString();

                strSql += " exec A_JVDetails_GenerateJV -1,@JVID,";

                strSql += "'" + AccountID + "',";
                strSql += SubAccountID.Equals("") ? "NULL," : "'" + SubAccountID + "',";
                strSql += CostCenterID.Equals("") ? "NULL," : "'" + CostCenterID + "',";
                strSql += SubCostCenterID.Equals("") ? "NULL," : "'" + SubCostCenterID + "',";
                strSql += Debit.Equals("") ? "NULL," : "'" + Debit + "',";
                strSql += Credit.Equals("") ? "NULL," : "'" + Credit + "',";
                strSql += "'" + CurrencyID + "',";
                strSql += "'" + ExchangeRate + "',";
                strSql += "'" + LocalDebit + "',";
                strSql += "'" + LocalCredit + "',";
                strSql += IsDocumented.Equals("") ? "0," : IsDocumented.Equals("True") ? "1," : IsDocumented.Equals("False") ? "0," : "'" + IsDocumented + "',";
                strSql += Notes.Equals("") ? "NULL," : "'" + Notes + "',";
                strSql += obj.Deleted.Equals("") ? "0," : obj.Deleted.Equals("True") ? "1," :   obj.Deleted.Equals("False") ? "0," : "'" + obj.Deleted + "',";
                strSql += BranchID.Equals("") ? "NULL," : "'" + BranchID + "'";
                strSql += ";";



                TotalDebit += Convert.ToDecimal(lstJvDetails[i].LocalDebit);
                TotalCredit += Convert.ToDecimal(lstJvDetails[i].LocalCredit);

            }

            #endregion

            #region Header

            if (TotalDebit != TotalCredit)
            {
                main.strMessageDetail += "TotalDebit Not Equal TotalCredit \n";
                ErrorCounter++;
            }
            if (ErrorCounter > 0)
            {
                main.strMessage = ErrorCounter + " Error Occurred In This JV ";
                //frmMessageBox Message = new frmMessageBox();
                //Message.ShowDialog();
                return 0;
            }
            if (obj.JVNo == "" || Check_Code(0, obj.JVDate.ToString("yyyy-MM-dd"), obj.JVNo,main))
            {
                obj.JVNo = GetCode(obj.JVDate.ToString("yyyy-MM-dd"), BranchID, obj.TransTypeID,main);
            }
            string strHSql = string.Empty;
            strHSql += " declare @JVID as int set @JVID= -1 ";
            strHSql += " exec A_JV_Insert_Update @JVID output,";
            strHSql += obj.JVNo.Equals("Null") ? "NULL," : "'" + obj.JVNo + "',";
            strHSql += obj.JVDate.Equals("Null") ? "NULL," : "'" + obj.JVDate + "',";
            strHSql += "'" + TotalDebit + "',";
            strHSql += "'" + TotalCredit + "',";
            strHSql += obj.TransTypeID.Equals("Null") ? "NULL," : "'" + obj.TransTypeID + "',";
            strHSql += obj.VoucherID.Equals("Null") ? "NULL," : "'" + obj.VoucherID + "',";
            strHSql += obj.ReceiptNo == null ? "NULL," : "'" + obj.ReceiptNo + "',";
            strHSql += obj.Notes == null ? "NULL," : "'" + obj.Notes + "',";
            strHSql += obj.IsOpenningJv == null ? "0," : "'" + (obj.IsOpenningJv ? "1" : "0" ) + "',";
            strHSql += obj.Approved == null ? "0," : "'" + (obj.Approved ?"1":"0" )+ "',";
            strHSql += obj.Deleted == null ? "0," : "'" + (obj.Deleted ? "1" : "0" )+"',";
            strHSql += BranchID == null ? "NULL," : "'" + BranchID + "',";
            strHSql += obj.IsInternalJV == null ? "0," : "'" + (obj.IsInternalJV ? "1" : "0" ) + "',";
            strHSql += UserID + " ; ";

            #endregion
            DataTable dataTable = main.ExecuteQuery_DataTable(strHSql + strSql, false, null);
            return int.Parse(dataTable.Rows[0][0].ToString());
        }

        public int GenerateJV_Update(JV obj, List<JVDetails> lstJvDetails, string BranchID, string UserID, Main main)
        {
            int ErrorCounter = 0;
            decimal TotalDebit = 0;
            decimal TotalCredit = 0;
            string DetailIDs = ",";

            #region Details

            string JVDetailID;
            string AccountID;
            string SubAccountID;
            string CostCenterID;
            string SubCostCenterID;
            string Debit;
            string Credit;
            string CurrencyID;
            string ExchangeRate;
            string LocalDebit;
            string LocalCredit;
            string IsDocumented;
            string Notes;
            string strSql = string.Empty;

            for (int i = 0; i < lstJvDetails.Count; i++)
            {
                if (lstJvDetails[i].LocalDebit.Equals(DBNull.Value)) lstJvDetails[i].LocalDebit = 0.0m;
                if (lstJvDetails[i].LocalCredit.Equals(DBNull.Value)) lstJvDetails[i].LocalCredit = 0.0m;
                if (lstJvDetails[i].LocalDebit.Equals(0.0) && lstJvDetails[i].LocalCredit.Equals(0.0))
                {
                    main.strMessageDetail += "Value In Row Number " + (i + 1) + " Should Not Be 0 \n";
                    ErrorCounter++;
                }
                if (Convert.ToDecimal(lstJvDetails[i].LocalDebit) < 0 || Convert.ToDecimal(lstJvDetails[i].LocalCredit) < 0)
                {
                    main.strMessageDetail += "Value In Row Number " + (i + 1) + " Should Not Be Less Than 0 \n";
                    ErrorCounter++;
                }

                if (lstJvDetails[i].AccountID.Equals(DBNull.Value))
                {
                    main.strMessageDetail += "Account In Row Number " + (i + 1) + " Should Not Be Null  \n";
                    ErrorCounter++;
                }
                if (lstJvDetails[i].CurrencyID.Equals(DBNull.Value))
                {
                    main.strMessageDetail += "Currency In Row Number " + (i + 1) + " Should Not Be Null  \n";
                    ErrorCounter++;
                }
                if (lstJvDetails[i].ExchangeRate.Equals(DBNull.Value) || lstJvDetails[i].ExchangeRate.Equals(0.0))
                {
                    main.strMessageDetail += "Currency In Row Number " + (i + 1) + " Should Not Be Null Or 0 \n";
                    ErrorCounter++;
                }
                if (ErrorCounter > 0) continue;

                JVDetailID = lstJvDetails[i].JVDetailID.ToString();
                AccountID = lstJvDetails[i].AccountID.ToString();
                SubAccountID = lstJvDetails[i].SubAccountID.ToString();
                CostCenterID = lstJvDetails[i].CostCenterID.ToString();
                SubCostCenterID = lstJvDetails[i].SubCostCenterID.ToString();
                Debit = lstJvDetails[i].Debit.ToString();
                Credit = lstJvDetails[i].Credit.ToString();
                CurrencyID = lstJvDetails[i].CurrencyID.ToString();
                ExchangeRate = lstJvDetails[i].ExchangeRate.ToString();
                LocalDebit = lstJvDetails[i].LocalDebit.ToString();
                LocalCredit = lstJvDetails[i].LocalCredit.ToString();
                IsDocumented = lstJvDetails[i].IsDocumented.ToString();
                Notes = lstJvDetails[i].Notes == null ? "" : lstJvDetails[i].Notes.ToString();

                strSql += " exec A_JVDetails_GenerateJV " + (JVDetailID.Equals("") ? "-1," : JVDetailID + ",");

                strSql += "" + obj.JVID + ",";
                strSql += "'" + AccountID + "',";
                strSql += SubAccountID.Equals("") ? "NULL," : "'" + SubAccountID + "',";
                strSql += CostCenterID.Equals("") ? "NULL," : "'" + CostCenterID + "',";
                strSql += SubCostCenterID.Equals("") ? "NULL," : "'" + SubCostCenterID + "',";
                strSql += Debit.Equals("") ? "NULL," : "'" + Debit + "',";
                strSql += Credit.Equals("") ? "NULL," : "'" + Credit + "',";
                strSql += "'" + CurrencyID + "',";
                strSql += "'" + ExchangeRate + "',";
                strSql += "'" + LocalDebit + "',";
                strSql += "'" + LocalCredit + "',";
                strSql += IsDocumented.Equals("") ? "0," : IsDocumented.Equals("True") ? "1," : IsDocumented.Equals("False") ? "0," : "'" + IsDocumented + "',";
                strSql += Notes.Equals("") ? "NULL," : "'" + Notes + "',";
                strSql += obj.Deleted.Equals("") ? "0," : obj.Deleted.Equals("True") ? "1," : obj.Deleted.Equals("False") ? "0," : "'" + obj.Deleted + "',";
                strSql += BranchID.Equals("") ? "NULL," : "'" + BranchID + "'";
                strSql += ";";



                TotalDebit += Convert.ToDecimal(lstJvDetails[i].LocalDebit);
                TotalCredit += Convert.ToDecimal(lstJvDetails[i].LocalCredit);
                if (JVDetailID != "-1" || JVDetailID != "") DetailIDs += JVDetailID + ",";
            }

            #endregion

            #region Header

            if (TotalDebit != TotalCredit)
            {
                main.strMessageDetail += "TotalDebit Not Equal TotalCredit \n";
                ErrorCounter++;
            }
            if (obj.JVNo.Equals("Null") || obj.JVNo.Equals(""))
            {
                main.strMessageDetail += "JVNo Should Not Be Null \n";
                ErrorCounter++;
            }
            if (obj.JVID.Equals("Null") || obj.JVID.Equals(""))
            {
                main.strMessageDetail += "JVID Should Not Be Null \n";
                ErrorCounter++;
            }
            if (ErrorCounter > 0)
            {
                main.strMessage = ErrorCounter + " Error Occurred In This JV (Support Required)";
                //frmMessageBox Message = new frmMessageBox();
                //Message.ShowDialog();
                return 0;
            }
            string strHSql = string.Empty;
            strHSql += " exec A_JVDetails_DeleteForUpdate " + obj.JVID + ",'" + DetailIDs + "'; ";
            strHSql += " exec A_JV_Insert_Update ";
            strHSql += obj.JVID + ",";
            strHSql += "'" + obj.JVNo + "',";
            strHSql += obj.JVDate.Equals("Null") ? "NULL," : "'" + obj.JVDate + "',";
            strHSql += "'" + TotalDebit + "',";
            strHSql += "'" + TotalCredit + "',";
            strHSql += obj.TransTypeID.Equals("Null") ? "NULL," : "'" + obj.TransTypeID + "',";
            strHSql += obj.VoucherID.Equals("Null") ? "NULL," : "'" + obj.VoucherID + "',";
            strHSql += obj.ReceiptNo == null ? "NULL," : "'" + obj.ReceiptNo + "',";
            strHSql += obj.Notes == null ? "NULL," : "'" + obj.Notes + "',";
            strHSql += obj.IsOpenningJv == null ? "0," : "'" + (obj.IsOpenningJv ? "1" : "0") + "',";
            strHSql += obj.Approved == null ? "0," : "'" + (obj.Approved ? "1" : "0") + "',";
            strHSql += obj.Deleted == null ? "0," : "'" + (obj.Deleted ? "1" : "0") + "',";
            strHSql += BranchID == null ? "NULL," : "'" + BranchID + "',";
            strHSql += obj.IsInternalJV == null ? "0," : "'" + (obj.IsInternalJV ? "1" : "0") + "',";
            strHSql += UserID + " ; ";

            #endregion
            DataTable dataTable = main.ExecuteQuery_DataTable(strHSql + strSql, false, null);
            return int.Parse(dataTable.Rows[0][0].ToString());
        }
        public DataTable Search(string BranchIDs, string FromDate, string ToDate, int Approved, bool Deleted, bool IsArabic, bool SeeingInvisibleAccounts,Main main)
        {
            SqlParameter[] parameters = new SqlParameter[7];
            parameters[0] = new SqlParameter("@BranchIDs", BranchIDs);
            parameters[1] = new SqlParameter("@FromDate", FromDate);
            parameters[2] = new SqlParameter("@ToDate", ToDate);
            parameters[3] = new SqlParameter("@Approved", Approved);
            parameters[4] = new SqlParameter("@Deleted", Deleted);
            parameters[5] = new SqlParameter("@IsArabic", IsArabic);
            parameters[6] = new SqlParameter("@SeeingInvisibleAccounts", SeeingInvisibleAccounts);
            DataTable dataTable = main.ExecuteQuery_DataTable("A_JV_Search", true, parameters);
            return dataTable;
        }

        public List<JV> Select(int JVID, string BranchIDs, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[3];
parameters[0] = new SqlParameter("@JVID", JVID);
parameters[1] = new SqlParameter("@BranchIDs",BranchIDs);
parameters[2] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable =main.ExecuteQuery_DataTable("A_JV_Select",true,parameters);
List<JV> lst = new List<JV>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<JV>(dataTable);
}
return lst;
}

public void Delete(int JVID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JVID", JVID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_JV_Delete",true,parameters);
}
public void DeleteVirtual(int JVID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JVID", JVID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_JV_DeleteVirtual",true,parameters);
}


public List<JV> SelectByFiscalYearID(int FiscalYearID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@FiscalYearID", FiscalYearID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("A_JV_SelectByFiscalYearID",true,parameters);
List<JV> lst = new List<JV>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<JV>(dataTable);
}
return lst;
}

public void DeleteByFiscalYearID(int FiscalYearID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@FiscalYearID", FiscalYearID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_JV_DeleteByFiscalYearID",true,parameters);
}
public void DeleteVirtualByFiscalYearID(int FiscalYearID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@FiscalYearID", FiscalYearID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_JV_DeleteVirtualByFiscalYearID" ,true,parameters);
}

public List<JV> SelectByJVID(int JVID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@JVID", JVID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("A_JV_SelectByJVID", true,parameters);
List<JV> lst = new List<JV>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<JV>(dataTable);
}
return lst;
}

public void DeleteByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_JV_DeleteByBranchID",true,parameters);
}
public void DeleteVirtualByBranchID(int BranchID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@BranchID", BranchID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_JV_DeleteVirtualByBranchID" ,true,parameters);
}

public List<JV> SelectByTransTypeID(int TransTypeID, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@TransTypeID", TransTypeID);
parameters[1] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable = main.ExecuteQuery_DataTable("A_JV_SelectByTransTypeID",true,parameters);
List<JV> lst = new List<JV>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<JV>(dataTable);
}
return lst;
}

public void DeleteByTransTypeID(int TransTypeID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@TransTypeID", TransTypeID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_JV_DeleteByTransTypeID",true,parameters);
}
public void DeleteVirtualByTransTypeID(int TransTypeID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@TransTypeID", TransTypeID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("A_JV_DeleteVirtualByTransTypeID" ,true,parameters);
}

}
}
