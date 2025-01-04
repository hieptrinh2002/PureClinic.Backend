using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PureLifeClinic.API.Helpers;
using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Exceptions;
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
        private readonly AppSettings _appSettings;
        private readonly IUserContext _userContext;
        private readonly IMailService _mailService;
        private readonly IUserService _userService;
        public AuthController(
            IMailService mailService,
            ILogger<AuthController> logger,
            IAuthService authService,
            IConfiguration configuration,
            IOptions<AppSettings> appSettings,
            IUserContext userContext,
            IUserService userService)
        {
            _mailService = mailService; 
            _logger = logger;
            _authService = authService;
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
                        var tokenData = GenerateJwtToken(result.Data.Id);
                        var refreshToken = tokenData.Data.RefreshToken;

                        // insert Refresh Token
                        RefreshTokenCreateViewModel refreshTokenModel = new RefreshTokenCreateViewModel
                        {
                            Token = refreshToken.Token,
                            CreateOn = refreshToken.CreateOn,
                            ExpireOn = refreshToken.ExpireOn,
                            AccessTokenId = tokenData.Data.AccessTokenId,
                        };

                        var createdTokenResult = await _authService.InsertRefreshToken(result.Data.Id, refreshTokenModel, default);
                        SetRefreshTokenInCookies(createdTokenResult.Data.Token, createdTokenResult.Data.ExpireOn);

                        return Ok(new ResponseViewModel<AuthResultViewModel>
                        {
                            Success = true,
                            Data = new AuthResultViewModel
                            {
                                AccessToken = tokenData.Data.AccessToken,
                                RefreshToken = createdTokenResult.Data,
                                Role = result.Data.Role,
                                UserId = result.Data.Id
                            },
                            Message = "Login successful"
                        });
                    }

                    return BadRequest(result);
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
        private ResponseViewModel<GenarateTokenViewModel> GenerateJwtToken(int userId)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.JwtConfig.Secret);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Aud, _appSettings.JwtConfig.ValidAudience),
                new Claim(JwtRegisteredClaimNames.Iss, _appSettings.JwtConfig.ValidIssuer),
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
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
                        var result = await _authService.RefreshTokenCheckAsync(refreshToken);

                        if (!result.Success)
                        {
                            return BadRequest(new ResponseViewModel
                            {
                                Message = "Invalid refresh token.",
                                Success = false,
                            });
                        }

                        var tokenData = GenerateJwtToken(Convert.ToInt32(_userContext.UserId));
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
                        var createdTokenResult = await _authService.InsertRefreshToken(Convert.ToInt32(_userContext.UserId), refreshTokenCreateModel, default);
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

        #region Reset password
        [HttpGet("activate-email")]
        public async Task<IActionResult> EmailConfirmation(string emailConfirmation, string activeToken, CancellationToken cancellationToken)
        {
            if (!await _userService.IsExists("Email", emailConfirmation, cancellationToken))
            {
                string message = $"The user Email- '{emailConfirmation}' not found !";
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel<UserViewModel>
                {
                    Success = false,
                    Message = message,
                    Error = new ErrorViewModel
                    {
                        Code = "NOT_FOUND_CODE",
                        Message = message
                    }
                });
            }
            var confirmResult = await _authService.ConfirmEmailAsync(emailConfirmation, activeToken, cancellationToken);
            if (!confirmResult.Success) {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel<UserViewModel>
                {
                    Success = false,
                    Message = confirmResult.Message,
                    Error = new ErrorViewModel
                    {
                        Code = "EMAIL_CONFIRM_ERROR",
                        Message = confirmResult.Message
                    }
                });
            }

            return Ok(confirmResult);   
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> SendActivationEmail(UserCreateViewModel model, string ClientUrl, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                string message = "";
                if (await _userService.IsExists("UserName", model.UserName, cancellationToken))
                {
                    message = $"The user name- '{model.UserName}' already exists";
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel<UserViewModel>
                    {
                        Success = false,
                        Message = message,
                        Error = new ErrorViewModel
                        {
                            Code = "DUPLICATE_NAME",
                            Message = message
                        }
                    });
                }

                if (await _userService.IsExists("Email", model.Email, cancellationToken))
                {
                    message = $"The user Email- '{model.Email}' already exists";
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel<UserViewModel>
                    {
                        Success = false,
                        Message = message,
                        Error = new ErrorViewModel
                        {
                            Code = "DUPLICATE_CODE",
                            Message = message
                        }
                    });
                }

                try
                {
                    var response = await _userService.Create(model, cancellationToken);

                    if (response.Success)
                    {
                        // Create activate email token => return link activate

                        var result = await _userService.GenerateEmailConfirmationTokenAsync(model.Email);
                        if (!result.Success)
                            throw new Exception(result.Message);
                      
                        var token = Uri.EscapeDataString(result.Data.ActivationToken);
                        var email = Uri.EscapeDataString(model.Email);
                        var confirmationLink = $"{ClientUrl}?token={token}&email={email}";
                      
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Template", "MailTemplate.html");
                        var emailBody = MailHelper.ReadAndProcessHtmlTemplate(filePath, confirmationLink, model.UserName);

                        var mailRequestViewModel = new MailRequestViewModel
                        {
                            ToEmail = model.Email,
                            Subject = "Activate Your Account",
                            Body = emailBody,
                        };

                        await _mailService.SendEmailAsync(mailRequestViewModel);

                        return Ok(response);
                    }
                    else
                    {
                        return BadRequest(response);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred while adding the user");
                    message = $"An error occurred while adding the user- " + ex.Message;

                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel<UserViewModel>
                    {
                        Success = false,
                        Message = message,
                        Error = new ErrorViewModel
                        {
                            Code = "ADD_USER_ERROR",
                            Message = message
                        }
                    });
                }
            }

            return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel<UserViewModel>
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


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ForgotPasswordRequestViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel
                    {
                        Success = false,
                        Message = "forgot password request failed",
                    });
                }  
                // get User by email
                var user = await _userService.GetByEmail(model.Email, default);

                if (user == null)
                    throw new NotFoundException("email not found");
                //var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                //var callback = Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email }, Request.Scheme);

                return Ok("Password changed successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception here
                return StatusCode(500, "An error occurred while changing the password.");
            }
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
