using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using PureLifeClinic.API.Controllers.V1;
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
