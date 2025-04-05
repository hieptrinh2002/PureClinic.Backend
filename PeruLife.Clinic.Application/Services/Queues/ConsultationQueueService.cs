using PureLifeClinic.Application.Interfaces.IQueue;
using PureLifeClinic.Core.Entities.General.Queues;
using PureLifeClinic.Core.Interfaces.IRepositories;

namespace PureLifeClinic.Application.Services.Queues
{
    public class ConsultationQueueService : IConsultationQueueService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ConsultationQueueService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ConsultationQueue> Create(ConsultationQueue model, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.ConsultationQueues.Create(model, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}
