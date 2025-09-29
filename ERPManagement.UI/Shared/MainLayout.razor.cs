using ERPManagement.UI.DataModels.ProtectedLocalStorage;
using Microsoft.AspNetCore.Components;

namespace ERPManagement.UI.Shared
{
    public partial class MainLayout
    {
        bool isLogin = true;
        UserLoginData model;
        protected override async Task OnInitializedAsync()
        {
            if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.Response.HasStarted)
            {
                model = await protectedLocalStorageService.GetUserDataAsync();
                if (model == null)
                    NavigationManager.NavigateTo("/login", false);
            }
        }
        public void redirect()
        {
            NavigationManager.NavigateTo("/", false);
        }
    }
}
