
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Infrastructure.Data;
using PureLifeClinic.Infrastructure.Repositories;

namespace PureLifeClinic.UnitTest.Infrastructure
{
    public class ProductRepositoryTests
    {
        private Mock<ApplicationDbContext> _dbContextMock;
        private ProductRepository _productRepository;

        public ProductRepositoryTests()
        {
            _dbContextMock = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            _productRepository = new ProductRepository(_dbContextMock.Object);
        }

        [Fact]
        public async Task AddAsync_ValidProduct_ReturnsAddedProduct()
        {

            // Arrange
            var newProduct = new Product
            {
                Code = "P001",
                Name = "Sample Product",
                Price = 9.99f,
                IsActive = true
            };

            var productDbSetMock = new Mock<DbSet<Product>>();

            _dbContextMock.Setup(db => db.Set<Product>())
                          .Returns(productDbSetMock.Object);

            productDbSetMock.Setup(dbSet => dbSet.AddAsync(newProduct, default)).ReturnsAsync((EntityEntry<Product>)null);

            // Act
            var result = await _productRepository.Create(newProduct, It.IsAny<CancellationToken>());


            // Assert
            Assert.NotNull(result);
            Assert.Equal(result, newProduct);
        }

        // Add more test methods for other repository operations, such as GetByIdAsync, UpdateAsync, DeleteAsync, etc.
    }
}
