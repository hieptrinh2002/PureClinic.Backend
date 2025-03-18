using Microsoft.EntityFrameworkCore;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Infrastructure.Persistence.Data;

namespace PureLifeClinic.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<double> PriceCheck(int productId, CancellationToken cancellationToken = default)
        {
            var price = await _dbContext.Products
                .Where(x => x.Id == productId)
                .Select(x => x.Price)
                .FirstOrDefaultAsync(cancellationToken);
            return price;
        }
    }
}
