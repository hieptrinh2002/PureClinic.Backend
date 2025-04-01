using PureLifeClinic.Application.BusinessObjects.RoleViewModels.Request;
using PureLifeClinic.Application.BusinessObjects.RoleViewModels.Response;

namespace PureLifeClinic.Application.Interfaces.IServices
{
    public interface IRoleService : IBaseService<RoleViewModel>
    {
        Task<RoleViewModel> Create(RoleCreateViewModel model, CancellationToken cancellationToken);
        Task Update(RoleUpdateViewModel model, CancellationToken cancellationToken);
        Task Delete(int id, CancellationToken cancellationToken);
    }
}
