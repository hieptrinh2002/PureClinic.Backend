using PureLifeClinic.Core.Entities.General.Queues;

namespace PureLifeClinic.Core.Interfaces.IRepositories
{
    public interface IConsultationQueueRepository : IBaseRepository<ConsultationQueue>
    {
        Task<List<ConsultationQueue>> GetAllToday(CancellationToken cancellation);
        Task<ConsultationQueue> GetFirstWithQueueNum(string queueNumber);
    }
}