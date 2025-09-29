using ERPManagement.Application.Shared.Enums;

namespace ERPManagement.Application.Contracts.Infrastructure.Services
{
	public interface ILanguageService
	{
		Language GetLanguage();
		string GetLanguageName();
		string GetCurrentCulture(); // اختياري: لو هتستخدم CultureInfo.
		bool IsRightToLeft(); // لو حابب تتأكد إن اللغة RTL زي العربية.
	}

}
