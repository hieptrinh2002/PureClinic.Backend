﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PureLifeClinic.Application.BusinessObjects.AppointmentHealthServices;
using PureLifeClinic.Application.BusinessObjects.AppointmentViewModels.Request;
using PureLifeClinic.Application.BusinessObjects.AppointmentViewModels.Response;
using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Application.Extentions.Mapping;
using PureLifeClinic.Application.Interfaces.IBackgroundJobs;
using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Application.Interfaces.IServices.INotification;
using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Common.Constants;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Enums;
using PureLifeClinic.Core.Exceptions;
using PureLifeClinic.Core.Interfaces.IRepositories;

namespace PureLifeClinic.Application.Services
{
    public class AppointmentService : BaseService<Appointment, AppointmentViewModel>, IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;
        private readonly ICacheServiceFactory _cacheServiceFactory;
        private readonly IDoctorService _doctorService;
        private readonly IBackgroundJobService _backgroundJobService;
        public AppointmentService(
            IMapper mapper,
            IUserContext userContext,
            ICacheServiceFactory cacheServiceFactory,
            IDoctorService doctorService,
            IBackgroundJobService backgroundJobService,
            IUnitOfWork unitOfWork)
            : base(mapper, unitOfWork.Appointments)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userContext = userContext;
            _cacheServiceFactory = cacheServiceFactory;
            _doctorService = doctorService;
            _backgroundJobService = backgroundJobService;
        }

        public new async Task<IEnumerable<AppointmentViewModel>> GetAll(CancellationToken cancellationToken)
        {
            var includeExpressions = new List<Func<IQueryable<Appointment>, IIncludableQueryable<Appointment, object>>>
            {
                query => query.Include(a => a.Doctor).ThenInclude(d => d.User),
                query => query.Include(a => a.Patient).ThenInclude(p => p.User)
            };
            var entities = (await _unitOfWork.Appointments.GetAll(includeExpressions, null, cancellationToken))
                .OrderBy(e => e.Status)
                .ThenBy(e => e.AppointmentDate)
                .ToList();

            return _mapper.Map<IEnumerable<AppointmentViewModel>>(entities);
        }

        public async Task<AppointmentViewModel> Create(AppointmentCreateViewModel model, CancellationToken cancellationToken)
        {
            bool checkValidTime = await _doctorService.CheckAvailableTimeSlots(model.DoctorId, model.AppointmentDate, cancellationToken);
            if (checkValidTime == false)
            {
                throw new ErrorException("Doctor is not available at this time");
            }

            var entity = _mapper.Map<Appointment>(model);
            entity.EntryDate = DateTime.Now;
            entity.EntryBy = Convert.ToInt32(_userContext.UserId);

            var result = await _unitOfWork.Appointments.Create(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // wrap the notification in a background job
            _backgroundJobService.ScheduleImmediateJob<INotificationService>(
                x => x.SendToGroupAsync(GroupConstants.Employee, EventConstants.OnNewAppointmentReceived, "We have a new appointment !")
            );

            var doctorUser = await _doctorService.GetUserAsync(model.DoctorId, cancellationToken);
            _backgroundJobService.ScheduleImmediateJob<INotificationService>(
                x => x.SendToUserAsync(doctorUser.Id.ToString(), EventConstants.OnNewAppointmentReceived, "You have a new appointment !")
            );

            return _mapper.Map<AppointmentViewModel>(result);
        }

        public async Task<AppointmentViewModel> CreateInPersonAppointment(InPersonAppointmentCreateViewModel model, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken: cancellationToken);
            try
            {
                if (model.Email != null && await _unitOfWork.Users.GetByEmail(model.Email, cancellationToken) != null)
                    throw new ErrorException("Email was used");

                // create patient 
                var patient = new Patient
                {
                    EntryDate = DateTime.Now,
                    EntryBy = Convert.ToInt32(_userContext.UserId),
                    User = new User
                    {
                        IsActive = true,
                        FullName = model.FullName,
                        PhoneNumber = model.PhoneNumber,
                        Gender = model.Gender,
                        DateOfBirth = model.DateOfBirth,
                        Role = _unitOfWork.Roles.GetPatientRole(),
                        Email = model.Email,
                    },
                };
                patient.Appointments.Add(new Appointment
                {
                    AppointmentDate = model.AppointmentDate,
                    Reason = model.Reason,
                    PatientId = patient.Id,
                    DoctorId = model.DoctorId,
                    ReferredPerson = model.ReferredPerson,
                    EntryDate = DateTime.Now,
                    EntryBy = Convert.ToInt32(_userContext.UserId),
                });

                await _unitOfWork.Patients.Create(patient, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // Commit transaction
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                return _mapper.Map<AppointmentViewModel>(_mapper.Map<AppointmentViewModel>(patient.Appointments.Last()));
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }

        public async Task<bool> IsExists(int doctorId, DateTime date, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Appointments.IsExistsAppointment(doctorId, date, cancellationToken);
        }

        public async Task<ResponseViewModel<AppointmentViewModel>> UpdateAppointmentAsync(int id, AppointmentUpdateViewModel model, CancellationToken cancellationToken)
        {
            var existingData = await _unitOfWork.Appointments.GetById(id, cancellationToken)
                ?? throw new KeyNotFoundException($"Appointment with ID {id} not found.");

            _mapper.Map(model, existingData);
            existingData.UpdatedDate = DateTime.Now;
            existingData.UpdatedBy = Convert.ToInt32(_userContext.UserId);
            await _unitOfWork.Appointments.Update(existingData, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ResponseViewModel<AppointmentViewModel>
            {
                Success = true,
                Data = existingData.MapToAppointmentViewModel(),
                Message = "Update apointment sucessfully !"
            };
        }

        public async Task<ResponseViewModel> UpdateAppointmentStatusAsync(int id, AppointmentStatus status, CancellationToken cancellationToken)
        {
            var appointment = await _unitOfWork.Appointments.GetById(id, cancellationToken)
                ?? throw new KeyNotFoundException($"Appointment with ID {id} not found.");
            appointment.Status = status;
            await _unitOfWork.Appointments.Update(appointment, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ResponseViewModel
            {
                Success = true,
                Message = "Appointment status updated successfully"
            };
        }

        public async Task<ResponseViewModel<IEnumerable<DoctorAppointmentViewModel>>> GetAllAppointmentsByDoctorIdAsync(int doctorId, CancellationToken cancellationToken)
        {
            var patient = await _unitOfWork.Doctors.GetById(doctorId, cancellationToken) ?? throw new ErrorException("Doctor not found");

            var filters = new List<ExpressionFilter>
            {
                new ()
                {
                    PropertyName = "DoctorId",
                    Value = doctorId,
                    Comparison = Comparison.Equal,
                }
            };
            var includeExpressions = new List<Func<IQueryable<Appointment>, IIncludableQueryable<Appointment, object>>>
            {
                query => query.Include(a => a.Doctor),
                query => query.Include(a => a.Patient).ThenInclude(p => p.User)
            };

            var entities = (await _unitOfWork.Appointments.GetAll(includeExpressions, filters, cancellationToken))
                      .OrderBy(e => e.Status).ThenBy(e => e.AppointmentDate).ToList();

            return new ResponseViewModel<IEnumerable<DoctorAppointmentViewModel>>
            {
                Success = true,
                Data = _mapper.Map<List<DoctorAppointmentViewModel>>(entities),
            };
        }

        public async Task<ResponseViewModel<IEnumerable<PatientAppointmentViewModel>>> GetAllAppointmentsByPatientIdAsync(int patientId, CancellationToken cancellationToken)
        {
            var cacheKey = $"patient-{patientId}";
            var _cacheService = _cacheServiceFactory.GetCacheService(CacheType.Redis);
            var cachedData = await _cacheService.GetAsync<ResponseViewModel<IEnumerable<PatientAppointmentViewModel>>>(cacheKey);
            if (cachedData != null)
            {
                return cachedData;
            }

            var patient = await _unitOfWork.Patients.GetById(patientId, cancellationToken) ?? throw new ErrorException("Patient not found");

            var filters = new List<ExpressionFilter>
            {
                new()
                {
                    PropertyName = "PatientId",
                    Value = patientId,
                    Comparison = Comparison.Equal,
                }
            };
            var includeExpressions = new List<Func<IQueryable<Appointment>, IIncludableQueryable<Appointment, object>>>
            {
                query => query.Include(a => a.Doctor).ThenInclude(p => p.User),
                query => query.Include(a => a.Patient)
            };

            var entities = (await _unitOfWork.Appointments.GetAll(includeExpressions, filters, cancellationToken))
                      .OrderBy(e => e.Status).ThenBy(e => e.AppointmentDate).ToList();

            var response = new ResponseViewModel<IEnumerable<PatientAppointmentViewModel>>
            {
                Success = true,
                Data = _mapper.Map<List<PatientAppointmentViewModel>>(entities),
            };

            await _cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(30));

            return response;
        }

        public async Task<ResponseViewModel<IEnumerable<AppointmentViewModel>>> GetAllFilterAppointments(
            FilterAppointmentRequestViewModel model, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Appointments.GetAllFilterAppointments(
                model.StartTime,
                model.DoctorId, model.PatientId,
                model.EndTime,
                model.Top,
                model.Status,
                model.SortBy,
                model.SortOrder,
                cancellationToken
            );

            return new ResponseViewModel<IEnumerable<AppointmentViewModel>>
            {
                Success = true,
                Data = _mapper.Map<List<AppointmentViewModel>>(result),
            };
        }

        public async Task<List<AppointmentHealthServiceVM>> AssignServiceToAppointment(
            int appointmentId, 
            List<AppointmentHealthServiceCreateVM> services, 
            CancellationToken cancellationToken)
        {
            var appointment = await _unitOfWork.Appointments.GetById(appointmentId, cancellationToken)
                ?? throw new NotFoundException("Appointment not found");

            // map to appointment health service
            var appointmentHealthServices = services.Select(x => x.MapToAppointmentHealthService()).ToList();

            appointmentHealthServices.ForEach(x =>
            {
                x.EntryBy = Convert.ToInt32(_userContext.UserId);
            });

            await _unitOfWork.AppointmentHealthServices.CreateRange(appointmentHealthServices, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var healthServiceList = await _unitOfWork.AppointmentHealthServices.GetAllByAppointmentId(appointmentId, cancellationToken);

            return healthServiceList.MapToAppointmentHealthServiceViewModelList();
        }

        public async Task<List<AppointmentHealthServiceVM>> GetAppointmentHealthService(
            int appointmentId,
            int serviceId, 
            CancellationToken cancellationToken)
        {
            var appointment = await _unitOfWork.Appointments.GetById(appointmentId, cancellationToken)      
                ?? throw new NotFoundException("Appointment not found");

            var healthServiceList = await _unitOfWork.AppointmentHealthServices.GetAllByAppointmentId(appointmentId, cancellationToken);

            return healthServiceList.MapToAppointmentHealthServiceViewModelList();
        }

        public async Task DeleteAppointmentHealthService(int appointmentId, List<int> serviceIds, CancellationToken cancellationToken)
        {
            var appointment = await _unitOfWork.Appointments.GetById(appointmentId, cancellationToken)
              ?? throw new NotFoundException("Appointment not found");

            var appointmentHealthServices = await _unitOfWork.AppointmentHealthServices.GetAllByAppointmentId(appointmentId, cancellationToken);
            await _unitOfWork.AppointmentHealthServices.DeleteRange(appointmentHealthServices, cancellationToken);
        }

        public async Task UpdateAppointmentHealthService(int appointmentId, AppointmentHealthServiceStatus status, List<int> serviceIds, CancellationToken cancellationToken)
        {
            var appointment = await _unitOfWork.Appointments.GetById(appointmentId, cancellationToken)
             ?? throw new NotFoundException("Appointment not found");

            var appointmentHealthServices = await _unitOfWork.AppointmentHealthServices.GetAllByAppointmentId(appointmentId, cancellationToken);
            appointmentHealthServices.ForEach(x =>
            {
                x.Status = status;
                x.UpdatedBy = Convert.ToInt32(_userContext.UserId);
            });
            await _unitOfWork.SaveChangesAsync(cancellationToken);  
        }
    }
}
