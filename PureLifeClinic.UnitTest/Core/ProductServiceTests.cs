using AutoMapper;
using Moq;
using PureLifeClinic.Application.BusinessObjects.ProductViewModels;
using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Application.Services;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IRepositories;

namespace PureLifeClinic.UnitTest.Core
{
    public class ProductServiceTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<IUserContext> _mockUserContext;
        private readonly ProductService _productService;
        private readonly IUnitOfWork _unitOfWork;
        public ProductServiceTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockUserContext = new Mock<IUserContext>();
            _productService = new ProductService(
                _mockMapper.Object,
                _mockUserContext.Object,
                _unitOfWork
            );
        }

        private ProductCreateViewModel CreateProductCreateViewModel() => new ProductCreateViewModel
        {
            Code = "123",
            Name = "Test Product",
            Price = 10.2,
            Quantity = 10,
            Description = "Description",
            IsActive = true
        };

        private Product CreateProductEntity() => new Product
        {
            Code = "123",
            Name = "Test Product",
            Price = 10.2,
            Quantity = 10,
            Description = "Description",
            IsActive = true
        };

        private ProductViewModel CreateProductViewModel() => new ProductViewModel
        {
            Code = "123",
            Name = "Test Product",
            Price = 10.2,
            Quantity = 10,
            Description = "Description",
            IsActive = true
        };

        [Fact]
        public async Task Create_ShouldCallRepositoryAndReturnMappedViewModel()
        {
            // Arrange
            var model = CreateProductCreateViewModel();
            var productEntity = CreateProductEntity();
            var productViewModel = CreateProductViewModel();

            _mockMapper.Setup(mapper => mapper.Map<Product>(model)).Returns(productEntity);
            _mockProductRepository.Setup(repo => repo.Create(It.IsAny<Product>(), It.IsAny<CancellationToken>())).ReturnsAsync(productEntity);
            _mockMapper.Setup(mapper => mapper.Map<ProductViewModel>(productEntity)).Returns(productViewModel);
            _mockUserContext.Setup(u => u.UserId).Returns("1");

            // Act
            var result = await _productService.Create(model, CancellationToken.None);

            // Assert
            _mockMapper.Verify(mapper => mapper.Map<Product>(model), Times.Once);
            _mockProductRepository.Verify(repo => repo.Create(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<ProductViewModel>(productEntity), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(productViewModel.Code, result.Code);
        }

        [Fact]
        public async Task Update_ShouldCallRepositoryAndUpdateExistingProduct()
        {
            // Arrange
            var model = new ProductUpdateViewModel { Id = 1, Name = "Updated Product" };
            var existingProduct = new Product { Id = 1, Name = "Old Product" };

            _mockProductRepository.Setup(repo => repo.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(existingProduct);
            _mockMapper.Setup(mapper => mapper.Map(model, existingProduct)).Verifiable();
            _mockUserContext.Setup(u => u.UserId).Returns("123");

            // Act
            await _productService.Update(model, CancellationToken.None);

            // Assert
            _mockProductRepository.Verify(repo => repo.Update(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map(model, existingProduct), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldCallRepositoryAndDeleteProduct()
        {
            // Arrange
            var productId = 1;
            var productEntity = CreateProductEntity();

            _mockProductRepository.Setup(repo => repo.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(productEntity);
            _mockProductRepository.Setup(repo => repo.Delete(It.IsAny<Product>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            await _productService.Delete(productId, CancellationToken.None);

            // Assert
            _mockProductRepository.Verify(repo => repo.Delete(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task PriceCheck_ShouldReturnPrice()
        {
            // Arrange
            var productId = 1;
            var expectedPrice = 100.0;

            _mockProductRepository.Setup(repo => repo.PriceCheck(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedPrice);

            // Act
            var result = await _productService.PriceCheck(productId, CancellationToken.None);

            // Assert
            Assert.Equal(expectedPrice, result);
        }
    }
}
