using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Core.Interfaces.IRepositories
{
    public interface IMedicineRepository : IBaseRepository<Medicine>
    {
        Task<double> PriceCheck(int medicineId, CancellationToken cancellationToken);
    }
}
