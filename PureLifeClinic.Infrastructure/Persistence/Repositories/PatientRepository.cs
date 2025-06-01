using Microsoft.EntityFrameworkCore;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Infrastructure.Persistence.Data;

namespace PureLifeClinic.Infrastructure.Persistence.Repositories
{
    public class PatientRepository : BaseRepository<Patient>, IPatientRepository
    {
        public PatientRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> GetByUserId(int userId, CancellationToken cancellationToken)
        {
            return await _dbContext.Patients.Include(p => p.User)
                .Where(p => !p.IsDeleted) // Assuming IsDeleted is a property to mark soft deletion 
                .AnyAsync(p => p.UserId == userId, cancellationToken);
        }
    }
}
