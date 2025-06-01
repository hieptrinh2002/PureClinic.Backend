using PureLifeClinic.Application.BusinessObjects.LabResultViewModels;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Application.Interfaces.IServices
{
    public interface ILabResultService
    {
        Task<IEnumerable<LabResult>> FilterAsync(int patientId, string? testType, LabTestStatus? status, DateTime? from, DateTime? to);
        Task<LabResult?> GetByIdAsync(int patientId, int id);
        Task<bool> UpdateAsync(int patientId, int id, LabResultUpdateViewModel dto);
    }
}
