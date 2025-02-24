using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Core.Interfaces.IRepositories
{
    public interface IMedicalReportRepository : IBaseRepository<MedicalReport>
    {
        Task<IEnumerable<MedicalReport>> GetMedicalReportByPatientId(int patientId, CancellationToken cancellationToken);
    }
}
