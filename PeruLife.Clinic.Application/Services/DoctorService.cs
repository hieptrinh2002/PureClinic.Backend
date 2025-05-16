using AutoMapper;
using PureLifeClinic.Application.BusinessObjects.AppointmentViewModels.Response;
using PureLifeClinic.Application.BusinessObjects.DoctorViewModels.Request;
using PureLifeClinic.Application.BusinessObjects.DoctorViewModels.Response;
using PureLifeClinic.Application.BusinessObjects.PatientsViewModels;
using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Application.BusinessObjects.Schedule.WorkDays;
using PureLifeClinic.Application.Extentions.Mapping;
using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Enums;
using PureLifeClinic.Core.Exceptions;
using PureLifeClinic.Core.Interfaces.IRepositories;
using System.Linq.Expressions;

namespace PureLifeClinic.Application.Services
{
    public class DoctorService : BaseService<Doctor, DoctorViewModel>, IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService; 

        public DoctorService(IMapper mapper, IUnitOfWork unitOfWork, ICloudinaryService cloudinaryService) : base(mapper, unitOfWork.Doctors)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<bool> CheckAvailableTimeSlots(int doctorId, DateTime appointmentDate, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Doctors.IsDoctorAvailableForAppointment(doctorId, appointmentDate, cancellationToken);
        }

        public async Task<IEnumerable<DoctorViewModel>> GetAll(CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.Users.GetAllDoctor(cancellationToken);
            var doctorViewModels = entities.ToList().MapToListDoctorViewModel();
            return doctorViewModels;
        }

        public async Task<IEnumerable<PatientViewModel>> GetAllPatient(int doctorId, CancellationToken cancellationToken)
        {
            Doctor doctor = await _unitOfWork.Doctors.GetById(doctorId, cancellationToken);
            if (doctor == null)
                throw new NotFoundException($"Doctor id - {doctorId} not found");

            var patient = await _unitOfWork.Doctors.GetAllPatient(doctorId, cancellationToken);
            return _mapper.Map<IEnumerable<PatientViewModel>>(patient);
        }

        public async Task<DoctorViewModel> GetById(int id, CancellationToken cancellationToken)
        {
            var doctor = await _unitOfWork.Users.GetDoctorById(id, cancellationToken);
            return doctor.MapToDoctorViewModel();
        }

        public async Task<IEnumerable<AppointmentSlotViewModel>> GetDoctorAvailableTimeSlots(int doctorId, DateTime weekStartDate, CancellationToken cancellationToken)
        {
            var workDayTimespans = _mapper.Map<List<TimespanWorkDayViewModel>>(
                await _unitOfWork.Doctors.GetDoctorWorkDaysTimespanOfWeek(doctorId, weekStartDate, cancellationToken));

            if (!workDayTimespans.Any())
                throw new NotFoundException($"Doctor id - {doctorId} don't have any workday timespan in thí week");

            var appointments = await _unitOfWork.Doctors.GetAllAppointmentOfWeek(doctorId, weekStartDate, cancellationToken);
            List<AppointmentSlotViewModel> availableSlots = new();

            foreach (var workDay in workDayTimespans)
            {
                TimeSpan currentStart = workDay.StartTime;
                TimeSpan workEnd = workDay.EndTime;

                // number of appointment slot base on working time.
                int maxAppointments = await _unitOfWork.Doctors.GetMaxAppointmentsPerDay(doctorId, workDay.WeekDate);
                int appointmentCount = 0;
                var dayAppointments = appointments
                    .Where(a => a.AppointmentDate.Date == workDay.WeekDate.Date)
                    .OrderBy(a => a.AppointmentDate.TimeOfDay)
                    .ToList();

                foreach (var appt in dayAppointments)
                {
                    if (appointmentCount >= maxAppointments) break;

                    if (currentStart < appt.StartTime)
                    {
                        availableSlots.Add(new AppointmentSlotViewModel
                        {
                            WeekDate = workDay.WeekDate,
                            StartTime = currentStart,
                            EndTime = appt.StartTime,
                        });
                    }

                    currentStart = appt.EndTime;
                }

                if (currentStart < workEnd)
                {
                    availableSlots.Add(new AppointmentSlotViewModel
                    {
                        WeekDate = workDay.WeekDate,
                        StartTime = currentStart,
                        EndTime = workEnd,
                    });
                }
            }

            return availableSlots;
        }

        public async Task<PaginatedData<DoctorViewModel>> GetPaginatedData(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var includeList = new List<Expression<Func<User, object>>> { x => x.Role, x => x.Doctor };

            var filters = new List<ExpressionFilter>
            {
                new ExpressionFilter
                {
                    PropertyName = "Doctor",
                    Value = null,
                    Comparison = Comparison.NotEqual
                }
            };
            var paginatedData = await _unitOfWork.Users.GetPaginatedData(includeList, pageNumber, pageSize, cancellationToken, filters);

            var paginatedDataViewModel = new PaginatedData<DoctorViewModel>(paginatedData.Data.ToList().MapToListDoctorViewModel(), paginatedData.TotalCount);
            return paginatedDataViewModel;
        }

        public async Task<PaginatedData<PatientViewModel>> GetPagtinatedPatientData(
            int doctorId, int pageNumber, int pageSize, List<ExpressionFilter>? filters, string sortBy, string sortOrder, CancellationToken cancellationToken)
        {
            var doctor = await _unitOfWork.Doctors.GetById(doctorId, cancellationToken);
            if (doctor == null)
                throw new NotFoundException($"Doctor id - {doctorId} not found");

            var paginatedData = await _unitOfWork.Doctors.GetPaginatedPaitentData(doctorId, pageNumber, pageSize, filters, sortBy, sortOrder, cancellationToken);
            var patientUsers = paginatedData.Data.Select(p => p.User);
            var mappedData = _mapper.Map<IEnumerable<PatientViewModel>>(patientUsers);
            return new PaginatedData<PatientViewModel>(mappedData, paginatedData.TotalCount);
        }

        public async Task<User> GetUserAsync(int doctorId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Doctors.GetUserByDoctorId(doctorId, cancellationToken);
        }

        public Task<ResponseViewModel> Update(DoctorUpdateViewModel model, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<DoctorViewModel> UpdateProfile(int doctorId, DoctorUpdateProfileRequestVM doctorUpdateViewModel, CancellationToken cancellationToken)
        {
            var doctor = await _unitOfWork.Doctors.GetById(doctorId, cancellationToken)
                ?? throw new NotFoundException($"Doctor id - {doctorId} not found");

            var user = await _unitOfWork.Users.GetById(doctor.UserId, cancellationToken)
                ?? throw new NotFoundException($"User id - {doctor.UserId} not found");

            user.FullName = doctorUpdateViewModel.FullName;
            user.Email = doctorUpdateViewModel.Email;
            doctor.Specialty = doctorUpdateViewModel.Specialty;
            doctor.Qualification = doctorUpdateViewModel.Qualification;
            doctor.ExperienceYears = doctorUpdateViewModel.ExperienceYears;
            doctor.Description = doctorUpdateViewModel.Description;
            doctor.RegistrationNumber = doctorUpdateViewModel.RegistrationNumber;

            if (doctorUpdateViewModel.Avatar != null && doctorUpdateViewModel.Avatar.Length > 0)
            {
                // delete old image from cloudinary
                if (!string.IsNullOrEmpty(user.ImagePathPublicId))
                {
                    await _cloudinaryService.DeleteFileAsync(user.ImagePathPublicId);
                }

                var uploadResult = await _cloudinaryService.UploadImgAsync(doctorUpdateViewModel.Avatar, $"user_{user.Id}_avatars");
                user.ImagePath = uploadResult.Url;
                user.ImagePathPublicId = uploadResult.PublicId;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);    

            return user.MapToDoctorViewModel();
        }
    }
}
