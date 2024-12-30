using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Core.Interfaces.IRepositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<double> PriceCheck(int productId, CancellationToken cancellationToken);
    }
}
