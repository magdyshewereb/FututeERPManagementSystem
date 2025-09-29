using ERPManagement.UI.GeneralClasses;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ERPManagement.UI.DataAccess
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    public class DbConnection
    {
        #region Variables
        public bool BulkTransHasErrors;
        public string BulkTransName;
        public SqlTransaction myBulkTrans;
        public SqlConnection myBulkTransConnection;

        public SqlTransaction mySyncBulkTrans;
        public SqlConnection mySyncBulkTransConnection;

        public SqlConnection mySyncConnection;

        public SqlConnection myConnection;
        public SqlTransaction myTrans;
        public SqlCommand myCommand;

        public string Server;
        public string DataBase;
        public string UserID;
        public string Password;
        public string Connectionstring { get; set; } = "";
        //String MyException;
        #endregion
        //public DbConnection(string MyServer, string MyDataBase, string MyUserID, string MyPassword)
        public DbConnection(string MyConnectionstring)
        {
            //
            // TODO: Add constructor logic here
            //
            Connectionstring = MyConnectionstring;
            //DataBase = MyDataBase;
            //UserID = MyUserID;
            //Password = MyPassword;
            CreateConnection();
            CloseConnection();
        }
        public DbConnection(string MyServer, string MyDataBase, string MyUserID, string MyPassword)
        {
            //
            // TODO: Add constructor logic here
            //
            Server = MyServer;
            DataBase = MyDataBase;
            UserID = MyUserID;
            Password = MyPassword;
        }      
        public bool CreateConnection()
        {
            if (Connectionstring != "")
            {
                myConnection = new SqlConnection(Connectionstring);
                myBulkTransConnection = new SqlConnection(Connectionstring);


                try
                {

                    myConnection.Open();
                    myConnection.Close();
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
            else { return false; }
        }
        public void SyncCreateConnection()
        {
            mySyncConnection = new SqlConnection(Connectionstring);
            mySyncBulkTransConnection = new SqlConnection(Connectionstring);
            // try
            //{

            //    mySyncConnection.Open();
            //    mySyncConnection.Close();
            //    return true;
            //}
            //catch (SqlException)
            //{
            //    return false;
            //}
        }
        public void CloseConnection()
        {
            if (myConnection.State == ConnectionState.Open)
                myConnection.Close();
        }
        public void SyncOpenConnection()
        {
            SyncCreateConnection();
            if (mySyncConnection.State == ConnectionState.Closed)
                mySyncConnection.Open();
        }
        public void OpenConnection()
        {
            CreateConnection();
            if (myConnection.State == ConnectionState.Closed)
                myConnection.Open();
        }
        public void CloseConnection(bool TransState)
        {
            //if (TransState)
            //{
            //    myTrans.Commit();
            //}
            //else
            //{
            //    myTrans.Rollback();
            //}
            //if (myConnection.State == ConnectionState.Open)
            //    myConnection.Close();
        }
        public void SyncCloseConnection()
        {
            if (mySyncConnection.State == ConnectionState.Open)
                mySyncConnection.Close();
        }
        public void StartBulkTrans(bool FromServer)
        {
            Random rnd = new Random();
            BulkTransName = "BulkTrans" + rnd.Next(999999);
            if (FromServer)
            {
                SyncCreateConnection();

                if (mySyncBulkTransConnection.State == ConnectionState.Closed)
                    mySyncBulkTransConnection.Open();
                mySyncBulkTrans = mySyncBulkTransConnection.BeginTransaction(BulkTransName);
                SyncExecuteQuery_DataTableBulkTrans(" SET XACT_ABORT ON ; ");
            }
            else
            {
                if (myBulkTransConnection.State == ConnectionState.Closed)
                    myBulkTransConnection.Open();
                myBulkTrans = myBulkTransConnection.BeginTransaction(BulkTransName);
                ExecuteQuery_DataTableBulkTrans(" SET XACT_ABORT ON ; ");
            }
            //  ExecuteQuery_DataTableBulkTrans(" SET XACT_ABORT ON ; ");

            BulkTransHasErrors = false;
        }
        public void EndBulkTrans(bool FromServer)
        {
            if (FromServer)
            {
                if (BulkTransHasErrors)
                {
                    mySyncBulkTrans.Rollback(BulkTransName);
                    throw new ArgumentException("Bulk transaction Erorr", "original");
                }
                else
                {
                    mySyncBulkTrans.Commit();
                }
                if (mySyncBulkTransConnection.State == ConnectionState.Open)
                    mySyncBulkTransConnection.Close();
            }
            else
            {
                if (BulkTransHasErrors)
                {
                    myBulkTrans.Rollback(BulkTransName);
                    throw new ArgumentException("Bulk transaction Erorr", "original");
                }
                else
                {
                    myBulkTrans.Commit();
                }
                if (myBulkTransConnection.State == ConnectionState.Open)
                    myBulkTransConnection.Close();
            }

        }
        public void RollbackBulkTrans(bool FromServer)
        {
            try
            {
                if (FromServer)
                {
                    mySyncBulkTrans.Rollback(BulkTransName);

                    if (mySyncBulkTransConnection.State == ConnectionState.Open)
                        mySyncBulkTransConnection.Close();
                }
                else
                {
                    myBulkTrans.Rollback(BulkTransName);

                    if (myBulkTransConnection.State == ConnectionState.Open)
                        myBulkTransConnection.Close();
                }
            }
            catch
            {
                if (FromServer)
                {
                    if (mySyncBulkTransConnection.State == ConnectionState.Open)
                        mySyncBulkTransConnection.Close();
                }
                else
                {
                    if (myBulkTransConnection.State == ConnectionState.Open)
                        myBulkTransConnection.Close();

                }
            }
        }


        public SqlDataReader ExecuteQuery(string mySelectQuery)
        {
            ConnectionState OriginalConnectionStatus = myConnection.State;
            SqlDataReader myReader;
            SqlCommand myCommand = new SqlCommand(mySelectQuery, myConnection);
            //Open Connenction
            OpenConnection();
            //Create Command
            myReader = myCommand.ExecuteReader();
            //Close Connection
            CloseConnection();
            //return myReader;
            return myReader;
        }
        public int ExecuteQueryBulkTrans(string strProcName, bool IsProcedure, SqlParameter[]? Parameters)
        {
            // Start a transaction
            try
            {
                myCommand = new SqlCommand(strProcName, myBulkTransConnection, myBulkTrans);
                myCommand.CommandTimeout = 0;
                if(IsProcedure)
                myCommand.CommandType = CommandType.StoredProcedure;
                if (Parameters != null && Parameters.Length > 0)
                    myCommand.Parameters.AddRange(Parameters);
                int rowsAffected = myCommand.ExecuteNonQuery();
                return rowsAffected;
            }
            catch (Exception)
            {
                BulkTransHasErrors = true;
                throw;
            }
        }
        public int SyncExecuteQueryBulkTrans(string myProcName, bool IsProcedure, SqlParameter[]? Parameters)
        {
            // Start a transaction
            try
            {
                myCommand = new SqlCommand(myProcName, mySyncBulkTransConnection, mySyncBulkTrans);
                myCommand.CommandTimeout = 0;
                if (IsProcedure)
                    myCommand.CommandType = CommandType.StoredProcedure;
                if (Parameters != null && Parameters.Length > 0)
                    myCommand.Parameters.AddRange(Parameters);
                int rowsAffected = myCommand.ExecuteNonQuery();
                return rowsAffected;
            }
            catch (Exception)
            {
                BulkTransHasErrors = true;
                throw;
            }
        }
        public DataTable SyncExecuteQuery_DataTableBulkTrans(string mySelectQuery, int TimeOut = 0)
        {
            DataTable dataTable = new DataTable();
            SqlDataReader SDRResult;
            // Start a transaction
            try
            {
                myCommand = new SqlCommand(mySelectQuery, mySyncBulkTransConnection, mySyncBulkTrans);
                myCommand.CommandTimeout = TimeOut;
                SDRResult = myCommand.ExecuteReader();

                for (int intIndex = 0; intIndex < SDRResult.FieldCount; intIndex++)
                {
                    // create and add a column
                    dataTable.Columns.Add(SDRResult.GetName(intIndex), SDRResult.GetFieldType(intIndex));
                }
                while (SDRResult.Read())
                {
                    // add a row
                    DataRow row = dataTable.NewRow();
                    for (int intIndex = 0; intIndex < SDRResult.FieldCount; intIndex++)
                    {
                        row[SDRResult.GetName(intIndex)] = SDRResult[SDRResult.GetName(intIndex)];
                    }
                    dataTable.Rows.Add(row);
                }
                //Close Reader
                SDRResult.Close();
            }
            catch (Exception)
            {
                BulkTransHasErrors = true;
                throw;
            }
            //Return Result of Executed Query
            dataTable.AcceptChanges();
            return dataTable;
        }

        //public int SyncExecuteNonQuery(string mySelectQuery)
        //{
        //    SqlCommand myCommand = new SqlCommand(mySelectQuery, new SqlConnection(GlobalVariables.SyncConnectionString));
        //    myCommand.CommandTimeout = 0;
        //    //Open Connenction
        //    myCommand.Connection.Open();
        //    //Create Command
        //    int rowsAffected = myCommand.ExecuteNonQuery();
        //    //Close Connection
        //    myCommand.Connection.Close();
        //    return rowsAffected;
        //}
        public int SyncExecuteNonQuery(string strProcName, bool IsProcedure, SqlParameter[]? Parameters)
        {
            SyncOpenConnection();
            SqlCommand myCommand = new SqlCommand(strProcName,mySyncConnection);
            myCommand.CommandTimeout = 0;
            if (IsProcedure)
                myCommand.CommandType = CommandType.StoredProcedure;
            if (Parameters != null && Parameters.Length > 0)
                myCommand.Parameters.AddRange(Parameters);
            //Open Connenction
            myCommand.Connection.Open();
            int rowsAffected = myCommand.ExecuteNonQuery();

            //Close Connection
            myCommand.Connection.Close();
            return rowsAffected;
        }
        public int ExecuteNonQuery(string mySelectQuery)
        {
            SqlCommand myCommand = new SqlCommand(mySelectQuery, myConnection);
            myCommand.CommandTimeout = 0;
            //Open Connenction
            OpenConnection();
            //Create Command
            int rowsAffected = myCommand.ExecuteNonQuery();
            //Close Connection
            CloseConnection();

            return rowsAffected;
        }
        public int ExecuteNonQueryBulkTrans(string mySelectQuery)
        {
            int rowsAffected = 0;
            try
            {
                myCommand = new SqlCommand(mySelectQuery, myBulkTransConnection, myBulkTrans);
                myCommand.CommandTimeout = 0;
                rowsAffected = myCommand.ExecuteNonQuery();
            }
            catch (Exception)
            {
                BulkTransHasErrors = true;
                throw;
            }
            return rowsAffected;
        }
        public int ExecuteNonQuery(string strProcName, bool IsProcedure, SqlParameter[]? Parameters)
        {
            OpenConnection();
            SqlCommand myCommand = new SqlCommand(strProcName, myConnection);
            myCommand.CommandTimeout = 0;
            if (IsProcedure)
                myCommand.CommandType = CommandType.StoredProcedure;
            if (Parameters != null && Parameters.Length > 0)
                myCommand.Parameters.AddRange(Parameters);
            //Open Connenction
            
            //Create Command
            int rowsAffected = myCommand.ExecuteNonQuery();
            //Close Connection
            CloseConnection();

            return rowsAffected;
        }

        public DataTable SyncExecuteQuery_DataTable(string mySelectQuery, bool IsProcedure, SqlParameter[]? Parameters, int TimeOut = 0)
        {
            DataTable dataTable = new DataTable();
            SqlDataReader SDRResult;
            //Open Connenction
            SyncCreateConnection();
            SyncOpenConnection();
            SqlCommand myCommand = new SqlCommand(mySelectQuery, mySyncConnection);
            myCommand.CommandTimeout = TimeOut;
            if (IsProcedure)
                myCommand.CommandType = CommandType.StoredProcedure;
            if (Parameters != null && Parameters.Length > 0)
                myCommand.Parameters.AddRange(Parameters);

            SDRResult = myCommand.ExecuteReader();
            for (int intIndex = 0; intIndex < SDRResult.FieldCount; intIndex++)
            {
                // create and add a column
                //dataTable.Columns.Add(Create_Column(SDRResult.GetName(intIndex), SDRResult.GetDataTypeName(intIndex)));
                dataTable.Columns.Add(SDRResult.GetName(intIndex), SDRResult.GetFieldType(intIndex));
            }
            while (SDRResult.Read())
            {
                // add a row
                DataRow row = dataTable.NewRow();
                for (int intIndex = 0; intIndex < SDRResult.FieldCount; intIndex++)
                {
                    row[SDRResult.GetName(intIndex)] = SDRResult[SDRResult.GetName(intIndex)];
                }
                dataTable.Rows.Add(row);
            }
            //Close Reader
            SDRResult.Close();
            //Close Connection
            SyncCloseConnection();
            //Return Result of Executed Query
            dataTable.AcceptChanges();
            return dataTable;
        }
        public DataTable SyncExecuteQuery_DataTableBulkTrans(string mySelectQuery, bool IsProcedure, SqlParameter[]? Parameters, int TimeOut = 0)
        {
            DataTable dataTable = new DataTable();
            SqlDataReader SDRResult;
            // Start a transaction
            try
            {
                myCommand = new SqlCommand(mySelectQuery, mySyncBulkTransConnection, mySyncBulkTrans);
                myCommand.CommandTimeout = TimeOut;
                if (IsProcedure)
                    myCommand.CommandType = CommandType.StoredProcedure;
                if (Parameters != null && Parameters.Length > 0)
                    myCommand.Parameters.AddRange(Parameters);

                SDRResult = myCommand.ExecuteReader();

                for (int intIndex = 0; intIndex < SDRResult.FieldCount; intIndex++)
                {
                    // create and add a column
                    dataTable.Columns.Add(SDRResult.GetName(intIndex), SDRResult.GetFieldType(intIndex));
                }
                while (SDRResult.Read())
                {
                    // add a row
                    DataRow row = dataTable.NewRow();
                    for (int intIndex = 0; intIndex < SDRResult.FieldCount; intIndex++)
                    {
                        row[SDRResult.GetName(intIndex)] = SDRResult[SDRResult.GetName(intIndex)];
                    }
                    dataTable.Rows.Add(row);
                }
                //Close Reader
                SDRResult.Close();
            }
            catch (Exception)
            {
                BulkTransHasErrors = true;
                throw;
            }
            //Return Result of Executed Query
            dataTable.AcceptChanges();
            return dataTable;
        }
        public DataTable ExecuteQuery_DataTable(string strProcName, bool IsProcedure, SqlParameter[]? Parameters)
        {
            DataTable dataTable = new DataTable();
            SqlDataReader SDRResult;
            //Open Connenction
            OpenConnection();

            try
            {
                myTrans = myConnection.BeginTransaction();
                SqlCommand myCommand = new SqlCommand(strProcName, myConnection, myTrans);
                myCommand.CommandTimeout = 0;

                if (IsProcedure)
                    myCommand.CommandType = CommandType.StoredProcedure;

                if (Parameters != null && Parameters.Length > 0)
                    myCommand.Parameters.AddRange(Parameters);

                SDRResult = myCommand.ExecuteReader();
                for (int intIndex = 0; intIndex < SDRResult.FieldCount; intIndex++)
                {
                    // create and add a column
                    //dataTable.Columns.Add(Create_Column(SDRResult.GetName(intIndex), SDRResult.GetDataTypeName(intIndex)));
                    dataTable.Columns.Add(SDRResult.GetName(intIndex), SDRResult.GetFieldType(intIndex));
                }
                while (SDRResult.Read())
                {
                    // add a row
                    DataRow row = dataTable.NewRow();
                    for (int intIndex = 0; intIndex < SDRResult.FieldCount; intIndex++)
                    {
                        row[SDRResult.GetName(intIndex)] = SDRResult[SDRResult.GetName(intIndex)];
                    }
                    dataTable.Rows.Add(row);
                }
                //Close Reader
                SDRResult.Close();
                //Close Connection
                myTrans.Commit();
            }
            catch (Exception ex)
            {

                myTrans.Rollback();
                throw;
            }

            //Return Result of Executed Query
            CloseConnection();
            dataTable.AcceptChanges();
            return dataTable;
        }
        public DataTable ExecuteQuery_DataTableBulkTrans(string mySelectQuery, bool IsProcedure, SqlParameter[]? Parameters)
        {
            DataTable dataTable = new DataTable();
            SqlDataReader SDRResult;
            // Start a transaction
            try
            {
                myCommand = new SqlCommand(mySelectQuery, myBulkTransConnection, myBulkTrans);
                myCommand.CommandTimeout = 0;
                if (IsProcedure)
                    myCommand.CommandType = CommandType.StoredProcedure;
                if (Parameters != null && Parameters.Length > 0)
                    myCommand.Parameters.AddRange(Parameters);

                SDRResult = myCommand.ExecuteReader();

                for (int intIndex = 0; intIndex < SDRResult.FieldCount; intIndex++)
                {
                    // create and add a column
                    dataTable.Columns.Add(SDRResult.GetName(intIndex), SDRResult.GetFieldType(intIndex));
                }
                while (SDRResult.Read())
                {
                    // add a row
                    DataRow row = dataTable.NewRow();
                    for (int intIndex = 0; intIndex < SDRResult.FieldCount; intIndex++)
                    {
                        row[SDRResult.GetName(intIndex)] = SDRResult[SDRResult.GetName(intIndex)];
                    }
                    dataTable.Rows.Add(row);
                }
                //Close Reader
                SDRResult.Close();
            }
            catch (Exception)
            {
                BulkTransHasErrors = true;
                throw;
            }
            //Return Result of Executed Query
            dataTable.AcceptChanges();
            return dataTable;
        }
        public DataTable ExecuteQuery_DataTableBulkTrans(string mySelectQuery)
        {
            DataTable dataTable = new DataTable();
            SqlDataReader SDRResult;
            // Start a transaction
            try
            {
                myCommand = new SqlCommand(mySelectQuery, myBulkTransConnection, myBulkTrans);
                myCommand.CommandTimeout = 0;
                SDRResult = myCommand.ExecuteReader();

                for (int intIndex = 0; intIndex < SDRResult.FieldCount; intIndex++)
                {
                    // create and add a column
                    dataTable.Columns.Add(SDRResult.GetName(intIndex), SDRResult.GetFieldType(intIndex));
                }
                while (SDRResult.Read())
                {
                    // add a row
                    DataRow row = dataTable.NewRow();
                    for (int intIndex = 0; intIndex < SDRResult.FieldCount; intIndex++)
                    {
                        row[SDRResult.GetName(intIndex)] = SDRResult[SDRResult.GetName(intIndex)];
                    }
                    dataTable.Rows.Add(row);
                }
                //Close Reader
                SDRResult.Close();
            }
            catch (Exception)
            {
                BulkTransHasErrors = true;
                throw;
            }
            //Return Result of Executed Query
            dataTable.AcceptChanges();
            return dataTable;
        }


        //public DataTable ExecuteQuery_Thread(string mySelectQuery)
        //{
        //    DataTable dataTable = new DataTable();
        //    SqlDataReader SDRResult;
        //    SqlConnection Conn = new SqlConnection(GlobalVariables.ConnectionString);
        //    Conn.Open();
        //    SqlCommand Com = new SqlCommand(mySelectQuery, Conn);
        //    Com.CommandTimeout = 0;
        //    SDRResult = Com.ExecuteReader();

        //    for (int intIndex = 0; intIndex < SDRResult.FieldCount; intIndex++)
        //    {
        //        // create and add a column
        //        dataTable.Columns.Add(SDRResult.GetName(intIndex), SDRResult.GetFieldType(intIndex));
        //    }
        //    while (SDRResult.Read())
        //    {
        //        // add a row
        //        DataRow row = dataTable.NewRow();
        //        for (int intIndex = 0; intIndex < SDRResult.FieldCount; intIndex++)
        //        {
        //            row[SDRResult.GetName(intIndex)] = SDRResult[SDRResult.GetName(intIndex)];
        //        }
        //        dataTable.Rows.Add(row);
        //    }
        //    SDRResult.Close();
        //    Conn.Close();
        //    Conn = null;
        //    dataTable.AcceptChanges();
        //    return dataTable;
        //}


    }
}
