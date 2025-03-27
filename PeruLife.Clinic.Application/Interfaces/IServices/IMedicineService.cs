using PureLifeClinic.Application.BusinessObjects.MedicineViewModels;

namespace PureLifeClinic.Application.Interfaces.IServices
{
    public interface IMedicineService : IBaseService<MedicineViewModel>
    {
        Task<MedicineViewModel> Create(MedicineCreateViewModel model, CancellationToken cancellationToken);
        Task Update(MedicineUpdateViewModel model, CancellationToken cancellationToken);
        Task Delete(int id, CancellationToken cancellationToken);
        Task<double> PriceCheck(int medicineId, CancellationToken cancellationToken);
    }
}
