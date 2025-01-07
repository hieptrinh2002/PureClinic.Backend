using AutoMapper;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Core.Interfaces.IServices;

namespace PureLifeClinic.Core.Services
{
    public class RoleService : BaseService<Role, RoleViewModel>, IRoleService
    {
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _unitOfWork;    
        public RoleService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IUserContext userContext)
            : base(mapper, unitOfWork.Roles)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userContext = userContext;
        }

        public async Task<RoleViewModel> Create(RoleCreateViewModel model, CancellationToken cancellationToken)
        {
            //Mapping through AutoMapper
            var entity = _mapper.Map<Role>(model);

            // Set additional properties or perform other logic as needed
            entity.NormalizedName = model.Name.ToUpper();
            entity.EntryDate = DateTime.Now;
            entity.EntryBy = Convert.ToInt32(_userContext.UserId);
            var createdRole = await _unitOfWork.Roles.Create(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<RoleViewModel>(createdRole);
        }

        public async Task Update(RoleUpdateViewModel model, CancellationToken cancellationToken)
        {
            var existingData = await _unitOfWork.Roles.GetById(model.Id, cancellationToken);
            //Mapping through AutoMapper
            _mapper.Map(model, existingData);

            // Set additional properties or perform other logic as needed
            existingData.UpdatedDate = DateTime.Now;
            existingData.UpdatedBy = Convert.ToInt32(_userContext.UserId);

            await _unitOfWork.Roles.Update(existingData, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task Delete(int id, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Roles.GetById(id, cancellationToken);
            await _unitOfWork.Roles.Delete(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
