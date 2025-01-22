using PureLifeClinic.Core.Entities.Business;

namespace PureLifeClinic.Core.Interfaces.IServices
{
    public interface IMedicineService : IBaseService<MedicineViewModel>
    {
        Task<MedicineViewModel> Create(MedicineCreateViewModel model, CancellationToken cancellationToken);
        Task Update(MedicineUpdateViewModel model, CancellationToken cancellationToken);
        Task Delete(int id, CancellationToken cancellationToken);
        Task<double> PriceCheck(int medicineId, CancellationToken cancellationToken);
    }
}
