using ERPManagement.Application.Security.Authentication.Models;
using ERPManagement.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace ERPManagement.Application.Security.Authentication.Services
{
    public interface IAuthenticationService
    {
        Task<JwtAuthResult> RegisterAsync(User user);
        Task<(JwtSecurityToken, string)> CreateJwtToken(User user);
        Task<JwtAuthResult> GetTokenAsync(TokenRequestModel model, User user);
        JwtSecurityToken ReadJWTToken(string accessToken);
        Task<(string, DateTime?)> ValidateDetails(JwtSecurityToken jwtToken, string accessToken, string refreshToken);
        Task<JwtAuthResult> RefreshTokenAsync(string token);
        Task<string> ValidateToken(string accessToken);
        Task<string> ConfirmEmail(int? userId, string? code);
        Task<string> SendResetPasswordCode(string email);
        Task<string> ConfirmResetPassword(string code, string email);
        Task<string> ResetPassword(string email, string password, string token);
        Task<bool> RevokeTokenAsync(string token);
    }
}
