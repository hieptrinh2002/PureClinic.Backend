using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PureLifeClinic.API.Helpers;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Exceptions;
using PureLifeClinic.Core.Interfaces.IServices;

namespace PureLifeClinic.API.Controllers.V1
{
    [ApiVersion("1.0")] 
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IMailService _mailService;
        private readonly ILogger<AuthController> _logger;

        public AccountController(ILogger<AuthController> logger, 
            IUserService userService, IAuthService authService, IMailService mailService)
        {
            _logger = logger;
            _userService = userService;
            _authService = authService;
            _mailService = mailService;
        }

        [HttpPost("unlock-account")]
        public async Task<IActionResult> UnlockAccount([FromBody] UnlockAccountRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _userService.UnlockAccountAsync(model.UserId);
                    if (!result)
                    {
                        return BadRequest("Failed to unlock the account.");
                    }

                    return Ok("Account unlocked successfully.");
                }
                catch (Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel
            {
                Success = false,
                Message = "Invalid input",
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> SendActivationEmail([FromBody] UserCreateViewModel model, string clientUrl, CancellationToken cancellationToken)
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
                        var confirmationLink = MailHelper.GenerateConfirmationLink(email, clientUrl, token);

                        await SendConfirmationEmailAsync(model.Email, confirmationLink, model.UserName);
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
            if (!confirmResult.Success)
            {
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

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest model)
        {
            try
            {
                var result = await _userService.ResetPasswordAsync(model.Email, model.Token, model.NewPassword);
                if (!result.Success)
                {
                    return BadRequest(new ResponseViewModel
                    {
                        Message = "Failed to reset password.",
                        Success = false
                    });
                }

                return Ok(new ResponseViewModel
                {
                    Message = "Password reset successfully.",
                    Success = false
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ForgotPasswordRequestViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel
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

                // get User by email
                var user = await _userService.GetByEmail(model.Email, default);

                if (user == null)
                    throw new NotFoundException("email not found");

                // send email with refresh passwork link
                var result = await _userService.GenerateResetPasswordTokenAsync(model);
                if (!result.Success)
                    throw new Exception(result.Message);

                var mailRequest = new MailRequestViewModel()
                {
                    ToEmail = model.Email,
                    Subject = "Forgot password",
                    Body = result.Data.EmailBody
                };

                await _mailService.SendEmailAsync(mailRequest);

                return Ok(new ResponseViewModel
                {
                    Message = "Reset password mail was sent successfully.",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel
                {
                    Success = false,
                    Error = new ErrorViewModel
                    {
                        Code = "EMAIL_RESET_PASSWORD_ERROR",
                        Message = ex.Message
                    }
                });
            }
        }

        private async Task SendConfirmationEmailAsync(string email, string confirmationLink, string userName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Template", "MailTemplate.html");
            var emailBody = MailHelper.ReadAndProcessHtmlTemplate(filePath, confirmationLink, userName);

            var mailRequestViewModel = new MailRequestViewModel
            {
                ToEmail = email,
                Subject = "Activate Your Account",
                Body = emailBody,
            };

            await _mailService.SendEmailAsync(mailRequestViewModel);
        }
    }
}
