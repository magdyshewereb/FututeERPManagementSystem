using ERPManagement.Application.Models.Mail;

namespace ERPManagement.Application.Contracts.Infrastructure
{
	public interface IEmailService
	{
		Task<bool> SendEmailAsync(Email email);
		//Task<string> SendEmailAsync(string mailTo, string? sendTo, string body, string? subject, IList<IFormFile> attachments);
	}
}
