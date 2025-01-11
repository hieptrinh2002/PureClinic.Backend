﻿using AutoMapper;
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

        public AppointmentService(IMapper mapper, IUserContext userContext, IUnitOfWork unitOfWork)
            : base(mapper, unitOfWork.Appointments)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userContext = userContext;
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
            var entity = _mapper.Map<Appointment>(model);
            entity.EntryDate = DateTime.Now;
            entity.EntryBy = Convert.ToInt32(_userContext.UserId);

            var result = await _unitOfWork.Appointments.Create(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<AppointmentViewModel>(result);
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

        public async Task<IEnumerable<DoctorAppointmentViewModel>> GetAllAppointmentsByDoctorIdAsync(int doctorId, CancellationToken cancellationToken)
        {
            //var includeList = new List<Expression<Func<Appointment, object>>> { x => x.Doctor, x => x.Patient };
            var filters = new List<ExpressionFilter>
            {
                new ExpressionFilter()
                {
                    PropertyName = "DoctorId",
                    Value = doctorId,
                    Comparison = Comparison.Equal,
                }
            };
            var includeExpressions = new List<Func<IQueryable<Appointment>, IIncludableQueryable<Appointment, object>>>
            {
                query => query.Include(a => a.Doctor),  // Include và ThenInclude cho Doctor
                query => query.Include(a => a.Patient).ThenInclude(p => p.User)  // Include cho Patient
            };

            try
            {
                var entities = (await _unitOfWork.Appointments.GetAll(includeExpressions, filters, cancellationToken))
                          .OrderBy(e => e.Status).ThenBy(e => e.AppointmentDate).ToList();
   
                return _mapper.Map<List<DoctorAppointmentViewModel>>(entities);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
