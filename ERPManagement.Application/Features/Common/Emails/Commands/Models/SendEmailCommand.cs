using ERPManagement.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;


namespace ERPManagement.Application.Features.Common.Emails.Commands.Models
{
	public class SendEmailCommand : IRequest<BaseResponse<string>>
	{
		public string MailTo { get; set; }
		public string? SendTo { get; set; }
		public string Body { get; set; }
		public string? Subject { get; set; }
		public IList<IFormFile> attachments { get; set; }
	}
}
