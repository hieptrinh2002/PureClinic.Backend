namespace PureLifeClinic.Application.Interfaces.IQueue
{
    public interface IRedisQueueService
    {
        Task AddToConsultationQueueAsync(string queueNumber);
        Task<string?> GetNextConsultationAsync();
        Task AddToExaminationQueueAsync(string queueNumber, int doctorId, int priority);
        Task<string?> GetNextExaminationAsync(int doctorId);
        Task<List<string>> GetExaminationQueueSnapshot(int doctorId);
        Task<List<string>> GetConsultationQueueSnapshot(int pageNumber, int pageSize);
    }
}
