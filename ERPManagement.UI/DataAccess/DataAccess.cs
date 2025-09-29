using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Data;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Reflection;
using System.Xml.Serialization;
using System.Xml;

namespace ERPManagement.UI.DataAccess
{

    public class Main
    {
        #region Fields

        public bool TicketReq;
        public string strMessage;
        public string strMessageDetail;
        public bool Language;
        public bool bol_Language;
        public SqlException _SqlException;
        public Exception _Exception;
        public bool Success;
        public bool IsBulkTrans;
        public bool IsSynchronization = false;
        #endregion
        DbConnection dbConnection;

        public Main(string conStr)
        {
            dbConnection = new DbConnection(conStr);
        }

        #region Methods
        public DataTable SyncExecuteQuery_DataTable(string strQuery, bool IsProcedure, SqlParameter[]? Parameters=null)
        {
            //MessageBox.Show(strQuery);
            DataTable dataTable = new DataTable();
            try
            {
                if (IsSynchronization)
                {
                    if (IsBulkTrans)
                    {
                        if (!dbConnection.BulkTransHasErrors)
                            dataTable = dbConnection.SyncExecuteQuery_DataTableBulkTrans(strQuery, IsProcedure, Parameters);
                    }
                    else
                    {
                        dataTable = dbConnection.SyncExecuteQuery_DataTable(strQuery, IsProcedure, Parameters);
                    }
                }
                else
                {
                    if (IsBulkTrans)
                    {
                        if (!dbConnection.BulkTransHasErrors)
                            dataTable = dbConnection.ExecuteQuery_DataTableBulkTrans(strQuery, IsProcedure, Parameters);
                    }
                    else
                    {
                        dataTable = dbConnection.ExecuteQuery_DataTable(strQuery, IsProcedure, Parameters);
                    }
                }
                Success = true;
            }
            // Catch SqlExceptions and generate the appropraiate error message
            catch (SqlException ex)
            {
                //if (ex.Message.Contains("Login fail") || ex.Message.Contains("server was not found"))
                //{
                //    MessageBox.Show("Login Faild To Server ", "Server Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    if (IsBulkTrans) dbConnection.BulkTransHasErrors = true;
                //    Success = false;
                //    return null;
                //}
                _SqlException = ex;
                Language = bol_Language;
                strMessage = Error_Msg(ex);
                strMessageDetail = strQuery;
                strMessageDetail += "\r\n";
                strMessageDetail += "-----------------------------------";
                strMessageDetail += "\r\n";
                strMessageDetail += " SqlException Number : " + ex.Number.ToString();
                strMessageDetail += "\r\n";
                strMessageDetail += "-----------------------------------";
                strMessageDetail += "\r\n";
                strMessageDetail += " SqlException Number : " + ex.Message.ToString();
                strMessageDetail += "\r\n";
                strMessageDetail += "-----------------------------------";

                //frmMessageBox Message = new frmMessageBox();
                //Message.ShowDialog();

                if (IsBulkTrans) dbConnection.BulkTransHasErrors = true;
                Success = false;
                TicketReq = true;
            }
            // Catch other exceptions
            catch (Exception ex)
            {
                _Exception = ex;
                Language = bol_Language;
                strMessage = Error_Msg();
                strMessageDetail = strQuery;
                strMessageDetail += "\r\n";
                strMessageDetail += "-----------------------------------";
                strMessageDetail += "\r\n";
                strMessageDetail += ex.Message.ToString();
                strMessageDetail += "\r\n";
                strMessageDetail += "-----------------------------------";
                strMessageDetail += "\r\n";

                //frmMessageBox Message = new frmMessageBox();
                //Message.ShowDialog();
                if (IsBulkTrans) dbConnection.BulkTransHasErrors = true;
                Success = false;
                TicketReq = true;
            }
            return dataTable;
        }
        public DataTable SyncExecuteQuery_DataTable(string strQuery, bool IsProcedure, SqlParameter[]? Parameters, int SyncTimeOut)
        {
            //MessageBox.Show(strQuery);
            DataTable dataTable = new DataTable();
            try
            {
                if (IsSynchronization)
                {
                    if (IsBulkTrans)
                    {
                        if (!dbConnection.BulkTransHasErrors)
                            dataTable = dbConnection.SyncExecuteQuery_DataTableBulkTrans(strQuery, IsProcedure, Parameters, SyncTimeOut);
                    }
                    else
                    {
                        dataTable = dbConnection.SyncExecuteQuery_DataTable(strQuery, IsProcedure, Parameters, SyncTimeOut);
                    }
                }
                else
                {
                    if (IsBulkTrans)
                    {
                        if (!dbConnection.BulkTransHasErrors)
                            dataTable = dbConnection.ExecuteQuery_DataTableBulkTrans(strQuery, IsProcedure, Parameters);
                    }
                    else
                    {
                        dataTable = dbConnection.ExecuteQuery_DataTable(strQuery, IsProcedure, Parameters);
                    }
                }
                Success = true;
            }
            // Catch SqlExceptions and generate the appropraiate error message
            catch (SqlException ex)
            {
                //if (ex.Message.Contains("Login fail") || ex.Message.Contains("server was not found"))
                //{
                //    MessageBox.Show("Login Faild To Server ", "Server Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    if (IsBulkTrans) dbConnection.BulkTransHasErrors = true;
                //    Success = false;
                //    return null;
                //}
                _SqlException = ex;
                Language = bol_Language;
                strMessage = Error_Msg(ex);
                strMessageDetail = strQuery;
                strMessageDetail += "\r\n";
                strMessageDetail += "-----------------------------------";
                strMessageDetail += "\r\n";
                strMessageDetail += " SqlException Number : " + ex.Number.ToString();
                strMessageDetail += "\r\n";
                strMessageDetail += "-----------------------------------";
                strMessageDetail += "\r\n";
                strMessageDetail += " SqlException Number : " + ex.Message.ToString();
                strMessageDetail += "\r\n";
                strMessageDetail += "-----------------------------------";

                //frmMessageBox Message = new frmMessageBox();
                //Message.ShowDialog();

                if (IsBulkTrans) dbConnection.BulkTransHasErrors = true;
                Success = false;
                TicketReq = true;
            }
            // Catch other exceptions
            catch (Exception ex)
            {
                _Exception = ex;
                Language = bol_Language;
                strMessage = Error_Msg();
                strMessageDetail = strQuery;
                strMessageDetail += "\r\n";
                strMessageDetail += "-----------------------------------";
                strMessageDetail += "\r\n";
                strMessageDetail += ex.Message.ToString();
                strMessageDetail += "\r\n";
                strMessageDetail += "-----------------------------------";
                strMessageDetail += "\r\n";

                //frmMessageBox Message = new frmMessageBox();
                //Message.ShowDialog();
                if (IsBulkTrans) dbConnection.BulkTransHasErrors = true;
                Success = false;
                TicketReq = true;
            }
            return dataTable;
        }
        public DataTable ExecuteQuery_DataTable(string strQuery, bool IsProcedure, SqlParameter[]? Parameters=null)
        {
            //MessageBox.Show(strQuery);
            DataTable dataTable = new DataTable();
            try
            {

                if (IsBulkTrans)
                {
                    if (!dbConnection.BulkTransHasErrors)
                        dataTable = dbConnection.ExecuteQuery_DataTableBulkTrans(strQuery, IsProcedure, Parameters);
                }
                else
                {
                    dataTable = dbConnection.ExecuteQuery_DataTable(strQuery, IsProcedure, Parameters);
                }
                Success = true;
            }
            // Catch SqlExceptions and generate the appropraiate error message
            catch (SqlException ex)
            {
                _SqlException = ex;
                Language = bol_Language;
                strMessage = Error_Msg(ex);
                strMessageDetail = strQuery;
                strMessageDetail += "\r\n";
                strMessageDetail += "-----------------------------------";
                strMessageDetail += "\r\n";
                strMessageDetail += " SqlException Number : " + ex.Number.ToString();
                strMessageDetail += "\r\n";
                strMessageDetail += "-----------------------------------";
                strMessageDetail += "\r\n";
                strMessageDetail += " SqlException Number : " + ex.Message.ToString();
                strMessageDetail += "\r\n";
                strMessageDetail += "-----------------------------------";
                if (IsBulkTrans) dbConnection.BulkTransHasErrors = true;
                //frmMessageBox Message = new frmMessageBox();
                //Message.ShowDialog();

                Success = false;
                TicketReq = true;
            }
            // Catch other exceptions
            catch (Exception ex)
            {
                _Exception = ex;
                Language = bol_Language;
                strMessage = Error_Msg();
                strMessageDetail = strQuery;
                strMessageDetail += "\r\n";
                strMessageDetail += "-----------------------------------";
                strMessageDetail += "\r\n";
                strMessageDetail += ex.Message.ToString();
                strMessageDetail += "\r\n";
                strMessageDetail += "-----------------------------------";
                strMessageDetail += "\r\n";
                if (IsBulkTrans) dbConnection.BulkTransHasErrors = true;

                //frmMessageBox Message = new frmMessageBox();
                //Message.ShowDialog();
                Success = false;
                TicketReq = true;
            }
            return dataTable;
        }
        public DataTable ExecuteQuery_DataTableWithoutErrorMessage(string strQuery, bool IsProcedure, SqlParameter[]? Parameters = null)
        {
            //MessageBox.Show(strQuery);
            DataTable dataTable = new DataTable();
            try
            {

                if (IsBulkTrans)
                {
                    if (!dbConnection.BulkTransHasErrors)
                        dataTable = dbConnection.ExecuteQuery_DataTableBulkTrans(strQuery, IsProcedure, Parameters);
                }
                else
                {
                    dataTable = dbConnection.ExecuteQuery_DataTable(strQuery, IsProcedure, Parameters);
                }
                Success = true;
            }
            // Catch SqlExceptions and generate the appropraiate error message
            catch
            {
                if (IsBulkTrans) dbConnection.BulkTransHasErrors = true;
                Success = false;
            }
            return dataTable;
        }

        //public DataTable ExecuteQuery_DataTableWithoutErrorMessageThread(string strQuery)
        //{
        //    return dbConnection.ExecuteQuery_Thread(strQuery);
        //}

        public void StartBulkTrans(bool FromServer)
        {
            IsBulkTrans = true;
            dbConnection.StartBulkTrans(IsSynchronization ? FromServer : false);
        }
        public void EndBulkTrans(bool FromServer)
        {
            IsBulkTrans = false;
            dbConnection.EndBulkTrans(IsSynchronization ? FromServer : false);
        }
        public void RollbackBulkTrans(bool FromServer)
        {
            IsBulkTrans = false;
            dbConnection.RollbackBulkTrans(IsSynchronization ? FromServer : false);
        }

        public DataTable SyncExecuteQuery_DataTable_Trans(string strQuery, bool IsProcedure, SqlParameter[]? Parameters = null)
        {
            //MessageBox.Show(strQuery);
            DataTable dataTable = new DataTable();
            try
            {
                if (IsSynchronization)
                {
                    if (IsBulkTrans)
                    {
                        if (!dbConnection.BulkTransHasErrors)
                            dataTable = dbConnection.SyncExecuteQuery_DataTableBulkTrans(strQuery, IsProcedure, Parameters);
                    }
                    else
                    {
                        dataTable = dbConnection.SyncExecuteQuery_DataTable(strQuery, IsProcedure, Parameters);
                    }
                }
                else
                {
                    if (IsBulkTrans)
                    {
                        if (!dbConnection.BulkTransHasErrors)
                            dataTable = dbConnection.ExecuteQuery_DataTableBulkTrans(strQuery, IsProcedure, Parameters);
                    }
                    else
                    {
                        dataTable = dbConnection.ExecuteQuery_DataTable(strQuery, IsProcedure, Parameters);
                    }
                }
                Success = true;
            }
            // Catch SqlExceptions and generate the appropraiate error message
            catch (SqlException ex)
            {
                //if (ex.Message.Contains("Login fail") || ex.Message.Contains("server was not found"))
                //{
                //    MessageBox.Show("Login Faild To Server ", "Server Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    Success = false;
                //    if (IsBulkTrans) dbConnection.BulkTransHasErrors = true;
                //    return null;
                //}
                _SqlException = ex;
                Language = bol_Language;
                strMessage = Error_Msg(ex);
                strMessageDetail = strQuery;
                strMessageDetail += "\r\n";
                strMessageDetail += "-----------------------------------";
                strMessageDetail += "\r\n";
                strMessageDetail += " SqlException Number : " + ex.Number.ToString();
                strMessageDetail += "\r\n";
                strMessageDetail += "-----------------------------------";
                strMessageDetail += "\r\n";
                strMessageDetail += " SqlException Number : " + ex.Message.ToString();
                strMessageDetail += "\r\n";
                strMessageDetail += "-----------------------------------";

                //frmMessageBox Message = new frmMessageBox();
                //Message.ShowDialog();

                if (IsBulkTrans) dbConnection.BulkTransHasErrors = true;
                Success = false;
                TicketReq = true;
            }
            // Catch other exceptions
            catch (Exception ex)
            {
                _Exception = ex;
                Language = bol_Language;
                strMessage = Error_Msg();
                strMessageDetail = strQuery;
                strMessageDetail += "\r\n";
                strMessageDetail += "-----------------------------------";
                strMessageDetail += "\r\n";
                strMessageDetail += ex.Message.ToString();
                strMessageDetail += "\r\n";
                strMessageDetail += "-----------------------------------";
                strMessageDetail += "\r\n";

                //frmMessageBox Message = new frmMessageBox();
                //Message.ShowDialog();
                if (IsBulkTrans) dbConnection.BulkTransHasErrors = true;
                Success = false;
                TicketReq = true;
            }
            return dataTable;
        }

        public int SyncExecuteNonQuery(string strProcName, bool IsProcedure, SqlParameter[]? Parameters = null)
        {
            int rowsAffected = 0;
            try
            {
                if (IsSynchronization)
                {
                    if (IsBulkTrans)
                    {
                        if (!dbConnection.BulkTransHasErrors)
                            dbConnection.SyncExecuteQueryBulkTrans(strProcName, IsProcedure, Parameters);
                    }
                    else
                    {
                        rowsAffected = dbConnection.SyncExecuteNonQuery(strProcName, IsProcedure, Parameters);
                    }
                }
                else
                {
                    if (IsBulkTrans)
                    {
                        if (!dbConnection.BulkTransHasErrors)
                            dbConnection.ExecuteQueryBulkTrans(strProcName, IsProcedure, Parameters);
                    }
                    else
                    {
                        rowsAffected = dbConnection.ExecuteNonQuery(strProcName, IsProcedure, Parameters);
                    }
                }
                Success = true;
            }
            // Catch SqlExceptions and generate the appropraiate error message
            catch (SqlException ex)
            {
                //if (ex.Message.Contains("Login fail") || ex.Message.Contains("server was not found"))
                //{
                //    MessageBox.Show("Login Faild To Server ", "Server Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    if (IsBulkTrans) dbConnection.BulkTransHasErrors = true;
                //    Success = false;
                //    return rowsAffected;
                //}

                //if (ex.Message.Contains("REFERENCE"))
                //{
                //    MessageBox.Show("This item is used in other locations", "Can't delete this item", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    if (IsBulkTrans) dbConnection.BulkTransHasErrors = true;
                //    Success = false;
                //    return rowsAffected;
                //}
                _SqlException = ex;
                Language = bol_Language;
                strMessage = Error_Msg(ex);
                strMessageDetail = strProcName;
                strMessageDetail += "\r\n";
                strMessageDetail += "-----------------------------------";
                strMessageDetail += "\r\n";
                strMessageDetail += " SqlException Number : " + ex.Number.ToString();
                strMessageDetail += "\r\n";
                strMessageDetail += "-----------------------------------";
                strMessageDetail += "\r\n";
                strMessageDetail += " SqlException Number : " + ex.Message.ToString();
                strMessageDetail += "\r\n";
                strMessageDetail += "-----------------------------------";

                //frmMessageBox Message = new frmMessageBox();
                //Message.ShowDialog();

                if (IsBulkTrans) dbConnection.BulkTransHasErrors = true;
                Success = false;
                TicketReq = true;
            }
            // Catch other exceptions
            catch (Exception ex)
            {
                _Exception = ex;
                Language = bol_Language;
                strMessage = Error_Msg();
                strMessageDetail = strProcName;
                strMessageDetail += "\r\n";
                strMessageDetail += "-----------------------------------";
                strMessageDetail += "\r\n";
                strMessageDetail += ex.Message.ToString();
                strMessageDetail += "\r\n";
                strMessageDetail += "-----------------------------------";
                strMessageDetail += "\r\n";

                //frmMessageBox Message = new frmMessageBox();
                //Message.ShowDialog();
                if (IsBulkTrans) dbConnection.BulkTransHasErrors = true;
                Success = false;
                TicketReq = true;
            }

            return rowsAffected;
        }
        public int ExecuteNonQuery(string strProcName, bool IsProcedure, SqlParameter[]? Parameters = null)
        {
            int rowsAffected = 0;
            try
            {
                if (IsBulkTrans)
                {
                    if (!dbConnection.BulkTransHasErrors)
                        dbConnection.ExecuteQueryBulkTrans(strProcName, IsProcedure, Parameters);
                }
                else
                {
                    rowsAffected = dbConnection.ExecuteNonQuery(strProcName, IsProcedure, Parameters);
                }

                Success = true;
            }
            // Catch SqlExceptions and generate the appropraiate error message
            catch (SqlException ex)
            {
                //if (ex.Message.Contains("REFERENCE"))
                //{
                //    MessageBox.Show("This item is used in other locations", "Can't delete this item", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    if (IsBulkTrans) dbConnection.BulkTransHasErrors = true;
                //    Success = false;
                //    return rowsAffected;
                //}
                _SqlException = ex;
                Language = bol_Language;
                strMessage = Error_Msg(ex);
                strMessageDetail = strProcName;
                strMessageDetail += "\r\n";
                strMessageDetail += "-----------------------------------";
                strMessageDetail += "\r\n";
                strMessageDetail += " SqlException Number : " + ex.Number.ToString();
                strMessageDetail += "\r\n";
                strMessageDetail += "-----------------------------------";
                strMessageDetail += "\r\n";
                strMessageDetail += " SqlException Number : " + ex.Message.ToString();
                strMessageDetail += "\r\n";
                strMessageDetail += "-----------------------------------";

                //frmMessageBox Message = new frmMessageBox();
                //Message.ShowDialog();

                if (IsBulkTrans) dbConnection.BulkTransHasErrors = true;
                Success = false;
                TicketReq = true;
            }
            // Catch other exceptions
            catch (Exception ex)
            {
                _Exception = ex;
                Language = bol_Language;
                strMessage = Error_Msg();
                strMessageDetail = strProcName;
                strMessageDetail += "\r\n";
                strMessageDetail += "-----------------------------------";
                strMessageDetail += "\r\n";
                strMessageDetail += ex.Message.ToString();
                strMessageDetail += "\r\n";
                strMessageDetail += "-----------------------------------";
                strMessageDetail += "\r\n";

                //frmMessageBox Message = new frmMessageBox();
                //Message.ShowDialog();
                if (IsBulkTrans) dbConnection.BulkTransHasErrors = true;
                Success = false;
                TicketReq = true;
            }

            return rowsAffected;
        }

        //public int ExecuteQueryThread(string strQuery, bool IsProcedure, SqlParameter[]? Parameters = null)
        //{
        //    int rowsAffected = 0;
        //    try
        //    {
        //        dbConnection.ExecuteQuery_Thread(strQuery);
        //        Success = true;
        //    }
        //    // Catch SqlExceptions and generate the appropraiate error message
        //    catch
        //    {
        //        //if (ex.Message.Contains("REFERENCE"))
        //        //{
        //        //    MessageBox.Show("This item is used in other locations", "Can't delete this item", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        //    Main.Success = false;
        //        //    return rowsAffected;
        //        //}
        //        //_SqlException = ex;
        //        //Language = Main.bol_Language;
        //        //strMessage = Error_Msg(ex);
        //        //strMessageDetail = strQuery;
        //        //strMessageDetail += "\r\n";
        //        //strMessageDetail += "-----------------------------------";
        //        //strMessageDetail += "\r\n";
        //        //strMessageDetail += " SqlException Number : " + ex.Number.ToString();
        //        //strMessageDetail += "\r\n";
        //        //strMessageDetail += "-----------------------------------";
        //        //strMessageDetail += "\r\n";
        //        //strMessageDetail += " SqlException Number : " + ex.Message.ToString();
        //        //strMessageDetail += "\r\n";
        //        //strMessageDetail += "-----------------------------------";

        //        //frmMessageBox Message = new frmMessageBox();
        //        //Message.ShowDialog();

        //        if (IsBulkTrans) dbConnection.BulkTransHasErrors = true;
        //        Success = false;
        //    }
        //    // Catch other exceptions
        //    //catch (Exception ex)
        //    //{
        //    //    //_Exception = ex;
        //    //    //Language = Main.bol_Language;
        //    //    //strMessage = Error_Msg();
        //    //    //strMessageDetail = strQuery;
        //    //    //strMessageDetail += "\r\n";
        //    //    //strMessageDetail += "-----------------------------------";
        //    //    //strMessageDetail += "\r\n";
        //    //    //strMessageDetail += ex.Message.ToString();
        //    //    //strMessageDetail += "\r\n";
        //    //    //strMessageDetail += "-----------------------------------";
        //    //    //strMessageDetail += "\r\n";

        //    //    //frmMessageBox Message = new frmMessageBox();
        //    //    //Message.ShowDialog();
        //    //    Main.Success = false;
        //    //}

        //    return rowsAffected;
        //}

        public string Error_Msg(SqlException e)
        {
            string myStr = "";

            // The error number 2627 idicates violating unique constraint
            if (e.Number == 2627 || e.Number == 2601)
                myStr = bol_Language ? " €Ì— „”„ÊÕ » ﬂ—«— «·»Ì«‰ " : "Cannot insert duplicate key";
            // The error number 547 idicates violating foreign key constarint
            else if (e.Number == 547)
                myStr = bol_Language ? " €Ì— „”„ÊÕ »«·Õ–› ·≈— »«ÿÂ »»Ì«‰«  √Œ—Ï " : "DELETE statement conflicted";
            // For other error numbers generate a message corresponding to the page mode
            else if (e.Number == 11001 || e.Message.Contains("Login fail") || e.Message.Contains("server was not found"))
            {
                myStr = bol_Language ? "  ⁄–— «·« ’«· »«·Œ«œ„ " : "Server Connection Error";
            }
            else
            {
                myStr = Error_Msg();
            }

            return myStr;
        }

        public string Error_Msg()
        {
            return "Execution Error (Support Required)";
        }

        public List<T> CreateListFromTable<T>(DataTable tbl) where T : new()
        {
            // define return list
            List<T> lst = new List<T>();

            // go through each row
            foreach (DataRow r in tbl.Rows)
            {
                // add to the list
                lst.Add(CreateItemFromRow<T>(r));
            }

            // return the list
            return lst;
        }
        public T CreateItemFromRow<T>(DataRow row) where T : new()
        {
            // create a new object
            T item = new T();

            // set the item
            SetItemFromRow(item, row);

            // return 
            return item;
        }
        public void SetItemFromRow<T>(T item, DataRow row) where T : new()
        {
            // go through each column
            foreach (DataColumn c in row.Table.Columns)
            {
                // find the property for the column
                PropertyInfo p = item.GetType().GetProperty(c.ColumnName);

                // if exists, set the value
                if (p != null && row[c] != DBNull.Value)
                {
                    p.SetValue(item, row[c], null);
                }
            }
        }
        public string ToXml<T>(T obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StringWriter sw = new StringWriter();
            using (XmlWriter writer = XmlWriter.Create(sw, new XmlWriterSettings { OmitXmlDeclaration = true }))
            {
                serializer.Serialize(writer, obj);
                return sw.ToString();
            }
        }
        #endregion
        public  DataTable SelectNext(string BranchIDs, string TableName, string IDColumn, string NoColumn, string DateColumn, string RowID, string WhereCondition, string IsNext)
        {
            return ExecuteQuery_DataTable(" SelectNext '" + BranchIDs + "','" + TableName + "','" + IDColumn + "','"
                                            + NoColumn + "','" + DateColumn + "','" + (RowID != "" ? RowID : "-1") + "','" + WhereCondition + "'," + IsNext,false,null);
        }
    }
}
