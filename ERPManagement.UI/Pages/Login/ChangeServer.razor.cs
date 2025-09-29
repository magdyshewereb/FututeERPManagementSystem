using ERPManagement.UI.DataModels.ProtectedLocalStorage;

namespace ERPManagement.UI.Pages.Login
{
    public partial class ChangeServer
    {
        public string errorMessage = "";
        public string loading = "";
        bool isModalVisible;
        private ServerLoginData model { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.Response.HasStarted)
            {
                ServerLoginData result = await protectedLocalStorageService.GetServerDataAsync();
                if (result != null)
                {
                    model = result;
                }
            }
        }
        private void btnOkClick()
        {
            if (model.Server == "")
            {
                errorMessage = "Please Enter Server Name";//  "قم بإدخال الخادم";
                isModalVisible = true;
            }
            //else if (model.DBUser == "")
            //{
            //    errorMessage = "Please Enter User Name";//  "قم بإدخال الخادم";
            //    isModalVisible = true;
            //}
            else//Data Is Complete check for Connection
            {
                try
                {
                    loading = "fa fa-circle-o-notch fa-spin";
                    string conString = globalFunctions.CheckAndSetConnectionString(model.Server.Trim(), "Master", model.DBUser.Trim(), model.DBPassword);
                    loading = "";
                    if (conString != null)
                    {
                        protectedLocalStorageService.SetServerDataAsync(model);
                        protectedLocalStorageService.SetConnectionStringAsync(conString);
                        Nav.NavigateTo("/login", forceLoad: true);
                    }
                }
                catch (Exception ex)
                {
                    loading = "";
                    errorMessage = ex.Message;
                }

            }
        }
        private void Close()
        {
            isModalVisible = false;
        }
    }
}

