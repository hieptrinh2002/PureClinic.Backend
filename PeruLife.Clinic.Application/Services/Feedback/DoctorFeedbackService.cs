using AutoMapper;
using Microsoft.Extensions.Logging;
using PureLifeClinic.Application.BusinessObjects.Feedbacks.Doctor.Request;
using PureLifeClinic.Application.BusinessObjects.Feedbacks.Doctor.Response;
using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Application.Interfaces.IServices.FeedBack;
using PureLifeClinic.Core.Entities.General.Feedback;
using PureLifeClinic.Core.Exceptions;
using PureLifeClinic.Core.Interfaces.IRepositories;

namespace PureLifeClinic.Application.Services.FeedBack
{
    public class DoctorFeedbackService : BaseService<DoctorFeedback, DoctorFeedbackViewModel>, IDoctorFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;
        private readonly ILogger<DoctorFeedbackService> _logger;
        public DoctorFeedbackService(
            IMapper mapper,
            ILogger<DoctorFeedbackService> logger,
            IUserContext userContext,
            IUnitOfWork unitOfWork) : base(mapper, unitOfWork.DoctorFeedbacks)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContext = userContext;
            _logger = logger;
        }

        public async Task<Feedback> CreateFeedbackAsync(FeedbackDoctorCreateViewModel feedback, CancellationToken cancellationToken)
        {
            var model = _mapper.Map<DoctorFeedback>(feedback);
            model.EntryBy = Convert.ToInt32(_userContext.UserId);
            model.EntryDate = DateTime.Now;
            var entity = await _unitOfWork.DoctorFeedbacks.Create(model, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<DoctorFeedbackSummaryViewModel> GetDoctorFeedbackSummaryAsync(int doctorId, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Doctors.GetById(doctorId, cancellationToken) == null)
                throw new NotFoundException($"Doctor with ID {doctorId} not found.");

            var averageRating = await _unitOfWork.DoctorFeedbacks.GetAverageRatingByDoctorAsync(doctorId);
            var feedbackCount = await _unitOfWork.DoctorFeedbacks.GetFeedbackCountByDoctorAsync(doctorId);

            return new DoctorFeedbackSummaryViewModel
            {
                DoctorId = doctorId,
                AverageRating = averageRating,
                TotalFeedbacks = feedbackCount
            };
        }

        public async Task<IEnumerable<DoctorFeedbackViewModel>> GetDoctorFeedbacksByDateRangeAsync(int doctorId, DateTime startDate, DateTime endDate)
        {
            if (await _unitOfWork.Doctors.GetById(doctorId, default) == null)
                throw new NotFoundException($"Doctor with ID {doctorId} not found.");

            if (startDate > endDate)
                throw new ArgumentException("Start date cannot be greater than end date.");

            var feedbacks = await _unitOfWork.DoctorFeedbacks.GetByDateRangeAsync(doctorId, startDate, endDate);
            return _mapper.Map<IEnumerable<DoctorFeedbackViewModel>>(feedbacks);
        }

        public async Task<bool> ReportFeedbackAsync(int feedbackId)
        {
            var feedback = await _unitOfWork.DoctorFeedbacks.GetById(feedbackId, cancellationToken: default);
            if (feedback == null)
                return false;

            feedback.IsReported = true;
            await _unitOfWork.DoctorFeedbacks.Update(feedback, default);
            await _unitOfWork.SaveChangesAsync(default);
            return true;
        }

        public async Task<IEnumerable<DoctorFeedbackViewModel>> GetFeedbacksByPatientAsync(int patientId, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Patients.GetById(patientId, cancellationToken) == null)
                throw new NotFoundException($"Patient with ID {patientId} not found.");

            var feedbacks = await _unitOfWork.DoctorFeedbacks.GetByPatientIdAsync(patientId, cancellationToken);
            if (feedbacks == null || !feedbacks.Any())
            {
                _logger.LogWarning($"No feedbacks found for patient with ID {patientId}.");
            }

            return _mapper.Map<IEnumerable<DoctorFeedbackViewModel>>(feedbacks);
        }

        public async Task<IEnumerable<DoctorFeedbackViewModel>> GetFeedbacksByDoctorAsync(int doctorId, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Doctors.GetById(doctorId, cancellationToken) == null)
                throw new NotFoundException($"Doctor with ID {doctorId} not found.");

            var feedbacks = await _unitOfWork.DoctorFeedbacks.GetByDoctorIdAsync(doctorId, cancellationToken);
            if (feedbacks == null || !feedbacks.Any())
            {
                _logger.LogWarning($"No feedbacks found for doctor with ID {doctorId}.");
            }

            return _mapper.Map<IEnumerable<DoctorFeedbackViewModel>>(feedbacks);
        }
    }
}
