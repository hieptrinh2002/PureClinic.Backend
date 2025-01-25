using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureLifeClinic.Core.Interfaces.IRepositories
{
    public interface IMedicalReportRepository : IBaseRepository<MedicalReport>
    {
        Task<IEnumerable<MedicalReport>> GetMedicalReportByPatientId(int patientId, CancellationToken cancellationToken);
    }
}
