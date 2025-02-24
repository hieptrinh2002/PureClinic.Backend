using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PureLifeClinic.API.Helpers;
using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Enums;
using PureLifeClinic.Core.Exceptions;
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
        public async Task<IActionResult> Get(
            int? pageNumber, int? pageSize, string? search, string? sortBy, string? sortOrder, CancellationToken cancellationToken)
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
            var medicines = await _medicineService.GetAll(cancellationToken);

            var response = new ResponseViewModel<IEnumerable<MedicineViewModel>>
            {
                Success = true,
                Message = "Medicines retrieved successfully",
                Data = medicines
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            if (_memoryCache.TryGetValue($"Medicine_{id}", out MedicineViewModel cachedMedicine))
            {
                return Ok(new ResponseViewModel<MedicineViewModel>
                {
                    Success = true,
                    Message = "Medicine retrieved successfully",
                    Data = cachedMedicine
                });
            }

            var medicine = await _medicineService.GetById(id, cancellationToken)
                          ?? throw new NotFoundException("Medicine not found", "NOT_FOUND");

            _memoryCache.Set($"Medicine_{id}", medicine, TimeSpan.FromMinutes(10));

            return Ok(new ResponseViewModel<MedicineViewModel>
            {
                Success = true,
                Message = "Medicine retrieved successfully",
                Data = medicine
            });
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create(MedicineCreateViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) {
                throw new BadRequestException("Invalid input: " + ModelStateHelper.GetErrors(ModelState), ErrorCode.InputValidateError);
            }
            if (await _medicineService.IsExists("Name", model.Name, cancellationToken))
            {
                throw new BadRequestException($"The medicine name '{model.Name}' already exists.", ErrorCode.DuplicateNameError);
            }

            if (await _medicineService.IsExists("Code", model.Code, cancellationToken))
            {
                throw new BadRequestException($"The medicine code '{model.Code}' already exists.", ErrorCode.DuplicateCodeError);
            }
            var data = await _medicineService.Create(model, cancellationToken);

            var response = new ResponseViewModel<MedicineViewModel>
            {
                Success = true,
                Message = "Medicine created successfully",
                Data = data
            };

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(MedicineUpdateViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                throw new BadRequestException("Invalid input: " + ModelStateHelper.GetErrors(ModelState), ErrorCode.InputValidateError);

            if (await _medicineService.IsExistsForUpdate(model.Id, "Name", model.Name, cancellationToken))
                throw new BadRequestException($"The medicine name '{model.Name}' already exists.", ErrorCode.DuplicateNameError);

            if (await _medicineService.IsExistsForUpdate(model.Id, "Code", model.Code, cancellationToken))
                throw new BadRequestException($"The medicine code '{model.Code}' already exists.", ErrorCode.DuplicateCodeError);

            await _medicineService.Update(model, cancellationToken);

            // Remove data from cache
            _memoryCache.Remove($"Medicine_{model.Id}");

            var response = new ResponseViewModel
            {
                Success = true,
                Message = "Medicine updated successfully"
            };

            return Ok(response);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
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
    }
}
