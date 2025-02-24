using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PureLifeClinic.API.Helpers;
using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PureLifeClinic.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly AppSettings _appSettings;
        private readonly IUserContext _userContext;
        private readonly IUserService _userService;

        public AuthController(
            ILogger<AuthController> logger,
            IAuthService authService,
            IOptions<AppSettings> appSettings,
            IRefreshTokenService refreshTokenService,
            IUserService userService,
            IUserContext userContext)
        {
            _logger = logger;
            _authService = authService;
            _refreshTokenService = refreshTokenService; 
            _appSettings = appSettings.Value;
            _userContext = userContext;
            _userService = userService;
        }

        #region login/ logout
        [HttpPost, Route("login")]
        public async Task<IActionResult> Login(LoginViewModel model, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _authService.Login(model.UserName, model.Password);
                    if (result.Success)
                    {
                        // get Token data
                        var tokenData = await GenerateJwtToken(result.Data.Id);
                        var refreshToken = tokenData.Data.RefreshToken;

                        // insert Refresh Token
                        RefreshTokenCreateViewModel refreshTokenModel = new RefreshTokenCreateViewModel
                        {
                            Token = refreshToken.Token,
                            CreateOn = refreshToken.CreateOn,
                            ExpireOn = refreshToken.ExpireOn,
                            AccessTokenId = tokenData.Data.AccessTokenId,
                        };

                        var createdTokenResult = await _refreshTokenService.InsertRefreshToken(result.Data.Id, refreshTokenModel, default);
                        SetRefreshTokenInCookies(createdTokenResult.Data.Token, createdTokenResult.Data.ExpireOn);

                        return Ok(new ResponseViewModel<AuthResultViewModel>
                        {
                            Success = true,
                            Data = new AuthResultViewModel
                            {
                                AccessToken = tokenData.Data.AccessToken,
                                RefreshToken = createdTokenResult.Data,
                                Role = result.Data.Role,
                                UserEmail = result.Data.Email,
                                UserId = result.Data.Id
                            },
                            Message = "Login successfully"
                        });
                    }

                    return Ok(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred while login");
                    string message = $"An error occurred while login- " + ex.Message;

                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel
                    {
                        Success = false,
                        Message = message,
                        Error = new ErrorViewModel
                        {
                            Code = "LOGIN_ERROR",
                            Message = message
                        }
                    });
                }
            }

            return BadRequest(new ResponseViewModel
            {
                Success = false,
                Message = "Invalid input",
                Error = new ErrorViewModel
                {
                    Code = "INPUT_VALIDATION_ERROR",
                    Message = ModelStateHelper.GetErrors(ModelState)
                }
            });
        }

        [HttpPost, Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();
            return Ok();
        }
        #endregion

        #region genarate Token
        private async Task<ResponseViewModel<GenarateTokenViewModel>> GenerateJwtToken(int userId)
        {
            var user = await _userService.GetById(userId, default);

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.JwtConfig.Secret);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Aud, _appSettings.JwtConfig.ValidAudience),
                new Claim(JwtRegisteredClaimNames.Iss, _appSettings.JwtConfig.ValidIssuer),
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(Convert.ToDouble(_appSettings.JwtConfig.TokenExpirationMinutes)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return new ResponseViewModel<GenarateTokenViewModel>()
            {
                Data = new GenarateTokenViewModel
                {
                    RefreshToken = GenerateRefreshToken(),
                    AccessToken = jwtToken,
                    AccessTokenId = token.Id,
                },
                Success = true,
            };
        }

        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var generator = new RNGCryptoServiceProvider();

            generator.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpireOn = DateTime.UtcNow.AddDays(_appSettings.JwtConfig.RefreshTokenExpiryDays),
                CreateOn = DateTime.UtcNow
            };
        }

        [HttpGet("refresh-token")]
        public async Task<IActionResult> RefreshTokenCheckAsync()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Request.Cookies.TryGetValue("refreshTokenKey", out var refreshToken))
                    {
                        var result = await _refreshTokenService.RefreshTokenCheckAsync(refreshToken);

                        if (!result.Success)
                        {
                            return BadRequest(new ResponseViewModel
                            {
                                Message = "Invalid refresh token.",
                                Success = false,
                            });
                        }

                        var tokenData = await GenerateJwtToken(Convert.ToInt32(_userContext.UserId));
                        var refreshTokenData = GenerateRefreshToken();

                        if (tokenData == null || refreshTokenData == null)
                        {
                            throw new Exception("Token was genarated failed");
                        }

                        refreshTokenData.AccessTokenId = tokenData?.Data?.AccessTokenId;

                        // autp mapper
                        RefreshTokenCreateViewModel refreshTokenCreateModel = new RefreshTokenCreateViewModel
                        {
                            Token = refreshTokenData.Token,
                            CreateOn = refreshTokenData.CreateOn,
                            ExpireOn = refreshTokenData.ExpireOn,
                            AccessTokenId = tokenData.Data?.AccessTokenId,
                        };

                        // insert refreshToken to db
                        var createdTokenResult = await _refreshTokenService.InsertRefreshToken(Convert.ToInt32(_userContext.UserId), refreshTokenCreateModel, default);
                        if (!createdTokenResult.Success || createdTokenResult.Data == null)
                        {
                            throw new Exception("Token was genarated failed");
                        }
                        SetRefreshTokenInCookies(createdTokenResult.Data.Token, createdTokenResult.Data.ExpireOn);

                        return Ok(new ResponseViewModel<AuthResultViewModel>
                        {
                            Success = true,
                            Data = new AuthResultViewModel
                            {
                                AccessToken = tokenData.Data.AccessToken,
                                RefreshToken = createdTokenResult.Data //_refreshTokenViewModelMapper.MapModel(refreshTokenData)
                            }
                        });
                    }
                    return BadRequest(new ResponseViewModel
                    {
                        Success = false,
                        Message = "RefreshToken not found",
                        Error = new ErrorViewModel
                        {
                            Code = "NOT_FOUND_ERROR",
                            Message = "RefreshToken not found"
                        }
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred while refresh token");
                    string message = $"An error occurred while refresh token- " + ex.Message;

                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel
                    {
                        Success = false,
                        Message = message,
                        Error = new ErrorViewModel
                        {
                            Code = "REFRESH_ERROR",
                            Message = message
                        }
                    });
                }
            }
            return BadRequest(new ResponseViewModel
            {
                Success = false,
                Message = "Invalid input",
                Error = new ErrorViewModel
                {
                    Code = "INPUT_VALIDATION_ERROR",
                    Message = ModelStateHelper.GetErrors(ModelState)
                }
            });
        }

        #endregion

        #region SetRefreshTokenInCookies
        private void SetRefreshTokenInCookies(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = expires.ToLocalTime()
            };

            //cookieOptionsExpires = DateTime.UtcNow.AddSeconds(cookieOptions.Timeout);
            Response.Cookies.Append("refreshTokenKey", refreshToken, cookieOptions);
        }

        #endregion
    }
}
