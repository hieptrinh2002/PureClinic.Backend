using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Infrastructure.Data;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Infrastructure.Repositories;

namespace Project.Infrastructure.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

    }
}
