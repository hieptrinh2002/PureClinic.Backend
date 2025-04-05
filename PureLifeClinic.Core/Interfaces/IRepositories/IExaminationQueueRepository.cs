using PureLifeClinic.Core.Entities.General.Queues;

namespace PureLifeClinic.Core.Interfaces.IRepositories
{
    public interface IExaminationQueueRepository : IBaseRepository<ExaminationQueue>
    {
        Task<List<ExaminationQueue>> GetAllToday(int doctorId, CancellationToken cancellation);
        Task<ExaminationQueue> GetFirstWithQueueNum(string queueNumber, CancellationToken cancellation);
    }
}
