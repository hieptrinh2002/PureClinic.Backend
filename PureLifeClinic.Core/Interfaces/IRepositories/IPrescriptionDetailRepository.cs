using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Core.Interfaces.IRepositories
{
    public interface IPrescriptionDetailRepository : IBaseRepository<PrescriptionDetail>
    {
        Task<List<PrescriptionDetail>> GetByMedicalReportId(int medicalReportId, CancellationToken cancellationToken);

    }
}
