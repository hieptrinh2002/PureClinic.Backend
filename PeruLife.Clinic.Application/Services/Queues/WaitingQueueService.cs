using PureLifeClinic.Application.Interfaces;
using PureLifeClinic.Application.Interfaces.IBackgroundJobs;
using PureLifeClinic.Application.Interfaces.IQueue;
using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Application.Interfaces.IServices.INotification;
using PureLifeClinic.Core.Entities.General.Queues;
using PureLifeClinic.Core.Enums;
using PureLifeClinic.Core.Enums.Queues;
using PureLifeClinic.Core.Exceptions;
using PureLifeClinic.Core.Interfaces.IRepositories;

namespace PureLifeClinic.Application.Services.Queues
{
    public class WaitingQueueService : IWaitingQueueService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisQueueService _redisQueueService;
        private readonly IUserContext _userContext;
        private readonly IBackgroundJobService _backgroundJobService;
        private readonly INotificationService _notificationService;
        public WaitingQueueService(
            IUnitOfWork unitOfWork,
            IRedisQueueService redisQueueService,
            IUserContext userContext,
            IBackgroundJobService backgroundJobService,
            INotificationService notificationService)
        {
            _userContext = userContext;
            _unitOfWork = unitOfWork;
            _redisQueueService = redisQueueService;
            _backgroundJobService = backgroundJobService;
            _notificationService = notificationService; 
        }

        public async Task<string> CheckInPatient(CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            // Generate queue number (A20231015-001)
            var queueNumber = await GenerateSequenceNumberOfConsultationQueue(cancellationToken);

            // Save to Redis (real-time)
            await _redisQueueService.AddToConsultationQueueAsync(queueNumber);

            // Save to SQL (history)
            var queueEntry = new ConsultationQueue
            {
                EntryDate = DateTime.UtcNow,
                QueueNumber = queueNumber,
                Status = QueueStatus.Waiting
            };

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            _backgroundJobService.ScheduleImmediateJob<IConsultationQueueService>(x => x.Create(queueEntry, cancellationToken));

            return queueNumber;
        }

        // Call the next consultation number
        public async Task<string?> CallNextConsultationQueueNumber(int counterId, CancellationToken cancellationToken)
        {
            var counter = await _unitOfWork.Counters.GetById(counterId, cancellationToken)
                ?? throw new NotFoundException($"Counter - {counterId} not found");

            if (counter.CounterType != CounterType.Ticketing)
                throw new ErrorException($"Counter don't have perrmisson to call next consultation queue number");

            var queueNumber = await _redisQueueService.GetNextConsultationAsync();
            if (string.IsNullOrEmpty(queueNumber))
                return null;

            var entry = await _unitOfWork.ConsultationQueues.GetFirstWithQueueNum(queueNumber)
                ?? throw new NotFoundException($"Consultation queue entry '{queueNumber}' not found.");

            entry.CounterId = counterId;
            entry.Status = QueueStatus.InProgress;
            counter.CurrentQueueNumber = queueNumber;
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return queueNumber;
        }

        // Update status when call the patient in waiting queue
        public async Task UpdateConsultationQueueStatus(string queueNumber, QueueStatus status, CancellationToken cancellationToken)
        {
            var consultationQueue = await _unitOfWork.ConsultationQueues.GetFirstWithQueueNum(queueNumber)
                ?? throw new NotFoundException($"Patient with consulation number -{queueNumber} is not found");
            consultationQueue.Status = status;
            consultationQueue.UpdatedDate = DateTime.UtcNow;
            consultationQueue.EntryBy = Convert.ToInt32(_userContext.UserId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // Get current consultation queue
        public async Task<List<string>> GetCurrentConsultationQueue(int pageNumber, int pageSize)
        {
            return await _redisQueueService.GetConsultationQueueSnapshot(pageNumber, pageSize);
        }

        public async Task<string> GenerateSequenceNumberOfConsultationQueue(CancellationToken cancellation)
        {
            return $"A{DateTime.Today:yyyyMMdd}-{(await _unitOfWork.ConsultationQueues.GetAllToday(cancellation)).Count + 1:D7}";
        }

        public async Task<string> GenerateSequenceNumberOfExaminationQueue(int doctorId, CancellationToken cancellation)
        {
            return $"B{DateTime.Today:yyyyMMdd}-{(await _unitOfWork.ExaminationQueues.GetAllToday(doctorId, cancellation)).Count + 1:D7}";
        }

        // ExaminationQueue
        public async Task UpdateExaminationQueueStatus(string queueNumber, QueueStatus status, CancellationToken cancellationToken)
        {
            var consultationQueue = await _unitOfWork.ExaminationQueues.GetFirstWithQueueNum(queueNumber, cancellationToken)
                ?? throw new NotFoundException($"Patient with examination number -{queueNumber} is not found");
            consultationQueue.Status = status;
            consultationQueue.UpdatedDate = DateTime.UtcNow;
            consultationQueue.EntryBy = Convert.ToInt32(_userContext.UserId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<string> AddToExaminationQueue(int patientId, int doctorId, ConsultationType type, CancellationToken cancellationToken)
        {
            var patient = await _unitOfWork.Patients.GetById(patientId, cancellationToken)
                ?? throw new ErrorException("Patient not found");
            var doctor = await _unitOfWork.Doctors.GetById(doctorId, cancellationToken)
                ?? throw new ErrorException("Doctor not found");

            var queueNumber = await GenerateSequenceNumberOfExaminationQueue(doctorId, cancellationToken);

            var entry = new ExaminationQueue
            {
                PatientId = patientId,
                DoctorId = doctorId,
                QueueNumber = queueNumber,
                Type = type,
                Priority = type == ConsultationType.Booking ? Priority.High : Priority.Standard,
                Status = QueueStatus.Waiting
            };

            await _redisQueueService.AddToExaminationQueueAsync(queueNumber, doctorId, (int)(entry.Priority));

            _backgroundJobService.ScheduleImmediateJob<IExaminationQueueService>(x => x.Create(entry, cancellationToken));

            //await NotifyQueueUpdate("examination", queueNumber, "Waiting", doctorId);
            return queueNumber;
        }

        public async Task<List<string>> GetDoctorExaminationQueue(int doctorId)
        {
            return await _redisQueueService.GetExaminationQueueSnapshot(doctorId);
        }

        public async Task<string?> CallNextExaminationQueueNumber(int doctorId, CancellationToken cancellationToken)
        {

            var queueNumber = await _redisQueueService.GetNextExaminationAsync(doctorId);
            if (queueNumber == null)
                return null;

            ExaminationQueue entry = await _unitOfWork.ExaminationQueues.GetFirstWithQueueNum(queueNumber, cancellationToken);

            if (entry != null)
            {
                entry.Status = QueueStatus.InProgress;
                entry.UpdatedDate = DateTime.UtcNow;
                entry.UpdatedBy = Convert.ToInt32(_userContext.UserId);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return queueNumber;
        }

        //private async Task<int> AssignDoctorAutomatically()
        //{
        //    // Triển khai logic gán bác sĩ tự động
        //    return await _db.Doctors
        //        .Where(d => d.IsAvailable)
        //        .OrderBy(d => d.ExaminationQueues.Count(e => e.Status == "InProgress"))
        //        .Select(d => d.Id)
        //        .FirstOrDefaultAsync();
        //}

        //public async Task UpdateExaminationStatus(string queueNumber, string status)
        //{
        //    var entry = await _db.ExaminationQueues.FirstOrDefaultAsync(q => q.QueueNumber == queueNumber);

        //    if (entry != null)
        //    {
        //        entry.Status = status;
        //        await _db.SaveChangesAsync();
        //        await NotifyQueueUpdate("examination", queueNumber, status, entry.DoctorId);
        //    }
        //}

        private async Task NotifyQueueUpdate(string queueType, string queueNumber, string status, int? doctorId, DateTime date)
        {
            string groupName = $"{queueType}-{doctorId}-{date:yyyy-MM-dd}";

            await _notificationService.SendToGroupAsync(groupName, "QueueUpdated", new
            {
                QueueType = queueType,
                QueueNumber = queueNumber,
                Status = status,
                DoctorId = doctorId,
                Date = date.ToString("yyyy-MM-dd")
            });
        }
    }
}