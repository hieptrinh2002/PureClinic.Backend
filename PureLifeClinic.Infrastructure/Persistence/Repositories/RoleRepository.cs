using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Exceptions;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Infrastructure.Persistence.Data;

namespace PureLifeClinic.Infrastructure.Persistence.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public Role GetPatientRole()
        {
            var role = _dbContext.Roles.Where(r => r.NormalizedName == "PATIENT")?.FirstOrDefault();
            return role == null ? throw new ErrorException("role not found") : role;
        }
    }
}
