using PureLifeClinic.Core.Entities.Business;

namespace PureLifeClinic.Core.Interfaces.IServices
{
    public interface IMedicalReportService : IBaseService<MedicalReportViewModel>
    {
        Task<MedicalReportViewModel> Create(MedicalReportCreateViewModel model, CancellationToken cancellationToken);
        Task UpdateMedicalReportAsync(MedicalReportUpdateViewModel model, CancellationToken cancellationToken);
        Task Delete(int id, CancellationToken cancellationToken);
        Task<ResponseViewModel<IEnumerable<MedicalReportViewModel>>> GetByPatientId(int patientId, CancellationToken cancellationToken);
        new Task<ResponseViewModel<MedicalReportViewModel>> GetById(int id, CancellationToken cancellationToken);
    }
}
