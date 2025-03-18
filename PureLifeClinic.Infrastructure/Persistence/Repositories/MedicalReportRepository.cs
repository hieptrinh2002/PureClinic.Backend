using Microsoft.EntityFrameworkCore;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Infrastructure.Persistence.Data;

namespace PureLifeClinic.Infrastructure.Persistence.Repositories
{
    public class MedicalReportRepository : BaseRepository<MedicalReport>, IMedicalReportRepository
    {
        public MedicalReportRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<MedicalReport>> GetMedicalReportByPatientId(int patientId, CancellationToken cancellationToken)
        {
            return await _dbContext.MedicalReports
                .Include(m => m.Appointment)
                .Where(m => m.Appointment != null && m.Appointment.PatientId == patientId)
                .Include(m => m.MedicalFiles)
                .Include(m => m.PrescriptionDetails)
                .ThenInclude(pd => pd.Medicine)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public Task<List<MedicalReport>> GetMedicalReportsByPatientIdAsync(int patientId, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            return _dbContext.MedicalReports
                .Where(m => m.Appointment.PatientId == patientId)
                .Include(m => m.Appointment)
                .Include(m => m.MedicalFiles)
                .Include(m => m.PrescriptionDetails)
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }
    }
}
