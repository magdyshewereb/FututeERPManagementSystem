using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using ERPManagement.Application.Configuration;
using ERPManagement.Application.Security.Authentication.Models;
using ERPManagement.Application.Security.Authentication.Services;
using ERPManagement.Domain.Entities;
using Hr.Application.Interfaces.GenericRepository.Command;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ERPManagement.Infrastructure.Security.Authentication.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<User> _userManager;
        private readonly IEncryptionProvider _encryptionProvider;
        public AuthenticationService(
            JwtSettings jwtSettings,
            IBaseRepository<UserRefreshToken> refreshTokenRepository,

            UserManager<User> userManager)
        {
            _jwtSettings = jwtSettings;

            _userManager = userManager;
            _encryptionProvider = new GenerateEncryptionProvider("8a4dcaaec64d412380fe4b02193cd26f"); // TODO: Move to config
        }

        public async Task<JwtAuthResult> RegisterAsync(User user)
        {
            var (jwtSecurityToken, accessToken) = await CreateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            user.RefreshTokens?.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            return new JwtAuthResult
            {
                Email = user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = user.UserName,
                RefreshToken = refreshToken.RefreshToken,
                RefreshTokenExpiration = refreshToken.ExpiresOn
                //,Roles = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? string.Empty
            };
        }
        public async Task<JwtAuthResult> GetTokenAsync(TokenRequestModel model, User user)
        {
            var authModel = new JwtAuthResult();
            var (jwtSecurityToken, accessToken) = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            authModel.IsAuthenticated = true;
            authModel.AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Email = user.Email;
            authModel.Username = user.UserName;
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.Roles = rolesList.ToList();

            var userRefreshToken = await _userManager.Users
                                  .Include(u => u.RefreshTokens)
                                  .SingleOrDefaultAsync(u => u.Id == user.Id);
            if (userRefreshToken.RefreshTokens.Any(t => t.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                authModel.RefreshToken = activeRefreshToken.RefreshToken;
                authModel.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                authModel.RefreshToken = refreshToken.RefreshToken;
                authModel.RefreshTokenExpiration = refreshToken.ExpiresOn;
                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);
            }

            return authModel;
        }
        public async Task<(JwtSecurityToken, string)> CreateJwtToken(User user)
        {
            List<Claim> claims = await GetClaims(user);

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var jwtSecurityToken = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience, // e.g. for Anjular, "https://localhost:4200",
                claims,
                expires: DateTime.UtcNow.AddHours(_jwtSettings.AccessTokenExpireDate),
                signingCredentials: creds
            );

            return (jwtSecurityToken, new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken));
        }
        public async Task<JwtAuthResult> RefreshTokenAsync(string token)
        {

            var authModel = new JwtAuthResult();

            #region Validate Token
            var principal = GetPrincipalFromExpiredToken(token);
            if (principal == null)
                authModel.Message = "Invalid access token.";
            var userId = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var _user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.Id == userId);
            #endregion

            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.RefreshToken == token));

            if (user == null)
            {
                authModel.Message = "Invalid token";
                return authModel;
            }

            var refreshToken = user.RefreshTokens.Single(t => t.RefreshToken == token);

            if (!refreshToken.IsActive)
            {
                authModel.Message = "Inactive token";
                return authModel;
            }

            refreshToken.RevokedOn = DateTime.UtcNow;

            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            var (jwtSecurityToken, accessToken) = await CreateJwtToken(user);
            authModel.IsAuthenticated = true;
            authModel.AccessToken = accessToken;
            authModel.Email = user.Email;
            authModel.Username = user.UserName;
            var roles = await _userManager.GetRolesAsync(user);
            authModel.Roles = roles.ToList();
            authModel.RefreshToken = newRefreshToken.RefreshToken;
            authModel.RefreshTokenExpiration = newRefreshToken.ExpiresOn;

            return authModel;
        }


        public async Task<bool> RevokeTokenAsync(string token)
        {
            var user = await _userManager.Users
                            .SingleOrDefaultAsync(u => u.RefreshTokens
                            .Any(t => t.RefreshToken == token));

            if (user == null)
                return false;

            var refreshToken = user.RefreshTokens.Single(t => t.RefreshToken == token);

            if (!refreshToken.IsActive)
                return false;

            refreshToken.RevokedOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            return true;
        }
        private UserRefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return new UserRefreshToken
            {
                RefreshToken = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(10),
                CreatedOn = DateTime.UtcNow
            };
        }
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
            var _tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = _jwtSettings.ValidateIssuer,
                ValidateAudience = _jwtSettings.ValidateAudience,
                ValidateLifetime = _jwtSettings.ValidateLifeTime,
                ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };

            var tokenValidation = _tokenValidationParameters.Clone();
            tokenValidation.ValidateLifetime = false;

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidation, out SecurityToken validatedToken);

                if (validatedToken is not JwtSecurityToken jwtToken ||
                    !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    return null;

                return principal;
            }
            catch
            {
                return null;
            }
        }
        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpireDate),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<List<Claim>> GetClaims(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(nameof(UserClaimModel.Id), user.Id.ToString()),
            };
            // Add all roles as claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            // Add extra claims if exist
            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            return claims;
        }
        public JwtSecurityToken ReadJWTToken(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                throw new ArgumentNullException(nameof(accessToken));

            var handler = new JwtSecurityTokenHandler();
            return handler.ReadJwtToken(accessToken);
        }
        public Task<string> ValidateToken(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                return Task.FromResult("InvalidToken");
            var handler = new JwtSecurityTokenHandler();
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = _jwtSettings.ValidateIssuer,
                ValidIssuers = new[] { _jwtSettings.Issuer },
                ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)),
                ValidAudience = _jwtSettings.Audience,
                ValidateAudience = _jwtSettings.ValidateAudience,
                ValidateLifetime = _jwtSettings.ValidateLifeTime
            };
            try
            {
                handler.ValidateToken(accessToken, parameters, out _);
                return Task.FromResult("NotExpired");
            }
            catch (SecurityTokenExpiredException)
            {
                return Task.FromResult("Expired");
            }
            catch
            {
                return Task.FromResult("InvalidToken");
            }
        }
        public async Task<string> ConfirmEmail(int? userId, string? code)
        {
            if (userId == null || string.IsNullOrEmpty(code))
                return "ErrorWhenConfirmEmail";

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return "UserNotFound";

            var result = await _userManager.ConfirmEmailAsync(user, code);
            return result.Succeeded ? "Success" : "ErrorWhenConfirmEmail";
        }
        public async Task<string> ConfirmResetPassword(string code, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return "UserNotFound";

            return user.Id.ToString() == code ? "Success" : "Failed";
        }
        public async Task<string> SendResetPasswordCode(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return "UserNotFound";

            var (jwtSecurityToken, accessToken) = await CreateJwtToken(user);

            return accessToken;
        }
        public async Task<string> ResetPassword(string email, string password, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return "UserNotFound";

            var result = await _userManager.ResetPasswordAsync(user, token, password);
            return result.Succeeded ? "Success" : "Failed";
        }

        // Not Implemented (for future use)
        public Task<(string, DateTime?)> ValidateDetails(JwtSecurityToken jwtToken, string accessToken, string refreshToken)
        {
            throw new NotImplementedException();
        }


    }
}
