using ERPManagement.UI.DataModels.ProtectedLocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Data;
using System.Security.Claims;

namespace ERPManagement.UI.GeneralClasses
{
    public class WebsiteAuthenticator : AuthenticationStateProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ProtectedLocalStorageService _protectedLocalStorageService;

        public WebsiteAuthenticator(IHttpContextAccessor httpContextAccessor, ProtectedLocalStorageService protectedLocalStorageService)
        {
            _httpContextAccessor = httpContextAccessor;
            _protectedLocalStorageService = protectedLocalStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var principal = new ClaimsPrincipal();
            if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.Response.HasStarted)
            {
                UserLoginData user = await _protectedLocalStorageService.GetUserDataAsync();
                if (user != null && !string.IsNullOrEmpty(user.UserID))
                {
                    var identity = await CreateIdentityFromUser(user);
                    principal = new(identity);
                }
            }
            return new AuthenticationState(principal);
        }

        private async Task<ClaimsIdentity> CreateIdentityFromUser(UserLoginData userIdentity)
        {
            try
            {
                var result = new ClaimsIdentity(
                new Claim[]
                {
                new (ClaimTypes.Name, userIdentity.Username)
                ////new (ClaimTypes.Hash, userIdentity.Password),
                //new (ClaimTypes.Sid, userIdentity.UserID)
                }, "Basic");
                List<string> roles = new List<string>();

                DataTable dtUserForms = await _protectedLocalStorageService.GetUserFormsAsync();
                if (dtUserForms != null && dtUserForms.Rows.Count > 0)
                {
                    roles = dtUserForms == null ? new List<string>() :
                        dtUserForms.Rows.OfType<DataRow>().Select(dr => (string)dr["FormFullName"]).ToList();
                    foreach (string role in roles)
                    {
                        result.AddClaim(new(ClaimTypes.Role, role));
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }



    }
}