using AutoMapper;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Core.Interfaces.IServices;

namespace PureLifeClinic.Core.Services
{
    public class ProductService : BaseService<Product, ProductViewModel>, IProductService
    {
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(
            IMapper mapper,
            IUserContext userContext,
            IUnitOfWork unitOfWork)
            : base(mapper, unitOfWork.Products)
        {
            _mapper = mapper;
            _userContext = userContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductViewModel> Create(ProductCreateViewModel model, CancellationToken cancellationToken)
        {
            //Mapping through AutoMapper
            var entity = _mapper.Map<Product>(model);
            entity.EntryDate = DateTime.Now;
            entity.EntryBy = Convert.ToInt32(_userContext.UserId);

            var result = await _unitOfWork.Products.Create(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<ProductViewModel>(result);
        }

        public async Task Update(ProductUpdateViewModel model, CancellationToken cancellationToken)
        {
            var existingData = await _unitOfWork.Products.GetById(model.Id, cancellationToken) 
                ?? throw new KeyNotFoundException($"Product with ID {model.Id} not found.");

            _mapper.Map(model, existingData);
            existingData.UpdatedDate = DateTime.Now;
            existingData.UpdatedBy = Convert.ToInt32(_userContext.UserId);
            await _unitOfWork.Products.Update(existingData, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

        }

        public async Task Delete(int id, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Products. GetById(id, cancellationToken);
            await _unitOfWork.Products.Delete(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<double> PriceCheck(int productId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Products.PriceCheck(productId, cancellationToken);
        }
    }
}
