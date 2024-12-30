using Moq;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IMapper;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Core.Interfaces.IServices;
using PureLifeClinic.Core.Services;

namespace Project.UnitTest
{
    public class ProductServiceTests
    {
        private readonly Mock<IBaseMapper<Product, ProductViewModel>> _mockProductViewModelMapper;
        private readonly Mock<IBaseMapper<ProductCreateViewModel, Product>> _mockProductCreateMapper;
        private readonly Mock<IBaseMapper<ProductUpdateViewModel, Product>> _mockProductUpdateMapper;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<IUserContext> _mockUserContext;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _mockProductViewModelMapper = new Mock<IBaseMapper<Product, ProductViewModel>>();
            _mockProductCreateMapper = new Mock<IBaseMapper<ProductCreateViewModel, Product>>();
            _mockProductUpdateMapper = new Mock<IBaseMapper<ProductUpdateViewModel, Product>>();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockUserContext = new Mock<IUserContext>();

            _productService = new ProductService(
                _mockProductViewModelMapper.Object,
                _mockProductCreateMapper.Object,
                _mockProductUpdateMapper.Object,
                _mockProductRepository.Object,
                _mockUserContext.Object
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

            _mockProductCreateMapper.Setup(m => m.MapModel(It.IsAny<ProductCreateViewModel>())).Returns(productEntity);
            _mockProductRepository.Setup(repo => repo.Create(It.IsAny<Product>(), It.IsAny<CancellationToken>())).ReturnsAsync(productEntity);
            _mockProductViewModelMapper.Setup(m => m.MapModel(It.IsAny<Product>())).Returns(productViewModel);
            _mockUserContext.Setup(u => u.UserId).Returns("1");

            // Act
            var result = await _productService.Create(model, CancellationToken.None);

            // Assert
            _mockProductCreateMapper.Verify(m => m.MapModel(It.IsAny<ProductCreateViewModel>()), Times.Once);
            _mockProductRepository.Verify(repo => repo.Create(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockProductViewModelMapper.Verify(m => m.MapModel(It.IsAny<Product>()), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(productViewModel.Id, result.Id);
        }

        [Fact]
        public async Task Update_ShouldCallRepositoryAndUpdateExistingProduct()
        {
            // Arrange
            var model = new ProductUpdateViewModel { Id = 1, Name = "Updated Product" };
            var existingProduct = new Product { Id = 1, Name = "Old Product" };

            _mockProductRepository.Setup(repo => repo.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(existingProduct);
            _mockProductUpdateMapper.Setup(m => m.MapModel(It.IsAny<ProductUpdateViewModel>(), It.IsAny<Product>())).Verifiable();
            _mockUserContext.Setup(u => u.UserId).Returns("123");

            // Act
            await _productService.Update(model, CancellationToken.None);

            // Assert
            _mockProductRepository.Verify(repo => repo.Update(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockProductUpdateMapper.Verify(m => m.MapModel(It.IsAny<ProductUpdateViewModel>(), It.IsAny<Product>()), Times.Once);
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
