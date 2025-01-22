using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PureLifeClinic.API.Helpers;
using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Interfaces.IServices;

namespace PureLifeClinic.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly ILogger<MedicineController> _logger;
        private readonly IMedicineService _medicineService;
        private readonly IMemoryCache _memoryCache;

        public MedicineController(ILogger<MedicineController> logger, IMedicineService medicineService, IMemoryCache memoryCache)
        {
            _logger = logger;
            _medicineService = medicineService;
            _memoryCache = memoryCache;
        }

        [HttpGet("paginated-data")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int? pageNumber, int? pageSize, string? search, string? sortBy, string? sortOrder, CancellationToken cancellationToken)
        {
            try
            {
                int pageSizeValue = pageSize ?? 10;
                int pageNumberValue = pageNumber ?? 1;
                sortBy = sortBy ?? "Id";
                sortOrder = sortOrder ?? "desc";

                var filters = new List<ExpressionFilter>();
                if (!string.IsNullOrWhiteSpace(search) && search != null)
                {
                    filters.AddRange(new[]
                    {
                        new ExpressionFilter
                        {
                            PropertyName = "Code",
                            Value = search,
                            Comparison = Comparison.Contains
                        },
                        new ExpressionFilter
                        {
                            PropertyName = "Name",
                            Value = search,
                            Comparison = Comparison.Contains
                        },
                        new ExpressionFilter
                        {
                            PropertyName = "Description",
                            Value = search,
                            Comparison = Comparison.Contains
                        }
                    });

                    // Check if the search string represents a valid numeric value for the "Price" property
                    if (double.TryParse(search, out double price))
                    {
                        filters.Add(new ExpressionFilter
                        {
                            PropertyName = "Price",
                            Value = price,
                            Comparison = Comparison.Equal
                        });
                    }
                }

                var medicines = await _medicineService.GetPaginatedData(pageNumberValue, pageSizeValue, filters, sortBy, sortOrder, cancellationToken);

                var response = new ResponseViewModel<PaginatedDataViewModel<MedicineViewModel>>
                {
                    Success = true,
                    Message = "Medicine retrieved successfully",
                    Data = medicines
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving medicines");

                var errorResponse = new ResponseViewModel<IEnumerable<MedicineViewModel>>
                {
                    Success = false,
                    Message = "Error retrieving medicines",
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
                var medicines = await _medicineService.GetAll(cancellationToken);

                var response = new ResponseViewModel<IEnumerable<MedicineViewModel>>
                {
                    Success = true,
                    Message = "Medicines retrieved successfully",
                    Data = medicines
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving medicines");

                var errorResponse = new ResponseViewModel<IEnumerable<MedicineViewModel>>
                {
                    Success = false,
                    Message = "Error retrieving medicines",
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
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            try
            {
                var medicine = new MedicineViewModel();

                // Attempt to retrieve the medicine from the cache
                if (_memoryCache.TryGetValue($"Medicine_{id}", out MedicineViewModel cachedMedicine))
                {
                    medicine = cachedMedicine;
                }
                else
                {
                    // If not found in cache, fetch the medicine from the data source
                    medicine = await _medicineService.GetById(id, cancellationToken);

                    if (medicine != null)
                    {
                        // Cache the medicine with an expiration time of 10 minutes
                        _memoryCache.Set($"Medicine_{id}", medicine, TimeSpan.FromMinutes(10));
                    }
                }

                var response = new ResponseViewModel<MedicineViewModel>
                {
                    Success = true,
                    Message = "Medicine retrieved successfully",
                    Data = medicine
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "No data found")
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseViewModel<MedicineViewModel>
                    {
                        Success = false,
                        Message = "Medicine not found",
                        Error = new ErrorViewModel
                        {
                            Code = "NOT_FOUND",
                            Message = "Medicine not found"
                        }
                    });
                }

                _logger.LogError(ex, $"An error occurred while retrieving the medicine");

                var errorResponse = new ResponseViewModel<MedicineViewModel>
                {
                    Success = false,
                    Message = "Error retrieving medicine",
                    Error = new ErrorViewModel
                    {
                        Code = "ERROR_CODE",
                        Message = ex.Message
                    }
                };

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create(MedicineCreateViewModel model, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                string message = "";
                if (await _medicineService.IsExists("Name", model.Name, cancellationToken))
                {
                    message = $"The medicine name- '{model.Name}' already exists";
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel<MedicineViewModel>
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

                if (await _medicineService.IsExists("Code", model.Code, cancellationToken))
                {
                    message = $"The medicine code- '{model.Code}' already exists";
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel<MedicineViewModel>
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
                    var data = await _medicineService.Create(model, cancellationToken);

                    var response = new ResponseViewModel<MedicineViewModel>
                    {
                        Success = true,
                        Message = "Medicine created successfully",
                        Data = data
                    };

                    return Ok(response);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred while adding the medicine");
                    message = $"An error occurred while adding the medicine- " + ex.Message;

                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel<MedicineViewModel>
                    {
                        Success = false,
                        Message = message,
                        Error = new ErrorViewModel
                        {
                            Code = "ADD_ROLE_ERROR",
                            Message = message
                        }
                    });
                }
            }

            return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel<MedicineViewModel>
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
        public async Task<IActionResult> Edit(MedicineUpdateViewModel model, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                string message = "";
                if (await _medicineService.IsExistsForUpdate(model.Id, "Name", model.Name, cancellationToken))
                {
                    message = $"The medicine name- '{model.Name}' already exists";
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel
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

                if (await _medicineService.IsExistsForUpdate(model.Id, "Code", model.Code, cancellationToken))
                {
                    message = $"The medicine code- '{model.Code}' already exists";
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel
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
                    await _medicineService.Update(model, cancellationToken);

                    // Remove data from cache by key
                    _memoryCache.Remove($"Medicine_{model.Id}");

                    var response = new ResponseViewModel
                    {
                        Success = true,
                        Message = "Medicine updated successfully"
                    };

                    return Ok(response);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred while updating the medicine");
                    message = $"An error occurred while updating the medicine- " + ex.Message;

                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel
                    {
                        Success = false,
                        Message = message,
                        Error = new ErrorViewModel
                        {
                            Code = "UPDATE_ROLE_ERROR",
                            Message = message
                        }
                    });
                }
            }

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _medicineService.Delete(id, cancellationToken);

                // Remove data from cache by key
                _memoryCache.Remove($"Medicine_{id}");

                var response = new ResponseViewModel
                {
                    Success = true,
                    Message = "Medicine deleted successfully"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "No data found")
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseViewModel
                    {
                        Success = false,
                        Message = "Medicine not found",
                        Error = new ErrorViewModel
                        {
                            Code = "NOT_FOUND",
                            Message = "Medicine not found"
                        }
                    });
                }

                _logger.LogError(ex, "An error occurred while deleting the medicine");

                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel
                {
                    Success = false,
                    Message = "Error deleting the medicine",
                    Error = new ErrorViewModel
                    {
                        Code = "DELETE_ROLE_ERROR",
                        Message = ex.Message
                    }
                });
            }
        }
    }
}
