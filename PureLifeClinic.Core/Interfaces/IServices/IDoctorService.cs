using PureLifeClinic.Core.Entities.Business;

namespace PureLifeClinic.Core.Interfaces.IServices
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorViewModel>> GetAll(CancellationToken cancellationToken);
        Task<PaginatedDataViewModel<DoctorViewModel>> GetPaginatedData(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<DoctorViewModel> GetById(int id, CancellationToken cancellationToken);
        Task<ResponseViewModel> Update(DoctorUpdateViewModel model, CancellationToken cancellationToken);
    }
}
