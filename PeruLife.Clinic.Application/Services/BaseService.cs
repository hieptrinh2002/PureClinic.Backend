using AutoMapper;
using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Interfaces.IRepositories;

namespace PureLifeClinic.Application.Services
{
    public class BaseService<T, TViewModel> : IBaseService<TViewModel>
        where T : class
        where TViewModel : class
    {
        private readonly IMapper _mapper;
        private readonly IBaseRepository<T> _repository;

        public BaseService(
            IMapper mapper,
            IBaseRepository<T> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public virtual async Task<IEnumerable<TViewModel>> GetAll(CancellationToken cancellationToken)
        {
            return _mapper.Map<List<TViewModel>>(await _repository.GetAll(cancellationToken));
        }

        public virtual async Task<PaginatedData<TViewModel>> GetPaginatedData(
            int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var paginatedData = await _repository.GetPaginatedData(pageNumber, pageSize, cancellationToken);
            var mappedData = _mapper.Map<IEnumerable<TViewModel>>(paginatedData.Data);
            return new PaginatedData<TViewModel>(mappedData, paginatedData.TotalCount);
        }

        public virtual async Task<PaginatedData<TViewModel>> GetPaginatedData(
            int pageNumber, int pageSize, List<ExpressionFilter> filters, CancellationToken cancellationToken)
        {
            var paginatedData = await _repository.GetPaginatedData(pageNumber, pageSize, filters, cancellationToken);
            var mappedData = _mapper.Map<IEnumerable<TViewModel>>(paginatedData.Data);
            return new PaginatedData<TViewModel>(mappedData, paginatedData.TotalCount);
        }

        public virtual async Task<PaginatedData<TViewModel>> GetPaginatedData(
            int pageNumber, int pageSize, List<ExpressionFilter> filters, string sortBy, string sortOrder, CancellationToken cancellationToken)
        {
            var paginatedData = await _repository.GetPaginatedData(pageNumber, pageSize, filters, sortBy, sortOrder, cancellationToken);
            var mappedData = _mapper.Map<IEnumerable<TViewModel>>(paginatedData.Data);
            return new PaginatedData<TViewModel>(mappedData, paginatedData.TotalCount);
        }

        public virtual async Task<TViewModel> GetById<Tid>(Tid id, CancellationToken cancellationToken)
        {
            return _mapper.Map<TViewModel>(await _repository.GetById(id, cancellationToken));
        }

        public virtual async Task<bool> IsExists<Tvalue>(string key, Tvalue value, CancellationToken cancellationToken)
        {
            return await _repository.IsExists(key, value?.ToString(), cancellationToken);
        }

        public virtual async Task<bool> IsExistsForUpdate<Tid>(Tid id, string key, string value, CancellationToken cancellationToken)
        {
            return await _repository.IsExistsForUpdate(id, key, value, cancellationToken);
        }
    }
}
