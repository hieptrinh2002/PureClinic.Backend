using Microsoft.EntityFrameworkCore;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Infrastructure.Data;

namespace PureLifeClinic.Infrastructure.Repositories
{
    public class MedicineRepository: BaseRepository<Medicine>, IMedicineRepository
    {
        public MedicineRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<double> PriceCheck(int medicineId, CancellationToken cancellationToken = default)
        {
            var price = await _dbContext.Medicines
                .Where(x => x.Id == medicineId)
                .Select(x => x.Price)
                .FirstOrDefaultAsync(cancellationToken);
            return price;
        }
    }
}
