using ERPManagement.Application.Contracts.Infrastructure;
using ERPManagement.Application.Features.Common.Emails.Commands.Models;
using ERPManagement.Application.Models.Mail;
using ERPManagement.Application.Responses;
using MediatR;
namespace ERPManagement.Application.Features.Common.Emails.Commands.Handlers
{
    public class EmailCommandHandler : BaseResponseHandler,
        IRequestHandler<SendEmailCommand, BaseResponse<string>>
    {
        #region Fields
        private readonly IEmailService _emailsService;
        #endregion
        #region Constructors
        public EmailCommandHandler(IEmailService emailsService)
        {
            _emailsService = emailsService;
        }
        #endregion
        #region Handle Functions
        public async Task<BaseResponse<string>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            var response = await _emailsService.SendEmailAsync(new Email
            {
                To = request.MailTo,
                Subject = request.Subject,
                Body = request.Body
            });
            // request.MailTo, request.SendTo, request.Body, request.Subject, request.attachments);
            if (response)
                return Success("");
            return BadRequest<string>();
        }
        #endregion
    }
}
