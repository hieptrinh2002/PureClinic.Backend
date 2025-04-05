
using PureLifeClinic.Core.Enums;
using PureLifeClinic.Core.Enums.Queues;

namespace PureLifeClinic.Application.Interfaces.IQueue
{
    public interface IWaitingQueueService
    {
        Task<string> CheckInPatient(CancellationToken cancellationToken);
        Task<string?> CallNextConsultationQueueNumber(int counterId, CancellationToken cancellationToken);
        Task UpdateConsultationQueueStatus(string queueNumber, QueueStatus status, CancellationToken cancellationToken);
        Task<List<string>> GetCurrentConsultationQueue(int pageNumber, int pageSize);
        Task<string> AddToExaminationQueue(int patientId, int doctorId, ConsultationType type, CancellationToken cancellationToken);
        Task<List<string>> GetDoctorExaminationQueue(int doctorId);
        Task<string?> CallNextExaminationQueueNumber(int doctorId, CancellationToken cancellationToken);
        Task UpdateExaminationQueueStatus(string queueNumber, QueueStatus status, CancellationToken cancellationToken);
    }
}