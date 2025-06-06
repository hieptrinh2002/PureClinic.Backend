﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PureLifeClinic.Application.BusinessObjects.AuthViewModels.Token;
using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IRepositories;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace PureLifeClinic.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly AppSettings _appSettings;
        private readonly ILogger<TokenService> _logger;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public TokenService(IOptions<AppSettings> appSettings, IUserService userService, ILogger<TokenService> logger, IUnitOfWork unitOfWork)
        {
            _appSettings = appSettings.Value;
            _logger = logger;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseViewModel<GenerateTokenViewModel>> GenerateJwtToken(int userId)
        {
            try
            {
                var includeList = new List<Expression<Func<User, object>>> { x => x.Role };
                var user = await _unitOfWork.Users.GetById(includeList, userId, default);
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.JwtConfig!.Secret!);

                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Aud, _appSettings.JwtConfig.ValidAudience!),
                    new Claim(JwtRegisteredClaimNames.Iss, _appSettings.JwtConfig.ValidIssuer!),
                    new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, user.Role.Name!.ToString()),
                    new Claim("userRoleId", user.Role.Id.ToString()),
                };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddMinutes(Convert.ToDouble(_appSettings.JwtConfig.TokenExpirationMinutes)),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = jwtTokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = jwtTokenHandler.WriteToken(token);

                return new ResponseViewModel<GenerateTokenViewModel>()
                {
                    Data = new GenerateTokenViewModel
                    {
                        RefreshToken = GenerateRefreshToken(),
                        AccessToken = jwtToken,
                        AccessTokenId = token.Id,
                    },
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error generating JWT: {ex.Message}");
                throw;
            }
        }

        public RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpireOn = DateTime.UtcNow.AddDays(_appSettings.JwtConfig.RefreshTokenExpiryDays),
                CreateOn = DateTime.UtcNow
            };
        }
    }
}
