using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Exceptions;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Infrastructure.Data;

namespace PureLifeClinic.Infrastructure.Repositories
{
    public class WorkWeekScheduleRepository : BaseRepository<WorkWeek>, IWorkWeekScheduleRepository
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public WorkWeekScheduleRepository(ApplicationDbContext dbContext, UserManager<User> userManager,  IMapper mapper) : base(dbContext)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<bool> IsWorkScheduleExistsAsync(int userId, DateTime weekStartDate, CancellationToken cancellationToken)
        {
            return await _dbContext.WorkWeeks.AnyAsync(w => w.UserId == userId && w.WeekStartDate == weekStartDate, cancellationToken);
        }

        public async Task<WorkWeekScheduleViewModel?> GetWorkSchedule(int userId, DateTime weekStartDate, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString()) ?? throw new NotFoundException("User not found");

            var workWeek = await _dbContext.WorkWeeks
                .AsNoTracking()
                .Include(ww => ww.User)
                .Include(ww => ww.WorkDays)
                .Where(ww => ww.UserId == user.Id && ww.WeekStartDate == weekStartDate)
                .Select(ww => new WorkWeekScheduleViewModel
                {
                    UserId = user.Id,
                    WeekStartDate = weekStartDate,
                    WeekEndDate = ww.WeekEndDate,
                    WorkDays = ww.WorkDays.Select(wd => new WorkDayViewModel
                    {
                        DayOfWeek = wd.DayOfWeek,
                        StartTime = wd.StartTime,
                        EndTime = wd.EndTime,
                        Notes = wd.Notes,
                    }).ToList(),
                })
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            return workWeek;
        }
    }
}
