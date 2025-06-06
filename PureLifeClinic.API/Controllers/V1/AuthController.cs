﻿using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PureLifeClinic.API.ActionFilters;
using PureLifeClinic.Application.BusinessObjects.AuthViewModels;
using PureLifeClinic.Application.BusinessObjects.AuthViewModels.Token;
using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Core.Exceptions;

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
        private readonly IUserContext _userContext;
        private readonly ITokenService _tokenService;

        public AuthController(
            ILogger<AuthController> logger,
            IAuthService authService,
            IRefreshTokenService refreshTokenService,
            ITokenService tokenService,
            IUserContext userContext)
        {
            _logger = logger;
            _authService = authService;
            _refreshTokenService = refreshTokenService; 
            _userContext = userContext;
            _tokenService = tokenService;   
        }

        #region login/ logout
        [HttpPost, Route("login")]
        [ServiceFilter(typeof(ValidateInputViewModelFilter))]

        public async Task<IActionResult> Login(LoginViewModel model, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _authService.Login(model.UserName, model.Password);
                // get Token data
                var tokenData = await _tokenService.GenerateJwtToken(result.Data.Id);
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

                if (createdTokenResult?.Data != null)
                    SetRefreshTokenInCookies(createdTokenResult.Data.Token, createdTokenResult.Data.ExpireOn);

                return Ok(new ResponseViewModel<AuthResultViewModel>
                {
                    Success = true,
                    Data = new AuthResultViewModel
                    {
                        AccessToken = tokenData.Data.AccessToken,
                        //RefreshToken = createdTokenResult.Data,
                        Role = result.Data.Role,
                        UserEmail = result.Data.Email,
                        UserId = result.Data.Id
                    },
                    Message = "Login successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while login");
                throw;
            }
        }

        [HttpPost, Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();
            return Ok(new ResponseViewModel
            {
                Message = "Logout successfully",
                Success = true
            });
        }

        #endregion

        #region genarate Token

        [HttpGet("refresh-token")]
        public async Task<IActionResult> RefreshTokenCheckAsync()
        {
            try
            {
                if (!Request.Cookies.TryGetValue("refreshTokenKey", out var refreshToken))
                    throw new BadRequestException("RefreshToken key not found");

                var result = await _refreshTokenService.RefreshTokenCheckAsync(refreshToken);

                if (!result)
                    throw new BadRequestException("Invalid refresh token");

                var tokenData = await _tokenService.GenerateJwtToken(Convert.ToInt32(_userContext.UserId));
                var refreshTokenData = _tokenService.GenerateRefreshToken();

                if (tokenData == null || refreshTokenData == null)
                {
                    throw new ErrorException("Token was generated failed");
                }

                refreshTokenData.AccessTokenId = tokenData?.Data?.AccessTokenId;

                // autp mapper
                var refreshTokenCreateModel = new RefreshTokenCreateViewModel
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
                    throw new ErrorException("Token was genarated failed");
                }
                SetRefreshTokenInCookies(createdTokenResult.Data.Token, createdTokenResult.Data.ExpireOn);

                return Ok(new ResponseViewModel<AuthResultViewModel>
                {
                    Success = true,
                    Data = new AuthResultViewModel
                    {
                        AccessToken = tokenData.Data.AccessToken,
                    }
                });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while refresh token");
                throw;
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
