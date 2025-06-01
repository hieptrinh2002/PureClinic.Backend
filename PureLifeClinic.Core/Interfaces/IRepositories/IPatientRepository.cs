using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Core.Interfaces.IRepositories
{
    public interface IPatientRepository : IBaseRepository<Patient>
    {
        Task<bool> GetByUserId(int userId, CancellationToken cancellationToken);
    }
}
