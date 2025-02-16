﻿using AutoMapper;
using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Enums;
using PureLifeClinic.Core.Exceptions;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Core.Interfaces.IServices;
using System.Linq.Expressions;

namespace PureLifeClinic.Core.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;   
        public DoctorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DoctorViewModel>> GetAll(CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.Users.GetAllDoctor(cancellationToken);
            var doctorViewModels = _mapper.Map<IEnumerable<DoctorViewModel>>(entities);
            return doctorViewModels;
        }

        public async Task<DoctorViewModel> GetById(int id, CancellationToken cancellationToken)
        {
            var resut = await _unitOfWork.Users.GetDoctorById(id, cancellationToken);
            return _mapper.Map<DoctorViewModel>(resut);
        }

        public async Task<IEnumerable<AppointmentSlotViewModel>> GetDoctorAvailableTimeSlots(int doctorId, DateTime weekStartDate, CancellationToken cancellationToken)
        {
            var workDayTimespans = await _unitOfWork.Doctors.GetDoctorWorkDaysTimespanOfWeek(doctorId, weekStartDate, cancellationToken);

            if (!workDayTimespans.Any())
                throw new NotFoundException($"Doctor id - {doctorId} don't have any workday timespan in thí week");

            var appointments = await _unitOfWork.Doctors.GetAllAppointmentOfWeek(doctorId, weekStartDate, cancellationToken);
            List<AppointmentSlotViewModel> availableSlots = new();

            foreach (var workDay in workDayTimespans)
            {
                TimeSpan currentStart = workDay.StartTime;
                TimeSpan workEnd = workDay.EndTime;
                int maxAppointments = await _unitOfWork.Doctors.GetMaxAppointmentsPerDay(doctorId, workDay.WeekDate);  // number of appointment slot base on working time.
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

        public async Task<PaginatedDataViewModel<DoctorViewModel>> GetPaginatedData(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var includeList = new List<Expression<Func<User, object>>> { x => x.Role, x=> x.Doctor};

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

            var paginatedDataViewModel = new PaginatedDataViewModel<DoctorViewModel>(_mapper.Map<IEnumerable<DoctorViewModel>>(paginatedData.Data), paginatedData.TotalCount);
            return paginatedDataViewModel;
        }

        public Task<ResponseViewModel> Update(DoctorUpdateViewModel model, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
