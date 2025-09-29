using ERPManagement.Application.Contracts.Infrastructure.Messaging.Commands;
using ERPManagement.Application.Features.Security.Authentication.Command.Models;
using ERPManagement.Application.Responses;
using ERPManagement.Application.Security.Authentication.Models;
using ERPManagement.Application.Security.Authentication.Services;
using ERPManagement.Application.Shared.Constants;
using ERPManagement.Application.Shared.Services;
using ERPManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;
namespace ERPManagement.Application.Features.Security.Authentication.Command.Handlers
{
    public class AuthenticationHandler : BaseResponseHandler,
          ICommandHandler<SignInCommand, BaseResponse<JwtAuthResult>>,
          ICommandHandler<RefreshTokenCommand, BaseResponse<JwtAuthResult>>,
          ICommandHandler<ResetPasswordVm, BaseResponse<string>>
    {
        UserManager<User> _userManager;
        SignInManager<User> _signInManager;
        IAuthenticationService _authenticationService;
        private readonly IMessageLocalizationService _messageLocalization;
        public AuthenticationHandler(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IAuthenticationService authenticationService,
            IMessageLocalizationService messageLocalization)
        {

            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationService = authenticationService;
            _messageLocalization = messageLocalization;
        }
        public async Task<BaseResponse<JwtAuthResult>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var invalidLogin = _messageLocalization.GetMessage(AuthMessages.InvalidLogin);
            var confirmEmail = _messageLocalization.GetMessage(AuthMessages.ConfirmEmail);

            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null || !(await _signInManager.CheckPasswordSignInAsync(user, request.Password, false)).Succeeded)
                return BadRequest<JwtAuthResult>(invalidLogin);

            //if (!user.EmailConfirmed)
            //    return BadRequest<JwtAuthResult>(confirmEmail);


            var token = await _authenticationService.GetTokenAsync
                (new TokenRequestModel { Email = request.Email, Password = request.Password }
                , user);
            return Success(token);
        }

        public async Task<BaseResponse<JwtAuthResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.RefreshTokenAsync(request.AccessToken);
            return Success(result);
        }
        public async Task<BaseResponse<string>> Handle(ResetPasswordVm request, CancellationToken cancellationToken)
        {
            var res = await _authenticationService.ResetPassword(request.EmailAddress, request.Password, request.Token);
            return Success(res);
        }

    }
}
