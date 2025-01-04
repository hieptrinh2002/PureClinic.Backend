using Microsoft.AspNetCore.Mvc;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Interfaces.IServices;

namespace PureLifeClinic.API.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Authorize("Admin")]
    public class DoctorController : ControllerBase
    {
        private readonly ILogger<DoctorController> _logger;
        private readonly IUserService _userService;

        public DoctorController(ILogger<DoctorController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        //[HttpGet("paginated")]
        //public async Task<IActionResult> Get(int? pageNumber, int? pageSize, CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        int pageSizeValue = pageSize ?? 10;
        //        int pageNumberValue = pageNumber ?? 1;

        //        var users = await _doctorService.GetPaginatedData(pageNumberValue, pageSizeValue, cancellationToken);

        //        var response = new ResponseViewModel<PaginatedDataViewModel<DoctorViewModel>>
        //        {
        //            Success = true,
        //            Message = "Users retrieved successfully",
        //            Data = users
        //        };

        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while retrieving users");

        //        var errorResponse = new ResponseViewModel<IEnumerable<UserViewModel>>
        //        {
        //            Success = false,
        //            Message = "Error retrieving users",
        //            Error = new ErrorViewModel
        //            {
        //                Code = "ERROR_CODE",
        //                Message = ex.Message
        //            }
        //        };

        //        return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
        //    }
        //}

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            try
            {
                var users = await _userService.GetAllDoctor(cancellationToken);

                var response = new ResponseViewModel<IEnumerable<DoctorViewModel>>
                {
                    Success = true,
                    Message = "doctors retrieved successfully",
                    Data = users
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving doctors");

                var errorResponse = new ResponseViewModel<IEnumerable<DoctorViewModel>>
                {
                    Success = false,
                    Message = "Error retrieving doctor",
                    Error = new ErrorViewModel
                    {
                        Code = "ERROR_CODE",
                        Message = ex.Message
                    }
                };

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        var data = await _doctorService.GetById(id, cancellationToken);

        //        var response = new ResponseViewModel<DoctorViewModel>
        //        {
        //            Success = true,
        //            Message = "User retrieved successfully",
        //            Data = data
        //        };

        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message == "No data found")
        //        {
        //            return StatusCode(StatusCodes.Status404NotFound, new ResponseViewModel<DoctorViewModel>
        //            {
        //                Success = false,
        //                Message = "User not found",
        //                Error = new ErrorViewModel
        //                {
        //                    Code = "NOT_FOUND",
        //                    Message = "User not found"
        //                }
        //            });
        //        }

        //        _logger.LogError(ex, $"An error occurred while retrieving the user");

        //        var errorResponse = new ResponseViewModel<DoctorViewModel>
        //        {
        //            Success = false,
        //            Message = "Error retrieving user",
        //            Error = new ErrorViewModel
        //            {
        //                Code = "ERROR_CODE",
        //                Message = ex.Message
        //            }
        //        };

        //        return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
        //    }
        //}
    }
}
