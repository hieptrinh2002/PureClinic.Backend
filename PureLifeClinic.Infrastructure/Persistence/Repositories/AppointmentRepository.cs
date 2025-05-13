using Microsoft.EntityFrameworkCore;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Enums;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Infrastructure.Persistence.Data;
using System.Linq.Expressions;

namespace PureLifeClinic.Infrastructure.Persistence.Repositories
{
    public class AppointmentRepository : BaseRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        private Expression<Func<T, object>> GetOrderByExpression<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyName);
            var conversion = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(conversion, parameter);
        }

        public async Task<List<Appointment>> GetAllFilterAppointments(
            DateTime? startTime, 
            string? doctorId, 
            string? patientId, 
            DateTime? endTime, 
            int top, 
            AppointmentStatus status, 
            string sortBy, 
            string sortOrder, 
            CancellationToken cancellationToken)
        {
            var query = _dbContext.Appointments.AsQueryable().AsNoTracking();
            if (startTime != null && endTime != null)
                query = query.Where(a => a.AppointmentDate >= startTime && a.AppointmentDate < endTime && a.Status == status);

            if (!string.IsNullOrEmpty(doctorId))
                query = query.Where(a => a.DoctorId == int.Parse(doctorId));

            if (!string.IsNullOrEmpty(patientId))
                query = query.Where(a => a.PatientId == int.Parse(patientId));

            if (!string.IsNullOrEmpty(sortBy))
            {
                var orderByExpression = GetOrderByExpression<Appointment>(sortBy);
                query = sortOrder?.ToLower() == SortOrderType.DESC ? query.OrderByDescending(orderByExpression) : query.OrderBy(orderByExpression);
            }
            if (top > 0)
            {
                query = query.Take(top);
            }

            var result = await query.Include(a => a.Doctor).ThenInclude(u => u.User)
                                    .Include(a => a.Patient).ThenInclude(u => u.User)
                                    .ToListAsync(cancellationToken);

            return result;
        }

        public async Task<bool> IsExistsAppointment(int doctorId, DateTime date, CancellationToken cancellationToken)
        {
            return await _dbContext.Appointments.AsNoTracking()
                .Where(a => a.DoctorId == doctorId && a.AppointmentDate == date)
                .AnyAsync(cancellationToken);
        }

        public async Task<List<Appointment>> GetUpcomingAppointmentsBatchAsync(int pageIndex, int batchSize, int hoursBefore)
        {
            DateTime targetTime = DateTime.UtcNow.AddHours(hoursBefore);

            return await _dbContext.Appointments
                .Where(a => a.AppointmentDate >= DateTime.UtcNow && a.AppointmentDate <= targetTime && a.Status == AppointmentStatus.Approved)
                .OrderBy(a => a.AppointmentDate)
                .Skip(pageIndex * batchSize)
                .Take(batchSize)
                .Include(a => a.Patient)
                .ThenInclude(p => p.User)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetLateAppointments(DateTime now)
        {
            return await _dbContext.Appointments
                .Include(a => a.Patient)    
                .ThenInclude(p => p.User)   
                .Where(apt => apt.AppointmentDate < now && apt.Status == AppointmentStatus.Approved).ToListAsync();
        }

        public async Task<int> CountConsecutiveMissedAppointments(int patientId)
        {
            var result =  await _dbContext.Appointments
            .Where(a => a.PatientId == patientId && a.Status == AppointmentStatus.NoShowCanceled)
            .OrderByDescending(a => a.AppointmentDate)
            .Take(3)
            .ToListAsync();
            return result.Count;
        }
    }
}
