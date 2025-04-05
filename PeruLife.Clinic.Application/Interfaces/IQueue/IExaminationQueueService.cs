using PureLifeClinic.Core.Entities.General.Queues;

namespace PureLifeClinic.Application.Interfaces.IQueue
{
    public interface IExaminationQueueService
    {
        Task<ExaminationQueue> Create(ExaminationQueue model, CancellationToken cancellationToken);
    }
}
