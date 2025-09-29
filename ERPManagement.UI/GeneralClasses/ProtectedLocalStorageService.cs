using ERPManagement.UI.DataModels.Defaults;
using ERPManagement.UI.DataModels.Interfaces;
using ERPManagement.UI.DataModels.ProtectedLocalStorage;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json;
using System.Data;

namespace ERPManagement.UI.GeneralClasses
{
    public class ProtectedLocalStorageService : IScopedService
    {
        private readonly ProtectedLocalStorage _protectedLocalStorage;

        public ProtectedLocalStorageService(ProtectedLocalStorage protectedLocalStorage)
        {
            _protectedLocalStorage = protectedLocalStorage;
        }

        public async void SetSyncConnectionStringAsync(string value)
        {
            await _protectedLocalStorage.SetAsync("SyncConnectionString", value);
        }
        public async Task<string> GetSyncConnectionStringAsync()
        {
            var result = await _protectedLocalStorage.GetAsync<string>("SyncConnectionString");
            if (result.Success && result.Value != null)
            {
                return result.Value;
            }
            else { return null; }
        }

        public async void SetConnectionStringAsync(string value)
        {
            await _protectedLocalStorage.SetAsync("ConnectionString", value);
        }
        public async Task<string> GetConnectionStringAsync()
        {
            var result = await _protectedLocalStorage.GetAsync<string>("ConnectionString");
            if (result.Success && result.Value != null)
            {
                return result.Value;
            }
            else { return null; }
        }
       
        public async void SetSyncConnectionAsync(SyncConnection value)
        {
            await _protectedLocalStorage.SetAsync("SyncConnection", JsonConvert.SerializeObject(value));
        }
        public async Task<SyncConnection> GetSyncConnectionAsync()
        {
            var result = await _protectedLocalStorage.GetAsync<string>("SyncConnection");
            if (result.Success && result.Value != null)
            {
                return JsonConvert.DeserializeObject<SyncConnection>(result.Value);
            }
            else { return null; }
        }
        public async void SetSystemSettingsAsync(SystemSettings value)
        {
            await _protectedLocalStorage.SetAsync("SystemSettings", JsonConvert.SerializeObject(value));
        }
        public async Task<SystemSettings> GetSystemSettingsAsync()
        {
            var result = await _protectedLocalStorage.GetAsync<string>("SystemSettings");
            if (result.Success && result.Value != null)
            {
                return JsonConvert.DeserializeObject<SystemSettings>(result.Value);
            }
            else { return null; }
        }
        public async void SetServerDataAsync(ServerLoginData value)
        {
            await _protectedLocalStorage.SetAsync("ServerData", JsonConvert.SerializeObject(value));
        }
        public async Task<ServerLoginData> GetServerDataAsync()
        {
            var result = await _protectedLocalStorage.GetAsync<string>("ServerData");
            if (result.Success && result.Value != null)
            {
                return JsonConvert.DeserializeObject<ServerLoginData>(result.Value);
            }
            else { return null; }
        }
        public async void SetUserDataAsync(UserLoginData value)
        {
            string jsonWithNulls = JsonConvert.SerializeObject(value, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            //string xx = JsonConvert.SerializeObject(value);
            await _protectedLocalStorage.SetAsync("UserData", jsonWithNulls);
        }
        public async Task<UserLoginData> GetUserDataAsync()
        {
            var result = await _protectedLocalStorage.GetAsync<string>("UserData");
            if (result.Success && result.Value != null)
            {
                return JsonConvert.DeserializeObject<UserLoginData>(result.Value);
            }
            else { return null; }
        }

        public async void SetUserFormsAsync(DataTable value)
        {
            string jsonlst = value.Rows.Count > 0 ? JsonConvert.SerializeObject(value) : "[]";
            await _protectedLocalStorage.SetAsync("UserForms", jsonlst);
        }
        public async Task<DataTable> GetUserFormsAsync()
        {
            try
            {
                var result = await _protectedLocalStorage.GetAsync<string>("UserForms");// await _protectedLocalStorage.GetAsync<string>("UserForms");
                if (result.Success && result.Value != null)
                {
                    return (DataTable)JsonConvert.DeserializeObject(result.Value, typeof(DataTable));
                }
                else { return null; }

            }
            catch (Exception ex)
            {
                throw;
                return null;
            }

        }
        public async void SetUserFormsFunctionsAsync(DataTable value)
        {
            string jsonlst = value.Rows.Count > 0 ? JsonConvert.SerializeObject(value) : "[]";
            await _protectedLocalStorage.SetAsync("UserFormsFunctions", jsonlst);
        }
        public async Task<DataTable> GetUserFormsFunctions()
        {
            try
            {
                var result = await _protectedLocalStorage.GetAsync<string>("UserFormsFunctions");// await _protectedLocalStorage.GetAsync<string>("UserFormsFunctions");
                if (result.Success && result.Value != null)
                {
                    return (DataTable)JsonConvert.DeserializeObject(result.Value, typeof(DataTable));
                }
                else { return null; }

            }
            catch (Exception ex)
            {
                throw;
                return null;
            }

        }
        public async void DeleteUserFormsAsync()
        {
            await _protectedLocalStorage.DeleteAsync("UserForms");
        }
    }
}
