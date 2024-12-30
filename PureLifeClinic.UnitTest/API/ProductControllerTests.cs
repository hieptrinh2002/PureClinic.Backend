using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using PureLifeClinic.API.Controllers.V1;
using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Interfaces.IServices;

namespace PureLifeClinic.UnitTest.API
{
    public class ProductControllerTests
    {
        private readonly Mock<ILogger<ProductController>> _mockLogger;
        private readonly Mock<IProductService> _mockProductService;
        private readonly Mock<IMemoryCache> _mockMemoryCache;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _mockLogger = new Mock<ILogger<ProductController>>();
            _mockProductService = new Mock<IProductService>();
            _mockMemoryCache = new Mock<IMemoryCache>();
            _controller = new ProductController(_mockLogger.Object, _mockProductService.Object, _mockMemoryCache.Object);
        }

        [Fact]
        public async Task Get_ShouldReturnFilteredProducts_WhenSearchIsProvided()
        {
            // Arrange
            string searchQuery = "Product";
            var mockProducts = new List<ProductViewModel>
            {
                new ProductViewModel { Id = 1, Name = "Product 1", Code = "P001" },
                new ProductViewModel { Id = 2, Name = "Product 2", Code = "P002" }
            };
            var paginatedData = new PaginatedDataViewModel<ProductViewModel>(mockProducts, mockProducts.Count);

            _mockProductService.Setup(service => service.GetPaginatedData(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.Is<List<ExpressionFilter>>(filters =>
                    filters.Any(f => f.PropertyName == "Code" && f.Value == searchQuery) &&
                    filters.Any(f => f.PropertyName == "Name" && f.Value == searchQuery) &&
                    filters.Any(f => f.PropertyName == "Description" && f.Value == searchQuery)),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(paginatedData);

            // Act
            var result = await _controller.Get(1, 10, searchQuery, null, null, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseViewModel<PaginatedDataViewModel<ProductViewModel>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal(2, response.Data.Data.Count()); 
            Assert.Equal(mockProducts.Count, response.Data.TotalCount);


            // check method GetPaginatedData only called oncetime
            _mockProductService.Verify(service => service.GetPaginatedData(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.Is<List<ExpressionFilter>>(filters =>
                    filters.Any(f => f.PropertyName == "Code" && f.Value == searchQuery) &&
                    filters.Any(f => f.PropertyName == "Name" && f.Value == searchQuery) &&
                    filters.Any(f => f.PropertyName == "Description" && f.Value == searchQuery)),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }


        [Fact]
        public async Task Get_ShouldReturnPaginatedProducts_WhenValidRequest()
        {
            // Arrange
            var mockProducts = new List<ProductViewModel>
            {
                new ProductViewModel { Id = 1, Name = "Product 1", Code = "P001" },
                new ProductViewModel { Id = 2, Name = "Product 2", Code = "P002" }
            };
            var paginatedData = new PaginatedDataViewModel<ProductViewModel>(mockProducts, mockProducts.Count);

            _mockProductService.Setup(service => service.GetPaginatedData(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<ExpressionFilter>>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())
            ).ReturnsAsync(paginatedData);

            // Act
            var result = await _controller.Get(1, 10, null, null, null, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseViewModel<PaginatedDataViewModel<ProductViewModel>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal("Products retrieved successfully", response.Message);
            Assert.NotNull(response.Data);
            Assert.Equal(2, response.Data.Data.Count()); // Verify the number of products
            Assert.Equal(mockProducts.Count, response.Data.TotalCount); // Verify the total count of products
        }

        [Fact]
        public async Task Get_ShouldReturnError_WhenExceptionOccurs()
        {
            // Arrange
            _mockProductService.Setup(service => service.GetPaginatedData(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<ExpressionFilter>>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())
            ).ThrowsAsync(new System.Exception("Database error"));

            // Act
            var result = await _controller.Get(1, 10, null, null, null, CancellationToken.None);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            var errorResponse = Assert.IsType<ResponseViewModel<IEnumerable<ProductViewModel>>>(statusCodeResult.Value);
            Assert.False(errorResponse.Success);
            Assert.Equal("Error retrieving products", errorResponse.Message);
            Assert.NotNull(errorResponse.Error);
            Assert.Equal("ERROR_CODE", errorResponse.Error.Code);
        }

        [Fact]
        public async Task Get_ShouldReturnAllProducts_WhenProductsExist()
        {
            // Arrange
            var mockProducts = new List<ProductViewModel>
            {
                new ProductViewModel { Id = 1, Name = "Product 1", Code = "P001" },
                new ProductViewModel { Id = 2, Name = "Product 2", Code = "P002" }
            };

            _mockProductService.Setup(service => service.GetAll(It.IsAny<CancellationToken>()))
                               .ReturnsAsync(mockProducts);

            // Act
            var result = await _controller.Get(CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseViewModel<IEnumerable<ProductViewModel>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal(2, response.Data.ToList().Count);
        }

        [Fact]
        public async Task Get_ShouldReturn404_WhenProductNotFound()
        {
            // Arrange
            _mockProductService.Setup(service => service.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                               .ThrowsAsync(new KeyNotFoundException("No data found"));

            // Act
            var result = await _controller.Get(1, CancellationToken.None);

            // Assert
            var notFoundResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenDuplicateName()
        {
            // Arrange
            var model = new ProductCreateViewModel { Name = "Product 1", Code = "P001" };
            _mockProductService.Setup(service => service.IsExists("Name", model.Name, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(true);

            // Act
            var result = await _controller.Create(model, CancellationToken.None);

            // Assert
            var badRequestResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task Delete_ShouldReturnOk_WhenProductDeleted()
        {
            // Arrange
            var productId = 1;
            _mockProductService.Setup(service => service.Delete(productId, It.IsAny<CancellationToken>()))
                               .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(productId, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseViewModel>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal("Product deleted successfully", response.Message);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = 1;
            _mockProductService.Setup(service => service.Delete(productId, It.IsAny<CancellationToken>()))
                               .ThrowsAsync(new KeyNotFoundException("No data found"));

            // Act
            var result = await _controller.Delete(productId, CancellationToken.None);

            // Assert
            var notFoundResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
    }
}
