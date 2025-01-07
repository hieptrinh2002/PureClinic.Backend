using PureLifeClinic.Core.Entities.Business;

namespace PureLifeClinic.Core.Interfaces.IServices
{
    public interface IDoctorService
    {
        new Task<IEnumerable<DoctorViewModel>> GetAll(CancellationToken cancellationToken);
        new Task<PaginatedDataViewModel<DoctorViewModel>> GetPaginatedData(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<DoctorViewModel> GetById(int id, CancellationToken cancellationToken);
        Task<ResponseViewModel> Update(DoctorUpdateViewModel model, CancellationToken cancellationToken);
    }
}
