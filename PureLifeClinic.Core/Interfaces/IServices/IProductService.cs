using PureLifeClinic.Core.Entities.Business;

namespace PureLifeClinic.Core.Interfaces.IServices
{
    public interface IProductService : IBaseService<ProductViewModel>
    {
        Task<ProductViewModel> Create(ProductCreateViewModel model, CancellationToken cancellationToken);
        Task Update(ProductUpdateViewModel model, CancellationToken cancellationToken);
        Task Delete(int id, CancellationToken cancellationToken);
        Task<double> PriceCheck(int productId, CancellationToken cancellationToken);
    }
}
