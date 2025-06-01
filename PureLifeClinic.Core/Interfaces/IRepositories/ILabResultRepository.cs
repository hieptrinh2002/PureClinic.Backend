using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Core.Interfaces.IRepositories
{
    public interface ILabResultRepository : IBaseRepository<LabResult>
    {
        Task<IQueryable<LabResult>> QueryAsync(int patientId);
        Task<LabResult?> GetByIdAsync(int patientId, int id);
        void Update(LabResult entity);
    }
}
