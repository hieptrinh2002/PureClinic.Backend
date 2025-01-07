using PureLifeClinic.Core.Interfaces.IServices;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IMapper;
using PureLifeClinic.Core.Interfaces.IRepositories;
using AutoMapper;

namespace PureLifeClinic.Core.Services
{
    public class ProductService : BaseService<Product, ProductViewModel>, IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IUserContext _userContext;

        public ProductService(
            IMapper mapper,
            IProductRepository productRepository,
            IUserContext userContext)
            : base(mapper, productRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _userContext = userContext;
        }

        public async Task<ProductViewModel> Create(ProductCreateViewModel model, CancellationToken cancellationToken)
        {
            //Mapping through AutoMapper
            var entity = _mapper.Map<Product>(model);
            entity.EntryDate = DateTime.Now;
            entity.EntryBy = Convert.ToInt32(_userContext.UserId);

            return _mapper.Map<ProductViewModel>(await _productRepository.Create(entity, cancellationToken));
        }

        public async Task Update(ProductUpdateViewModel model, CancellationToken cancellationToken)
        {
            var existingData = await _productRepository.GetById(model.Id, cancellationToken) ?? throw new KeyNotFoundException($"Product with ID {model.Id} not found.");
            _mapper.Map(model, existingData);
            existingData.UpdatedDate = DateTime.Now;
            existingData.UpdatedBy = Convert.ToInt32(_userContext.UserId);
            await _productRepository.Update(existingData, cancellationToken);
        }

        public async Task Delete(int id, CancellationToken cancellationToken)
        {
            var entity = await _productRepository. GetById(id, cancellationToken);
            await _productRepository.Delete(entity, cancellationToken);
        }

        public async Task<double> PriceCheck(int productId, CancellationToken cancellationToken)
        {
            return await _productRepository.PriceCheck(productId, cancellationToken);
        }
    }
}
