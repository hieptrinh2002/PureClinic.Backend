using Microsoft.EntityFrameworkCore;
using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Infrastructure.Data;
using System.Linq.Expressions;

namespace PureLifeClinic.Infrastructure.Repositories
{
    public class DoctorRepository : BaseRepository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<bool> IsDoctorAvailableForAppointment(
            int doctorId, DateTime appointmentDate, TimeSpan appointmentStartTime, CancellationToken cancellationToken)
        {
            var appointmentEndTime = appointmentStartTime.Add(TimeSpan.FromMinutes(Constants.AvgAppointmentTimeInMinute)); // Mặc định mỗi ca khám ít nhất 30 phút

            bool hasOverlappingAppointment = await _dbContext.Appointments
                .Where(a => a.DoctorId == doctorId && a.AppointmentDate == appointmentDate.Date)
                .AnyAsync(a => (a.StartTime <= appointmentStartTime && a.EndTime > appointmentStartTime) ||
                               (a.StartTime < appointmentEndTime && a.EndTime >= appointmentEndTime),
                           cancellationToken);

            return !hasOverlappingAppointment;
        }

        public async Task<IEnumerable<TimespanWorkDayViewModel>> GetDoctorWorkDaysTimespanOfWeek(
            int doctorId, DateTime weekStartDate, CancellationToken cancellationToken)
        {
            var workDays = await _dbContext.WorkDays
           .Join(_dbContext.WorkWeeks, wd => wd.WorkWeekId, ww => ww.Id, (wd, ww) => new { wd, ww })
           .Join(_dbContext.Users, ww_wd => ww_wd.ww.UserId, u => u.Id, (ww_wd, u) => new { ww_wd.wd, ww_wd.ww, u })
           .Join(_dbContext.Doctors, u_ww_wd => u_ww_wd.u.Id, d => d.UserId, (u_ww_wd, d) => new { u_ww_wd.wd, d })
           .Where(x => x.d.Id == doctorId && x.wd.Date >= weekStartDate && x.wd.Date < weekStartDate.AddDays(7))
           .Select(x => new TimespanWorkDayViewModel
           {
               WeekDate = x.wd.Date,
               StartTime = x.wd.StartTime,
               EndTime = x.wd.EndTime
           })
           .ToListAsync(cancellationToken);
            return workDays;
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentOfWeek(int doctorId, DateTime weekStartDate, CancellationToken cancellationToken)
        {
            var appointments = await _dbContext.Appointments
            .Where(a => a.DoctorId == doctorId && a.AppointmentDate >= weekStartDate && a.AppointmentDate < weekStartDate.AddDays(7) && a.Status == AppointmentStatus.Confirmed)
            .OrderBy(a => a.AppointmentDate.TimeOfDay)
            .ToListAsync(cancellationToken);

            return appointments;
        }

        public async Task<bool> IsDoctorWorkingThatWeek(int doctorId, DateTime appointmentDate, CancellationToken cancellationToken)
        {
            return await (from ww in _dbContext.WorkWeeks
                          join u in _dbContext.Users on ww.UserId equals u.Id
                          join d in _dbContext.Doctors on u.Id equals d.UserId
                          where d.Id == doctorId &&
                                ww.WeekStartDate <= appointmentDate.Date &&
                                ww.WeekEndDate >= appointmentDate.Date &&
                                ww.WorkWeekStatus == WorkWeekStatus.Approved
                          select ww)
                          .AnyAsync(cancellationToken);
        }

        public async Task<bool> IsDoctorAvailableThatDay(int doctorId, DateTime appointmentDate, CancellationToken cancellationToken)
        {
            var appointmentTime = appointmentDate.TimeOfDay;

            return await _dbContext.WorkDays
                .Join(_dbContext.WorkWeeks, wd => wd.WorkWeekId, ww => ww.Id, (wd, ww) => new { wd, ww })
                .Join(_dbContext.Users, ww_wd => ww_wd.ww.UserId, u => u.Id, (ww_wd, u) => new { ww_wd.wd, ww_wd.ww, u })
                .Join(_dbContext.Doctors, u_ww_wd => u_ww_wd.u.Id, d => d.UserId, (u_ww_wd, d) => new { u_ww_wd.wd, d })
                .Where(x => x.d.Id == doctorId &&
                            x.wd.Date == appointmentDate.Date &&
                            x.wd.StartTime <= appointmentTime &&
                            x.wd.EndTime >= appointmentTime)
                .AnyAsync(cancellationToken);
        }

        public async Task<int> GetDoctorWorkingHours(int doctorId, DateTime workDate)
        {
            var totalWorkHours = await _dbContext.WorkDays
                .Join(_dbContext.WorkWeeks, wd => wd.WorkWeekId, ww => ww.Id, (wd, ww) => new { wd, ww })
                .Join(_dbContext.Users, ww_wd => ww_wd.ww.UserId, u => u.Id, (ww_wd, u) => new { ww_wd.wd, ww_wd.ww, u })
                .Join(_dbContext.Doctors, u_ww_wd => u_ww_wd.u.Id, d => d.UserId, (u_ww_wd, d) => new { u_ww_wd.wd, d })
                .Where(x => x.d.Id == doctorId && x.wd.Date == workDate.Date)
                .SumAsync(x => EF.Functions.DateDiffMinute(x.wd.StartTime, x.wd.EndTime));

            return totalWorkHours / 60;
        }

        public async Task<bool> IsDoctorAvailableForNewAppointment(int doctorId, DateTime appointmentDate, CancellationToken cancellationToken)
        {
            var totalAppointments = await _dbContext.Appointments
                .Where(a => a.DoctorId == doctorId && a.AppointmentDate.Date == appointmentDate.Date)
                .CountAsync(cancellationToken);

            return totalAppointments < Constants.MaxDoctorAppointmentPerday; // Giới hạn mỗi ngày
        }

        public async Task<int> GetMaxAppointmentsPerDay(int doctorId, DateTime workDate)
        {
            var totaHoursWorked = await GetDoctorWorkingHours(doctorId, workDate);

            int maxAppointments = (int)Math.Floor((totaHoursWorked / 8.0) * 15); // ratio 8 hours => 15 appointment
            return Math.Max(maxAppointments, 0);
        }

        public async Task<IEnumerable<Patient>> GetAllPatient(int doctorId, CancellationToken cancellationToken)
        {
           return await _dbContext.Patients
                .Join(_dbContext.Appointments, p => p.Id, a => a.PatientId, (p, a) => new { p, a })
                .Where(x => x.a.DoctorId == doctorId)
                .Where(x=> x.a.Status == AppointmentStatus.Completed)   
                .OrderBy(x=> x.a.AppointmentDate)
                .Select(x => x.p)
                .ToListAsync(cancellationToken);
        }

        public async Task<PaginatedDataViewModel<Patient>> GetPaginatedPaitentData(
             int doctorId,
             int pageNumber,
             int pageSize,
             List<ExpressionFilter> filters,
             string sortBy,
             string sortOrder,
             CancellationToken cancellationToken)
        {
            // Define base query with required includes
            var query = _dbContext.Patients
                .Include(p => p.User)
                .Include(p => p.Appointments)
                .Where(p => p.Appointments.Any(a => a.DoctorId == doctorId && a.Status == AppointmentStatus.Completed))
                .AsNoTracking(); // Improves read performance

            // Apply filters if provided
            if (filters != null && filters.Any())
            {
                var expressionTree = ExpressionBuilder.ConstructAndExpressionTree<Patient>(filters);
                query = query.Where(expressionTree);
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                var orderByExpression = GetOrderByExpression<Patient>(sortBy);
                query = sortOrder?.ToLower() == "desc"
                    ? query.OrderByDescending(orderByExpression)
                    : query.OrderBy(orderByExpression);
            }

            // Get total count before pagination
            var totalCount = await query.CountAsync(cancellationToken);

            // Apply pagination
            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedDataViewModel<Patient>(data, totalCount);
        }

        private Expression<Func<T, object>> GetOrderByExpression<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyName);
            var conversion = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(conversion, parameter);

        }
    }
}
