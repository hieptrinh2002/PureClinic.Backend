using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Infrastructure.Persistence.Data;

namespace PureLifeClinic.Infrastructure.Persistence.Repositories
{
    public class MedicalFileRepository : BaseRepository<MedicalFile>, IMedicalFileRepository
    {
        public MedicalFileRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
