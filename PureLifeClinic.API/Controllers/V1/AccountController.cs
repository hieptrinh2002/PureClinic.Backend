using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PureLifeClinic.API.ActionFilters;
using PureLifeClinic.API.Attributes;
using PureLifeClinic.API.Helpers;
using PureLifeClinic.Application.BusinessObjects.AuthViewModels.Account;
using PureLifeClinic.Application.BusinessObjects.AuthViewModels.ForgotPassword;
using PureLifeClinic.Application.BusinessObjects.AuthViewModels.ResetPassword;
using PureLifeClinic.Application.BusinessObjects.EmailViewModels;
using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Application.BusinessObjects.UserViewModels;
using PureLifeClinic.Application.Interfaces.IBackgroundJobs;
using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Core.Common.Constants;
using PureLifeClinic.Core.Enums.PermissionEnums;
using PureLifeClinic.Core.Exceptions;

namespace PureLifeClinic.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly ILogger<AccountController> _logger;
        private readonly IBackgroundJobService _backgroundService; 

        public AccountController(ILogger<AccountController> logger,
            IUserService userService, IAuthService authService, IBackgroundJobService backgroundJobService)
        {
            _logger = logger;
            _userService = userService;
            _authService = authService;
            _backgroundService = backgroundJobService;  
        }

        [HttpPost("unlock-account")]
        [PermissionAuthorize(ResourceConstants.User, PermissionAction.LockUnlock)]
        public async Task<IActionResult> UnlockAccount([FromBody] UnlockAccountRequestViewModel model)
        {
            if (!ModelState.IsValid)
                throw new BadRequestException("Invalid input: " + ModelStateHelper.GetErrors(ModelState), ErrorCode.InputValidateError);

            var result = await _userService.UnlockAccountAsync(model.UserId);
            if (!result)
                throw new BadRequestException("Failed to unlock the account.");

            return Ok("Account unlocked successfully.");
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> SendActivationEmail([FromBody] UserCreateViewModel model, string clientUrl, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                throw new BadRequestException("Invalid input: " + ModelStateHelper.GetErrors(ModelState), ErrorCode.InputValidateError);

            if (await _userService.IsExists("UserName", model.UserName, cancellationToken))
            {
                throw new BadRequestException($"The user name- '{model.UserName}' already exists", ErrorCode.DuplicateUserNameError);
            }

            if (await _userService.IsExists("Email", model.Email, cancellationToken))
            {
                var message = $"The user Email- '{model.Email}' already exists";
                throw new BadRequestException(message, ErrorCode.DuplicateUserNameError);
            }
            try
            {
                var response = await _userService.Create(model, cancellationToken);

                if (response.Success)
                {
                    var result = await _userService.GenerateEmailConfirmationTokenAsync(model.Email);
                    if (!result.Success)
                        throw new ErrorException(result.Message);

                    var token = Uri.EscapeDataString(result.Data.ActivationToken);
                    var email = Uri.EscapeDataString(model.Email);
                    var confirmationLink = MailHelper.GenerateConfirmationLink(email, clientUrl, token);

                    SendConfirmationEmailAsync(model.Email, confirmationLink, model.UserName);
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
                throw;
            }
        }

        [HttpGet("activate-email")]
        [AllowAnonymous]
        public async Task<IActionResult> EmailConfirmation(string emailConfirmation, string activeToken, CancellationToken cancellationToken)
        {
            if (!await _userService.IsExists("Email", emailConfirmation, cancellationToken))
            {
                string message = $"The user Email- '{emailConfirmation}' not found !";
                throw new BadRequestException(message);
            }
            var confirmResult = await _authService.ConfirmEmailAsync(emailConfirmation, activeToken, cancellationToken);
            if (!confirmResult.Success)
            {
                throw new BadRequestException(confirmResult.Message);
            }
            return Ok(confirmResult);
        }

        [HttpPost("reset-password")]
        [ServiceFilter(typeof(ValidateInputViewModelFilter))]

        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest model)
        {

            var result = await _userService.ResetPasswordAsync(model.Email, model.Token, model.NewPassword);
            if (!result.Success)
            {
                throw new BadRequestException("Failed to reset password.");
            }

            return Ok(new ResponseViewModel
            {
                Message = "Password reset successfully.",
                Success = true
            });
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        [ServiceFilter(typeof(ValidateInputViewModelFilter))]

        public async Task<IActionResult> ChangePassword([FromBody] ForgotPasswordRequestViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new BadRequestException("Invalid input: " + ModelStateHelper.GetErrors(ModelState), ErrorCode.InputValidateError);

                // get User by email
                var user = await _userService.GetByEmail(model.Email, default) 
                    ?? throw new NotFoundException("email not found");

                // send email with refresh passwork link
                var result = await _userService.GenerateResetPasswordTokenAsync(model);
                if (!result.Success)
                    throw new ErrorException(result.Message);

                if (result.Data == null)
                    throw new ErrorException("Failed to generate reset password token");

                var mailRequest = new MailRequestViewModel()
                {
                    ToEmail = model.Email,
                    Subject = "Forgot password",
                    Body = result.Data.EmailBody
                };

                _backgroundService.ScheduleImmediateJob<IMailService>(mailService => mailService.SendEmailAsync(mailRequest));

                return Ok(new ResponseViewModel
                {
                    Message = "Reset password mail was sent successfully.",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while sending reset password email");
                throw;
            }
        }

        [NonAction]
        private void SendConfirmationEmailAsync(string email, string confirmationLink, string userName)
        {
            var test = Directory.GetCurrentDirectory();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Template", "MailTemplate.html");
            var emailBody = MailHelper.ReadAndProcessHtmlTemplate(filePath, confirmationLink, userName);

            var mailRequestViewModel = new MailRequestViewModel
            {
                ToEmail = email,
                Subject = "Activate Your Account",
                Body = emailBody,
            };

            _backgroundService.ScheduleImmediateJob<IMailService>(mailService => mailService.SendEmailAsync(mailRequestViewModel));
        }
    }
}
