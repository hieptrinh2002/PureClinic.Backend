using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Entities.Business;

namespace PureLifeClinic.Application.Interfaces.IServices
{
    public interface IBaseService<TViewModel> where TViewModel : class
    {
        Task<IEnumerable<TViewModel>> GetAll(CancellationToken cancellationToken);
        Task<PaginatedData<TViewModel>> GetPaginatedData(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<PaginatedData<TViewModel>> GetPaginatedData(int pageNumber, int pageSize, List<ExpressionFilter> filters, CancellationToken cancellationToken);
        Task<PaginatedData<TViewModel>> GetPaginatedData(int pageNumber, int pageSize, List<ExpressionFilter> filters, string sortBy, string sortOrder, CancellationToken cancellationToken);
        Task<TViewModel> GetById<Tid>(Tid id, CancellationToken cancellationToken);
        Task<bool> IsExists<Tvalue>(string key, Tvalue value, CancellationToken cancellationToken);
        Task<bool> IsExistsForUpdate<Tid>(Tid id, string key, string value, CancellationToken cancellationToken);
    }
}
