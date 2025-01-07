using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Core.Interfaces.IServices;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IMapper;
using AutoMapper;

namespace PureLifeClinic.Core.Services
{
    public class RoleService : BaseService<Role, RoleViewModel>, IRoleService
    {
        private readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserContext _userContext;

        public RoleService(
            IMapper mapper,
            IRoleRepository roleRepository,
            IUserContext userContext)
            : base(mapper, roleRepository)
        {
            _mapper = mapper;
            _roleRepository = roleRepository;
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

            return _mapper.Map<RoleViewModel>(await _roleRepository.Create(entity, cancellationToken));
        }

        public async Task Update(RoleUpdateViewModel model, CancellationToken cancellationToken)
        {
            var existingData = await _roleRepository.GetById(model.Id, cancellationToken);
            //Mapping through AutoMapper
            _mapper.Map(model, existingData);

            // Set additional properties or perform other logic as needed
            existingData.UpdatedDate = DateTime.Now;
            existingData.UpdatedBy = Convert.ToInt32(_userContext.UserId);

            await _roleRepository.Update(existingData, cancellationToken);
        }

        public async Task Delete(int id, CancellationToken cancellationToken)
        {
            var entity = await _roleRepository.GetById(id, cancellationToken);
            await _roleRepository.Delete(entity, cancellationToken);
        }
    }
}
