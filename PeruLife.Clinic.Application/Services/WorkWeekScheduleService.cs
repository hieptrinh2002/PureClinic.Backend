﻿using AutoMapper;
using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Application.BusinessObjects.Schedule;
using PureLifeClinic.Application.BusinessObjects.Schedule.WorkWeeks;
using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IRepositories;

namespace PureLifeClinic.Application.Services
{
    public class WorkWeekScheduleService : BaseService<WorkWeek, WorkWeekScheduleViewModel>, IWorkWeekScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContext _userContext;
        private readonly IMapper _mapper;   
        public WorkWeekScheduleService(IMapper mapper, IUnitOfWork unitOfWork, IUserContext userContext) : base(mapper, unitOfWork.WorkWeeks)
        {
            _unitOfWork = unitOfWork;
            _userContext = userContext;
            _mapper = mapper;
        }
        public async Task<ResponseViewModel> RegisterWorkScheduleAsync(WorkScheduleRequestViewModel request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if the user exists
                var user = await _unitOfWork.Users.GetById(request.UserId, cancellationToken);
                if (user == null)
                {
                    return new ResponseViewModel
                    {
                        Success = false,
                        Message = "User does not exist."
                    };
                }

                // Validate the start date
                //if (request.WeekStartDate < DateTime.UtcNow.AddDays(7))
                //{
                //    return new ResponseViewModel
                //    {
                //        Success = false,
                //        Message = "The work schedule must start at least 1 week from today."
                //    };
                //}

                // Check if the work schedule already exists
                var isExists = await _unitOfWork.WorkWeeks.IsWorkScheduleExistsAsync(request.UserId, request.WeekStartDate, cancellationToken);
                if (isExists)
                {
                    return new ResponseViewModel
                    {
                        Success = false,
                        Message = "The user has already registered a work schedule for this week."
                    };
                }

                //Logic for calculating total hours
                var totalHours = 0.0;
                var validMorningStart = new TimeSpan(7, 0, 0);
                var validMorningEnd = new TimeSpan(12, 0, 0);
                var validAfternoonStart = new TimeSpan(13, 0, 0);
                var validAfternoonEnd = new TimeSpan(21, 0, 0);

                foreach (var day in request.WorkDays)
                {
                    if (!(day.StartTime >= validMorningStart && day.EndTime <= validMorningEnd ||
                          day.StartTime >= validAfternoonStart && day.EndTime <= validAfternoonEnd))
                    {
                        return new ResponseViewModel
                        {
                            Success = false,
                            Message = $"The working time on {day.DayOfWeek} is invalid."
                        };
                    }

                    var hoursWorked = (day.EndTime - day.StartTime).TotalHours;
                    if (hoursWorked <= 0)
                    {
                        return new ResponseViewModel
                        {
                            Success = false,
                            Message = $"The working time on {day.DayOfWeek} is invalid (start time must be before end time)."
                        };
                    }

                    totalHours += hoursWorked;
                }

                if (totalHours < 10)
                {
                    return new ResponseViewModel
                    {
                        Success = false,
                        Message = $"The total working hours for the entire month must be >= 30 hours. Currently, it is only {totalHours} hours."
                    };
                }

                var t = request.WeekStartDate.AddDays((int)DayOfWeek.Friday - (int)DayOfWeek.Monday);
                // Save the work schedule
                var workWeek = new WorkWeek
                {
                    UserId = request.UserId,
                    WeekStartDate = request.WeekStartDate,
                    WeekEndDate = request.WeekEndDate,
                    EntryDate = DateTime.Now,
                    EntryBy = int.Parse(_userContext.UserId),
                    WorkDays = request.WorkDays.Select(day => new WorkDay
                    {
                        DayOfWeek = day.DayOfWeek,
                        StartTime = day.StartTime,
                        Date = request.WeekStartDate.AddDays((int)day.DayOfWeek - (int)request.WeekStartDate.DayOfWeek),
                        EndTime = day.EndTime,
                        Notes = day.Notes,
                        EntryDate = DateTime.Now,
                        EntryBy = int.Parse(_userContext.UserId),
                        //EntryBy = string.IsNullOrEmpty(_userContext.UserId) ? (int?)null : int.Parse(_userContext.UserId)
                    }).ToList()
                };

                await _unitOfWork.WorkWeeks.Create(workWeek, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new ResponseViewModel
                {
                    Success = true,
                    Message = "The work schedule has been successfully created."
                };
            }
            catch (Exception ex)
            {
                return new ResponseViewModel
                {
                    Success = false,
                    Message = "create work schedule failed."
                };
            }
        }

        public async Task<ResponseViewModel<WorkWeekScheduleViewModel>> GetWorkSchedule(int userId, DateTime weekStartDate, CancellationToken cancellationToken)
        {
            var workWeek = await _unitOfWork.WorkWeeks.GetWorkSchedule(userId, weekStartDate, cancellationToken);
            if (workWeek == null)
                return new ResponseViewModel<WorkWeekScheduleViewModel>
                {
                    Success = false,
                    Message = "Get work week schedule failed"
                };

            return new ResponseViewModel<WorkWeekScheduleViewModel>
            {
                Success = true,
                Message = "Get work schedule successfully",
                Data = _mapper.Map<WorkWeekScheduleViewModel>(workWeek)
            };
        }
    }
}
