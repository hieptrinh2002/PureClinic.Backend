using PureLifeClinic.Application.BusinessObjects.PatientsViewModels;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Application.Interfaces.IServices
{
    public interface IPatientService
    {
        Task<Patient> CreateAsync(PatientCreateViewModel model, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
        Task<PatientViewModel> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(int id, PatientUpdateViewModel model, CancellationToken cancellationToken);
    }
}
