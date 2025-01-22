using Microsoft.EntityFrameworkCore;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Infrastructure.Data;
using System.Globalization;
using System.Linq.Expressions;

namespace PureLifeClinic.Infrastructure.Repositories
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
        public async Task<ResponseViewModel<List<Appointment>>> GetAllFilterAppointments(FilterAppointmentRequestViewModel model, CancellationToken cancellationToken)
        {
            if (model == null)
                throw new Exception("Invalid input to get filter appointment");

            var query = _dbContext.Appointments.AsQueryable().AsNoTracking();
            if (model.StartTime != null && model.EndTime != null)
                query = query.Where(a => a.AppointmentDate >= model.StartTime && a.AppointmentDate < model.EndTime && a.Status == model.Status);

            if (!string.IsNullOrEmpty(model.SortBy))
            {
                var orderByExpression = GetOrderByExpression<Appointment>(model.SortBy);
                query = model.SortOrder?.ToLower() == "desc" ? query.OrderByDescending(orderByExpression) : query.OrderBy(orderByExpression);
            }
            if (model.Top > 0)
            {
                query = query.Take(model.Top);
            }

            var result = await query.Include(a => a.Doctor).ThenInclude(u => u.User)
                                    .Include(a => a.Patient).ThenInclude(u => u.User)
                                    .ToListAsync(cancellationToken);

            return new ResponseViewModel<List<Appointment>>()
            {
                Success = true,
                Data = result
            };
        }

        public async Task<bool> IsExistsAppointment(int doctorId, DateTime date, CancellationToken cancellationToken)
        {
            return await _dbContext.Appointments.AsNoTracking().Where(a => a.DoctorId == doctorId && a.AppointmentDate == date).AnyAsync(cancellationToken);
        }
    }
}
