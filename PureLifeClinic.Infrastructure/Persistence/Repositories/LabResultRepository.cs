using Microsoft.EntityFrameworkCore;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Infrastructure.Persistence.Data;

namespace PureLifeClinic.Infrastructure.Persistence.Repositories
{
    public class LabResultRepository : BaseRepository<LabResult>, ILabResultRepository
    {
        public LabResultRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IQueryable<LabResult>> QueryAsync(int patientId)
        {
            return _dbContext.Set<LabResult>()
                .AsNoTracking()
                .Where(l => l.PatientId == patientId) // đảm bảo có thuộc tính này
                .Include(l => l.Results)
                .AsQueryable();
        }

        public async Task<LabResult?> GetByIdAsync(int patientId, int id)
        {
            return await _dbContext.Set<LabResult>()
                .Include(l => l.Results)
                .FirstOrDefaultAsync(l => l.Id == id && l.PatientId == patientId);
        }

        public void Update(LabResult entity)
        {
            _dbContext.Set<LabResult>().Update(entity);
        }
    }
}
