using ERPManagement.Application.Contracts.Infrastructure.Services;
using ERPManagement.Application.Shared.Enums;
using Microsoft.AspNetCore.Http;

namespace ERPManagement.Infrastructure.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LanguageService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Language GetLanguage()
        {
            // إذا كنت تريد استخدام CultureInfo الافتراضية للثريد الحالي
            //if (CultureInfo.DefaultThreadCurrentCulture != null && CultureInfo.DefaultThreadCurrentCulture.Name == "en-US")
            //{
            //    return Language.English;
            //}
            //return Language.Arabic;

            // استخدام رأس Accept-Language من الطلب لتحديد اللغة
            var languageHeader = _httpContextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString();

            if (string.IsNullOrEmpty(languageHeader))
                return Language.Arabic; // لغة افتراضية

            if (languageHeader.StartsWith("ar", StringComparison.OrdinalIgnoreCase))
                return Language.Arabic;

            return Language.English;



        }
        public string GetLanguageName()
        {

            return GetLanguage() == Language.Arabic ? "ar" : "en";
        }

        public string GetCurrentCulture()
        {
            return _httpContextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString() ?? "ar-EG";
        }

        public bool IsRightToLeft()
        {
            return GetLanguage() == Language.Arabic;
        }
    }
}
