using PureLifeClinic.Core.Entities.General.Queues;

namespace PureLifeClinic.Application.Interfaces.IQueue
{
    public interface IConsultationQueueService
    {
        Task<ConsultationQueue> Create(ConsultationQueue model, CancellationToken cancellationToken);
    }
}
