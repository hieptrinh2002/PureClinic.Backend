using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PureLifeClinic.API.Helpers;
using PureLifeClinic.Application.BusinessObjects.EmailViewModels;
using PureLifeClinic.Application.BusinessObjects.ErrorViewModels;
using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Application.BusinessObjects.UserViewModels;
using PureLifeClinic.Application.Interfaces.IBackgroundJobs;
using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Exceptions;
using PureLifeClinic.Infrastructure.ExternalServices.Email;

namespace PureLifeClinic.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IMailService _emailService;    
        private readonly IBackgroundJobService _backgroundJobService;
        private readonly IEmailTemplateService _emailTemplateService;
        public UserController(
            ILogger<UserController> logger, 
            IUserService userService, 
            IMailService emailService, 
            IBackgroundJobService backgroundJobService,
            IEmailTemplateService emailTemplateService)
        {
            _logger = logger;
            _userService = userService;
            _emailService = emailService;
            _backgroundJobService = backgroundJobService;
            _emailTemplateService = emailTemplateService;
        }

        [HttpGet("paginated")]
        public async Task<IActionResult> Get(int? pageNumber, int? pageSize, CancellationToken cancellationToken)
        {
            try
            {
                int pageSizeValue = pageSize ?? 10;
                int pageNumberValue = pageNumber ?? 1;

                var users = await _userService.GetPaginatedData(pageNumberValue, pageSizeValue, cancellationToken);

                var response = new ResponseViewModel<PaginatedData<UserViewModel>>
                {
                    Success = true,
                    Message = "Users retrieved successfully",
                    Data = users
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving users");

                var errorResponse = new ResponseViewModel<IEnumerable<UserViewModel>>
                {
                    Success = false,
                    Message = "Error retrieving users",
                    Error = new ErrorViewModel
                    {
                        Code = "ERROR_CODE",
                        Message = ex.Message
                    }
                };

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            try
            {
                var users = await _userService.GetAll(cancellationToken);

                var response = new ResponseViewModel<IEnumerable<UserViewModel>>
                {
                    Success = true,
                    Message = "Users retrieved successfully",
                    Data = users
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving users");

                var errorResponse = new ResponseViewModel<IEnumerable<UserViewModel>>
                {
                    Success = false,
                    Message = "Error retrieving users",
                    Error = new ErrorViewModel
                    {
                        Code = "ERROR_CODE",
                        Message = ex.Message
                    }
                };

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            try
            {
                var data = await _userService.GetById(id, cancellationToken);

                var response = new ResponseViewModel<UserViewModel>
                {
                    Success = true,
                    Message = "User retrieved successfully",
                    Data = data
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "No data found")
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseViewModel<UserViewModel>
                    {
                        Success = false,
                        Message = "User not found",
                        Error = new ErrorViewModel
                        {
                            Code = "NOT_FOUND",
                            Message = "User not found"
                        }
                    });
                }

                _logger.LogError(ex, $"An error occurred while retrieving the user");

                var errorResponse = new ResponseViewModel<UserViewModel>
                {
                    Success = false,
                    Message = "Error retrieving user",
                    Error = new ErrorViewModel
                    {
                        Code = "ERROR_CODE",
                        Message = ex.Message
                    }
                };

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Create(UserCreateViewModel model, CancellationToken cancellationToken)
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
                        if(!result.Success)
                            throw new ErrorException(result.Message);    
                        var confirmationLink = Url.Action("ConfirmEmail", "Register", new { result.Data.ActivationToken, email = model.Email }, Request.Scheme);

                        //var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Template", "MailTemplate.html");
                        //var emailBody = MailHelper.ReadAndProcessHtmlTemplate(filePath, confirmationLink, model.UserName);

                        var dict = new Dictionary<string, string>()
                        {
                            { "{{UserName}}", model.UserName },
                            { "{{ActivationLink}}", confirmationLink},
                            { "{{ResetPasswordLink}}", "" },
                            { "{{Year}}", DateTime.Now.Year.ToString() },
                            { "{{UserEmail}}", "johndoe@example.com" }
                        };

                        var emailBody = await _emailTemplateService.RenderTemplateAsync("MailTemplate.html", dict);

                        var mailRequestViewModel = new MailRequestViewModel
                        {
                            ToEmail = model.Email,
                            Subject = "Activate Your Account",
                            Body = emailBody,
                        };

                        _backgroundJobService.ScheduleImmediateJob<IMailService>(m => m.SendEmailAsync(mailRequestViewModel));
                       
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

        [HttpPut]
        public async Task<IActionResult> Edit(UserUpdateViewModel model, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                string message = "";
                if (await _userService.IsExistsForUpdate(model.Id, "UserName", model.UserName, cancellationToken))
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

                if (await _userService.IsExistsForUpdate(model.Id, "Email", model.Email, cancellationToken))
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
                    var response = await _userService.Update(model, cancellationToken);

                    if (response.Success)
                    {
                        return Ok(response);
                    }
                    else
                    {
                        return BadRequest(response);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred while updating the user");
                    message = $"An error occurred while updating the user- " + ex.Message;

                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel<UserViewModel>
                    {
                        Success = false,
                        Message = message,
                        Error = new ErrorViewModel
                        {
                            Code = "UPDATE_USER_ERROR",
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _userService.Delete(id, cancellationToken);

                var response = new ResponseViewModel<UserViewModel>
                {
                    Success = true,
                    Message = "User deleted successfully"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the user");

                var errorResponse = new ResponseViewModel<UserViewModel>
                {
                    Success = false,
                    Message = "Error deleting the user",
                    Error = new ErrorViewModel
                    {
                        Code = "DELETE_USER_ERROR",
                        Message = ex.Message
                    }
                };

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
    }
}
