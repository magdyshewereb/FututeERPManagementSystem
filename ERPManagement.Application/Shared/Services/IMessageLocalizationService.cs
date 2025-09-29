namespace ERPManagement.Application.Shared.Services
{
	public interface IMessageLocalizationService
	{
		string GetMessage(string key, params object[] args);
	}
}
