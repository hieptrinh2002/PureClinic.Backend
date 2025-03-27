using PureLifeClinic.Application.BusinessObjects.Schedule.ValidateAttributes;
using PureLifeClinic.Application.BusinessObjects.Schedule.WorkDays;
using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Application.BusinessObjects.Schedule
{
    public class WorkScheduleRequestViewModel
    {
        public int UserId { get; set; }

        [Required]
        [WeekStartDateMustBeMonday]
        public DateTime WeekStartDate { get; set; }

        [Required]
        [WeekEndDateMustBeSunday]
        public DateTime WeekEndDate { get; set; }

        [Required]
        [MaxLength(14, ErrorMessage = "WorkDays cannot exceed 14 sessions per week.")]
        public List<WorkDayRequestViewModel> WorkDays { get; set; } = new List<WorkDayRequestViewModel>();
    }
}
