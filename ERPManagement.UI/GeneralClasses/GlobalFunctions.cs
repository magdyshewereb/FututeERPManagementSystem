using ERPManagement.UI.DataAccess;
using ERPManagement.UI.DataModels.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace ERPManagement.UI.GeneralClasses
{

    public class GlobalFunctions : IScopedService
    {
        ProtectedLocalStorageService protectedLocalStorageService;
        public GlobalFunctions(ProtectedLocalStorageService _protectedLocalStorageService)
        {
            protectedLocalStorageService = _protectedLocalStorageService;
        }

        public async Task<Main> GetMain(bool isFromServer)
        {
            string connectionString;
            if (isFromServer)
                connectionString = await protectedLocalStorageService.GetSyncConnectionStringAsync();
            else connectionString = await protectedLocalStorageService.GetConnectionStringAsync();

           return  new Main(connectionString);             
        }
        public string CheckAndSetConnectionString(string Server, string DataBase, string UserID, string DBPassword)
        {
            string errorMessage = "";
            string connectionString;
            if (Server.IndexOf('.') > 0 || Server.IndexOf(',') > 0 || Server.IndexOf('\\') > 0)
            {
                connectionString = " Server= " + Server + " ;TrustServerCertificate=True; Database= " + DataBase + " ;User Id= " + UserID + " ; Password = " + DBPassword + ";Connect Timeout=30";
            }
            else
            {
                connectionString = " Server= np:" + Server + " ; TrustServerCertificate=True; Database= " + DataBase + " ;User Id= " + UserID + " ; Password = " + DBPassword + ";Connect Timeout=30";
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    connection.Close();
                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.ToLower().Contains("login failed"))
                {
                    errorMessage = "Connection failed:DataBase Username or password may be incorrect.";
                }
                else if (ex.Message.ToLower().Contains("cannot connect to server"))
                {
                    errorMessage = "Connection failed:Server name or network connectivity may be the issue.";
                }
                else { errorMessage =ex.Message; }
                connectionString = null;
                throw new Exception(errorMessage);
                
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                connectionString = null;
                throw new Exception(errorMessage);
            }
            return connectionString;
        }
        public List<T> GetListFromDataTable<T> (string strQuery, Main main) where T : new()
        {
            //MessageBox.Show(strQuery);
            DataTable dataTable = new DataTable();
            try
            {
                dataTable = main.ExecuteQuery_DataTable(strQuery, false, null);
            }
            // Catch SqlExceptions and generate the appropraiate error message
            catch
            {

            }
            List<T> lst = new List<T>();
            if (dataTable != null)
            {
                lst = main.CreateListFromTable<T>(dataTable);
            }
            return lst;
        }
        public void UpdateTable(string strQuery, Main main, bool IsFromServer)
        {
            //MessageBox.Show(strQuery);
            if (IsFromServer)
            {
                main.SyncExecuteQuery_DataTable(strQuery, false, null);
            }
            else
            {
                main.ExecuteQuery_DataTable(strQuery, false, null);
            }

        }
        #region reflection methodes to get your property type, propert value and also set property value 
        public string GetPropertyValue<T>(T item, string Property)
        {

            if (item != null)
            {
                return item.GetType().GetProperty(Property).GetValue(item, null)==null?null: item.GetType().GetProperty(Property).GetValue(item, null).ToString();
            }
            return "";
        }

        public void SetPropertyValue<T>(T item, string Property, object value)
        {
            if (item != null)
            {
                item.GetType().GetProperty(Property).SetValue(item, value);
            }
        }

        public string GetPropertyType<T>(T item, string Property)
        {

            if (item != null)
            {
                return item.GetType().GetProperty(Property).PropertyType.Name;
            }
            return null;
        }
        #endregion
        //public void CheckForNumbers(UltraGridCell activeCell, System.Windows.Forms.KeyPressEventArgs e)
        //{
        //    if (e.KeyChar == 3 || e.KeyChar == 22)//copy || paste
        //    {
        //        return;
        //    }
        //    string str = ".0123456789";
        //    // The 8 charachter is the backspace <-
        //    if (str.IndexOf(e.KeyChar) == -1 && e.KeyChar != 8)
        //    {
        //        e.Handled = true;
        //    }
        //    if (activeCell != null && ((UltraGridCell)activeCell).Text.Contains(".") && e.KeyChar != 8 && e.KeyChar == 46)
        //    {
        //        e.Handled = true;
        //    }
        //}
    }
}
