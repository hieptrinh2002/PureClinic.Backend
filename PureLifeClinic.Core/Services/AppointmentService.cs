using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Core.Interfaces.IServices;

namespace PureLifeClinic.Core.Services
{
    public class AppointmentService : BaseService<Appointment, AppointmentViewModel>, IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;
        private readonly ICacheServiceFactory _cacheServiceFactory;
        public AppointmentService(IMapper mapper, IUserContext userContext, ICacheServiceFactory cacheServiceFactory, IUnitOfWork unitOfWork)
            : base(mapper, unitOfWork.Appointments)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userContext = userContext;
            _cacheServiceFactory = cacheServiceFactory;
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
            // check doctor working time

            // check the appointment time 

            // wrap everything in one transaction 

            // 
            try
            {
                var entity = _mapper.Map<Appointment>(model);
                entity.EntryDate = DateTime.Now;
                entity.EntryBy = Convert.ToInt32(_userContext.UserId);

                var result = await _unitOfWork.Appointments.Create(entity, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return _mapper.Map<AppointmentViewModel>(result);
            }
            catch (Exception ex)
            {
                var t = ex.Message;
            }
            return null;
        }

        public async Task<AppointmentViewModel> CreateInPersonAppointment(InPersonAppointmentCreateViewModel model, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                if (model.Email != null)
                {
                    if (await _unitOfWork.Users.GetByEmail(model.Email, cancellationToken) != null) throw new Exception("Email was used");
                }

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
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }

        public async Task<bool> IsExists(int doctorId, DateTime date, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Appointments.IsExistsAppointment(doctorId, date, cancellationToken);
        }

        public async Task<ResponseViewModel> UpdateAppointmentAsync(int id, AppointmentUpdateViewModel model, CancellationToken cancellationToken)
        {
            var existingData = await _unitOfWork.Appointments.GetById(id, cancellationToken)
                ?? throw new KeyNotFoundException($"Appointment with ID {id} not found.");

            _mapper.Map(model, existingData);
            existingData.UpdatedDate = DateTime.Now;
            existingData.UpdatedBy = Convert.ToInt32(_userContext.UserId);
            await _unitOfWork.Appointments.Update(existingData, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ResponseViewModel
            {
                Success = true,
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
            //var includeList = new List<Expression<Func<Appointment, object>>> { x => x.Doctor, x => x.Patient };

            var patient = await _unitOfWork.Doctors.GetById(doctorId, cancellationToken) ?? throw new Exception("Doctor not found");

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

            var patient = await _unitOfWork.Patients.GetById(patientId, cancellationToken) ?? throw new Exception("Patient not found");

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

        public async Task<ResponseViewModel<IEnumerable<AppointmentViewModel>>> GetAllFilterAppointments(FilterAppointmentRequestViewModel model, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Appointments.GetAllFilterAppointments(model, cancellationToken);
            
            return new ResponseViewModel<IEnumerable<AppointmentViewModel>>
            {
                Success = true,
                Data = _mapper.Map<List<AppointmentViewModel>>(result.Data),
            };
        }
    }
}
