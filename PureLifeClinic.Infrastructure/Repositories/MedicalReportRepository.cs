using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Infrastructure.Data;

namespace PureLifeClinic.Infrastructure.Repositories
{
    public class MedicalReportRepository : BaseRepository<MedicalReport>, IMedicalReportRepository
    {
        public MedicalReportRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
