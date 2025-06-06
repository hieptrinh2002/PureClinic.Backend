﻿using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PureLifeClinic.API.Attributes;
using PureLifeClinic.API.Helpers;
using PureLifeClinic.Application.BusinessObjects.ErrorViewModels;
using PureLifeClinic.Application.BusinessObjects.ProductViewModels;
using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Common.Constants;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Enums;
using PureLifeClinic.Core.Enums.PermissionEnums;

namespace PureLifeClinic.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [AllowAnonymous]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;
        private readonly IMemoryCache _memoryCache;

        public ProductController(ILogger<ProductController> logger, IProductService productService, IMemoryCache memoryCache)
        {
            _logger = logger;
            _productService = productService;
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
                    // Add filters for relevant properties
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

                var products = await _productService.GetPaginatedData(pageNumberValue, pageSizeValue, filters, sortBy, sortOrder, cancellationToken);

                var response = new ResponseViewModel<PaginatedData<ProductViewModel>>
                {
                    Success = true,
                    Message = "Products retrieved successfully",
                    Data = products
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving products");

                var errorResponse = new ResponseViewModel<IEnumerable<ProductViewModel>>
                {
                    Success = false,
                    Message = "Error retrieving products",
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
        [PermissionAuthorize(ResourceConstants.Product, PermissionOperator.And, PermissionAction.View, PermissionAction.CreateDelete)]
        //[Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            try
            {
                var products = await _productService.GetAll(cancellationToken);

                var response = new ResponseViewModel<IEnumerable<ProductViewModel>>
                {
                    Success = true,
                    Message = "Products retrieved successfully",
                    Data = products
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving products");

                var errorResponse = new ResponseViewModel<IEnumerable<ProductViewModel>>
                {
                    Success = false,
                    Message = "Error retrieving products",
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
                var product = new ProductViewModel();

                // Attempt to retrieve the product from the cache
                if (_memoryCache.TryGetValue($"Product_{id}", out ProductViewModel cachedProduct))
                {
                    product = cachedProduct;
                }
                else
                {
                    // If not found in cache, fetch the product from the data source
                    product = await _productService.GetById(id, cancellationToken);

                    if (product != null)
                    {
                        // Cache the product with an expiration time of 10 minutes
                        _memoryCache.Set($"Product_{id}", product, TimeSpan.FromMinutes(10));
                    }
                }

                var response = new ResponseViewModel<ProductViewModel>
                {
                    Success = true,
                    Message = "Product retrieved successfully",
                    Data = product
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "No data found")
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseViewModel<ProductViewModel>
                    {
                        Success = false,
                        Message = "Product not found",
                        Error = new ErrorViewModel
                        {
                            Code = "NOT_FOUND",
                            Message = "Product not found"
                        }
                    });
                }

                _logger.LogError(ex, $"An error occurred while retrieving the product");

                var errorResponse = new ResponseViewModel<ProductViewModel>
                {
                    Success = false,
                    Message = "Error retrieving product",
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
        public async Task<IActionResult> Create(ProductCreateViewModel model, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                string message = "";
                if (await _productService.IsExists("Name", model.Name, cancellationToken))
                {
                    message = $"The product name- '{model.Name}' already exists";
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel<ProductViewModel>
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

                if (await _productService.IsExists("Code", model.Code, cancellationToken))
                {
                    message = $"The product code- '{model.Code}' already exists";
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel<ProductViewModel>
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
                    var data = await _productService.Create(model, cancellationToken);

                    var response = new ResponseViewModel<ProductViewModel>
                    {
                        Success = true,
                        Message = "Product created successfully",
                        Data = data
                    };

                    return Ok(response);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred while adding the product");
                    message = $"An error occurred while adding the product- " + ex.Message;

                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel<ProductViewModel>
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

            return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel<ProductViewModel>
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
        [AllowAnonymous]
        public async Task<IActionResult> Edit(ProductUpdateViewModel model, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                string message = "";
                if (await _productService.IsExistsForUpdate(model.Id, "Name", model.Name, cancellationToken))
                {
                    message = $"The product name- '{model.Name}' already exists";
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

                if (await _productService.IsExistsForUpdate(model.Id, "Code", model.Code, cancellationToken))
                {
                    message = $"The product code- '{model.Code}' already exists";
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
                    await _productService.Update(model, cancellationToken);

                    // Remove data from cache by key
                    _memoryCache.Remove($"Product_{model.Id}");

                    var response = new ResponseViewModel
                    {
                        Success = true,
                        Message = "Product updated successfully"
                    };

                    return Ok(response);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred while updating the product");
                    message = $"An error occurred while updating the product- " + ex.Message;

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
                await _productService.Delete(id, cancellationToken);

                // Remove data from cache by key
                _memoryCache.Remove($"Product_{id}");

                var response = new ResponseViewModel
                {
                    Success = true,
                    Message = "Product deleted successfully"
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
                        Message = "Product not found",
                        Error = new ErrorViewModel
                        {
                            Code = "NOT_FOUND",
                            Message = "Product not found"
                        }
                    });
                }

                _logger.LogError(ex, "An error occurred while deleting the product");

                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel
                {
                    Success = false,
                    Message = "Error deleting the product",
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
