using Microsoft.AspNetCore.Http;

namespace ERPManagement.Application.Models.Mail
{
	public class Email
	{
		public string To { get; set; }
		public string NameOfSend { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
		public IList<IFormFile> attachments { get; set; } = null;
	}
}
