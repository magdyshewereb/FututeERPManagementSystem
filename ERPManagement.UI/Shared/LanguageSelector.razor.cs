using ERPManagement.UI.DataModels.ProtectedLocalStorage;
using System.Globalization;

namespace ERPManagement.UI.Shared
{
    public partial class LanguageSelector
    {
        private SystemSettings systemSettings { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            //Culture = CultureInfo.CurrentCulture;
            systemSettings = await protectedLocalStorageService.GetSystemSettingsAsync();
            if (systemSettings != null)
            {
                if (systemSettings.Language == "English")
                    UICulture = new CultureInfo("En");
                else
                    UICulture = new CultureInfo("Ar");
            }
        }

        private CultureInfo UICulture
        {
            get => CultureInfo.CurrentUICulture;
            set
            {
                if (CultureInfo.CurrentUICulture.Name != value.Name)
                {
                    var uri = new Uri(Nav.Uri).GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped);
                    var cultureEscaped = Uri.EscapeDataString(value.Name);
                    var uriEscaped = Uri.EscapeDataString(uri);
                    systemSettings.Language = value.EnglishName;
                    systemSettings.IsArabic = systemSettings.Language == "Arabic";
                    new Task(async () =>
                    {
                        protectedLocalStorageService.SetSystemSettingsAsync(systemSettings);
                    }).Start();

                    Nav.NavigateTo($"Culture/Set?uiculture={cultureEscaped}&redirectUri={uriEscaped}", forceLoad: true);
                }
            }
        }
    }
}
