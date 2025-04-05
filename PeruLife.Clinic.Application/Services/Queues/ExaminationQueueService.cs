using PureLifeClinic.Application.Interfaces.IQueue;
using PureLifeClinic.Core.Entities.General.Queues;
using PureLifeClinic.Core.Interfaces.IRepositories;

namespace PureLifeClinic.Application.Services.Queues
{
    public class ExaminationQueueService: IExaminationQueueService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExaminationQueueService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ExaminationQueue> Create(ExaminationQueue model, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.ExaminationQueues.Create(model, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}
