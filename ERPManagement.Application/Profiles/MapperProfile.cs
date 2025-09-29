using AutoMapper;
using ERPManagement.Application.Shared.Enums;
using System.Globalization;

namespace RakezIntelliERP.Account.Application.Profiles
{
	public partial class MapperProfile : Profile
    {

        public MapperProfile()
        {
            // شغّل كل الميثودز اللي بتعمل مابينج هنا
            UsersMapping();
            UserRolesMapping(); // ← أضف السطر ده
        }

        public Language GetLanguage()
        {
            if (CultureInfo.DefaultThreadCurrentCulture != null && CultureInfo.DefaultThreadCurrentCulture.Name == "en-US")
            {
                return Language.English;
            }
            return Language.Arabic;

        }
    }
}
