using ERPManagement.UI.DataAccess;
using ERPManagement.UI.DataModels.Defaults;
using ERPManagement.UI.DataModels.General;
using ERPManagement.UI.DataModels.ProtectedLocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Data;

namespace ERPManagement.UI.Pages.Login
{
    public partial class Login
    {
        private bool isDarkMode = false;

        [Inject] private IJSRuntime JS { get; set; }

        private async Task ToggleTheme()
        {
            isDarkMode = !isDarkMode;
            var module = await JS.InvokeAsync<IJSObjectReference>("import", "/js/theme.js");
            await module.InvokeVoidAsync("setTheme", isDarkMode);
        }

        [Parameter]
        public bool isLogin { get; set; }

        bool showBranchesDialog = false;
        bool isModalVisible = false;
        string errorMessage = "";
        string userBranchIDs = "";
        string connectionString;

        public Dictionary<string, string> databases { get; set; }
        public List<Branch> branches { get; set; }
        private UserLoginData userModel { get; set; } = new();
        private SystemSettings systemSettings { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.Response.HasStarted)
            {
                if (isLogin)
                {
                    LogoutAsync();
                }
                else
                {

                    connectionString = await protectedLocalStorageService.GetConnectionStringAsync();
                    if (connectionString == null)
                    {
                        navigationManager.NavigateTo("/ChangeServer", false);
                    }
                    else
                    {
                        UserLoginData user = await protectedLocalStorageService.GetUserDataAsync();
                        systemSettings = await protectedLocalStorageService.GetSystemSettingsAsync();
                        if (systemSettings == null) systemSettings = new SystemSettings();
                        databases = GetDatabases(new Main(connectionString));
                        if (user != null)
                        {
                            userModel = user;
                        }
                        else
                        {
                            if (databases != null && databases.Count > 0)
                                userModel.DataBaseName = databases.First().Value;
                        }
                    }
                }
            }
        }
        private async Task btnLoginClick()
        {
            /*
             * 0-check that the user enters all the needed fields
             * 1-check that the connection is valid
             * 2-check the user is valid
             * 3-get branches for this company
             */

            if (string.IsNullOrEmpty(userModel.DataBaseName))
            {
                ShowAlert("Please select company");
            }
            else if (string.IsNullOrEmpty(systemSettings.Language))
            {
                ShowAlert("Please select language");
            }
            else if (string.IsNullOrEmpty(userModel.Username) && string.IsNullOrEmpty(userModel.Password))//In case of admin the user could be empty and in any other user the password could be empty but can't both be empty.
            {
                ShowAlert("Please enter your username & password");
            }
            //else if (model.Password == null)
            //{
            //    errorMessage = "Please enter your password";
            //    isModalVisible = true;
            //}
            if (!isModalVisible)
            {
                ServerLoginData serverData = await protectedLocalStorageService.GetServerDataAsync();
                try
                {
                    //"Server=.;Database=ERP_SS;User Id=sa;Password=magdy1986;TrustServerCertificate=True;";// 
                    //"Server= np:Rayan ; TrustServerCertificate=True; Database= SS ;User Id= sa ; Password = magdy1986;Connect Timeout=30//
                    connectionString = globalFunctions.CheckAndSetConnectionString(serverData.Server, userModel.DataBaseName, serverData.DBUser, serverData.DBPassword);
                    protectedLocalStorageService.SetConnectionStringAsync(connectionString);
                    protectedLocalStorageService.SetSystemSettingsAsync(systemSettings);
                    if (connectionString != null)
                    {
                        var user = userService.CheckUser(userModel.Username, userModel.Password == null ? "" : userModel.Password, new Main(connectionString));
                        if (user != null)
                        {
                            userBranchIDs = GetBranchIds(user.User_ID.ToString(), new Main(connectionString));
                            if (userBranchIDs != "")
                            {
                                DataRow drCompany = branchService.GetCompanyData("Defaults", "CompanyNameE, CompanyNameA, CurrencyID,TicketingHost,TicketingHost2", "", new Main(connectionString)).Rows[0];
                                if (drCompany != null)
                                {
                                    userModel.CompanyName = systemSettings.IsArabic ? drCompany["CompanyNameA"].ToString() : drCompany["CompanyNameE"].ToString();
                                }
                                SyncConnectionService syncConnection = new SyncConnectionService();
                                List<SyncConnection> syncConnections = syncConnection.Select(-1, "-1", true, new Main(connectionString));
                                if (syncConnections.Count > 0) protectedLocalStorageService.SetSyncConnectionAsync(syncConnections[0]);
                                branches = branchService.Select(-1, userBranchIDs, systemSettings.IsArabic ? 1 : 0, new Main(connectionString));

                                if (branches != null && branches.Count > 0)
                                {
                                    userModel.BranchID = branches[0].BranchID.ToString();
                                    if (branches.Count > 1)
                                    {
                                        userModel.UserID = user.User_ID.ToString();
                                        showBranchesDialog = true;
                                    }
                                    else if (branches.Count == 1)
                                    {
                                        userModel.BranchName = systemSettings.IsArabic ? branches[0].BranchNameAr.ToString() : branches[0].BranchNameEn.ToString();
                                        userModel.UserID = user.User_ID.ToString();
                                        if (!userModel.Remember)
                                            userModel.Password = "";
                                        protectedLocalStorageService.SetUserDataAsync(userModel);
                                        DataTable dtUserForms = userFormService.SelectForms(user.User_ID, new Main(connectionString));
                                        DataTable dtUserFormsFunctions = userFormFunctionsService.SelectFunctions(user.User_ID, new Main(connectionString));
                                        protectedLocalStorageService.SetUserFormsAsync(dtUserForms);
                                        protectedLocalStorageService.SetUserFormsFunctionsAsync(dtUserFormsFunctions);
                                        navigationManager.NavigateTo("/index", forceLoad: true);
                                    }
                                }
                                else
                                {
                                    ShowAlert("There are no assigned branches for this user");
                                }
                            }
                            else
                            {
                                ShowAlert("There are no assigned branches for this user");
                            }

                        }
                        else
                        {
                            ShowAlert("InValid username or password");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowAlert(ex.Message);
                }

            }
        }
        private void btnOkClick()
        {
            if (!userModel.Remember)
                userModel.Password = "";
            if (!string.IsNullOrEmpty(userModel.BranchID))
            {
                userModel.BranchName = systemSettings.IsArabic ? branches.Where(b => b.BranchID == int.Parse(userModel.BranchID.ToString())).ToList()[0].BranchNameAr
                                                    : branches.Where(b => b.BranchID == int.Parse(userModel.BranchID.ToString())).ToList()[0].BranchNameEn;

                protectedLocalStorageService.SetUserDataAsync(userModel);
                DataTable dtUserForms = userFormService.SelectForms(int.Parse(userModel.UserID), new Main(connectionString));
                DataTable dtUserFormsFunctions = userFormFunctionsService.SelectFunctions(int.Parse(userModel.UserID), new Main(connectionString));
                protectedLocalStorageService.SetUserFormsAsync(dtUserForms);
                protectedLocalStorageService.SetUserFormsFunctionsAsync(dtUserFormsFunctions);
                navigationManager.NavigateTo("/index", forceLoad: true);
            }
            else
            {
                ShowAlert("You have to select branch to continue.");
            }
        }
        private void btnCloseDialogClick()
        {
            showBranchesDialog = false;
        }
        private void ChangeLanguage()
        {
            if (userModel != null)
                if (systemSettings.Language != null) { systemSettings.IsArabic = systemSettings.Language == "Arabic"; }
        }
        public Dictionary<string, string> GetDatabases(Main main)
        {
            Dictionary<string, string> result = userService.GetDatabases(main);
            return result;
        }
        public string GetBranchIds(string userID, Main main)
        {
            var result = usersBranchesService.SelectUserBrancheIDs(userID, main);
            return result;
        }
        private void ShowAlert(string alertMessage)
        {
            errorMessage = alertMessage;
            isModalVisible = true;
        }
        public async void LogoutAsync()
        {
            protectedLocalStorageService.DeleteUserFormsAsync();
            UserLoginData user = await protectedLocalStorageService.GetUserDataAsync();
            if (user != null)
            {
                user.Password = "";
                user.UserID = "";
                user.Username = "";
                protectedLocalStorageService.SetUserDataAsync(user);
                connectionString = await protectedLocalStorageService.GetConnectionStringAsync();
                if (string.IsNullOrEmpty(connectionString))
                {
                    navigationManager.NavigateTo("/ChangeServer", false);
                }
                else
                {
                    databases = GetDatabases(new Main(connectionString));
                    userModel = user;
                    StateHasChanged();
                }
            }
        }
        private void Close()
        {
            isModalVisible = false;
        }
    }
}