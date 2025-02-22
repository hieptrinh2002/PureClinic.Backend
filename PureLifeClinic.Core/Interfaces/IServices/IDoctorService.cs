using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Entities.Business;

namespace PureLifeClinic.Core.Interfaces.IServices
{
    public interface IDoctorService : IBaseService<DoctorViewModel> 
    {
        Task<IEnumerable<DoctorViewModel>> GetAll(CancellationToken cancellationToken);
        Task<PaginatedDataViewModel<DoctorViewModel>> GetPaginatedData(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<DoctorViewModel> GetById(int id, CancellationToken cancellationToken);
        Task<ResponseViewModel> Update(DoctorUpdateViewModel model, CancellationToken cancellationToken);
        Task<IEnumerable<AppointmentSlotViewModel>> GetDoctorAvailableTimeSlots(int doctorId, DateTime weekStartDate, CancellationToken cancellationToken);
        Task<IEnumerable<PatientViewModel>> GetAllPatient(int doctorId, CancellationToken cancellationToken);
        Task<PaginatedDataViewModel<PatientViewModel>> GetPagtinatedPatientData(
           int doctorId, int pageNumber, int pageSize, List<ExpressionFilter>? filters, string sortBy, string sortOrder, CancellationToken cancellationToken);

        //new Task<PaginatedDataViewModel<PatientViewModel>> GetPaginatedPatientData(
        //    int doctorId, int pageNumber, int pageSize, string? sortBy, string? sortOrder, string? searchKey, CancellationToken cancellationToken);
    }
}
