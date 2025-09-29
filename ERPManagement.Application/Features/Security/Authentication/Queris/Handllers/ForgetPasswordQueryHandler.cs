using ERPManagement.Application.Contracts.Infrastructure.Messaging.Queries;
using ERPManagement.Application.Features.Security.Authentication.Queris.Models;
using ERPManagement.Application.Responses;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;

namespace ERPManagement.Application.Features.Security.Authentication.Queris.Handllers
{
	public class ForgetPasswordQueryHandler(IConfiguration _configuration, IAuthenticationService _authenticationService) : IQuerytHandler<ForgetPasswordQuery, BaseResponse<string>>
	{
		public async Task<BaseResponse<string>> Handle(ForgetPasswordQuery request, CancellationToken cancellationToken)
		{
			//try
			//{

			//    var emailsetting = _configuration.GetSection("emailSettings").Get<EmailSettings>();
			//    var uiurl = _configuration.GetValue<string>("uiurl");
			//    var token = await _authenticationService.SendResetPasswordCode(request.EmailAddress);
			//    SmtpClient client = new SmtpClient(emailsetting.Host, emailsetting.Port);
			//    client.EnableSsl = true;
			//    client.UseDefaultCredentials = false;
			//    client.Credentials = new NetworkCredential(emailsetting.FromEmail, emailsetting.Password);

			//    // Create email message
			//    MailMessage mailMessage = new MailMessage();
			//    mailMessage.From = new MailAddress(emailsetting.FromEmail);
			//    mailMessage.To.Add(request.EmailAddress);
			//    mailMessage.Subject = "Password Recovery";
			//    mailMessage.IsBodyHtml = true;
			//    StringBuilder mailBody = new StringBuilder();
			//    mailBody.AppendFormat("<h1>Password Recovery</h1>");
			//    mailBody.AppendFormat("<br />");
			//    mailBody.AppendFormat($"<a>{uiurl}/Accounts/ResetPassword?token={token}&&email={request.EmailAddress}</a>");
			//    mailMessage.Body = mailBody.ToString();

			//    // Send email
			//    await client.SendMailAsync(mailMessage);
			//    return new ResponseHandler().Success(token);
			//}
			//catch (Exception e)
			//{
			//    return new ResponseHandler().BadRequest<string>("Failed");
			//}
			return new BaseResponse<string>();
		}
	}
}
