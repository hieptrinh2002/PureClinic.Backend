using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Core.Entities.Business
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

    public class WorkDayRequestViewModel
    {
        [Required]
        public DayOfWeek DayOfWeek { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        public string Notes { get; set; }
    }

    public class WeekEndDateMustBeSundayAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime weekEndDate && weekEndDate.DayOfWeek != DayOfWeek.Sunday)
            {
                return new ValidationResult("WeekEndDate must be a Sunday.");
            }

            return ValidationResult.Success;
        }
    }
    public class WeekStartDateMustBeMondayAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime weekStartDate && weekStartDate.DayOfWeek != DayOfWeek.Monday)
            {
                return new ValidationResult("WeekStartDate must be a Monday.");
            }

            return ValidationResult.Success;
        }
    }
}


//const workScheduleData = {
//  userId: 1, // ID của bác sĩ
//  weekStartDate: "2025-01-22", // Ngày bắt đầu tuần
//  weekEndDate: "2025-01-28",   // Ngày kết thúc tuần
//  workDays:
//[
//    {
//dayOfWeek: 1, // Thứ Hai
//      startTime: "08:00:00", // Giờ bắt đầu
//      endTime: "12:00:00",   // Giờ kết thúc
//      notes: "Buổi sáng thứ hai"
//    },
//    {
//dayOfWeek: 3, // Thứ Tư
//      startTime: "13:00:00",
//      endTime: "17:00:00",
//      notes: "Buổi chiều thứ tư"
//    }
//  ]
//};


//const validateWorkDays = (workDays) => {
//    const validMorningStart = "07:00:00";
//    const validMorningEnd = "12:00:00";
//    const validAfternoonStart = "13:00:00";
//    const validAfternoonEnd = "21:00:00";

//    let totalHours = 0;

//    for (const day of workDays) {
//        const startTime = day.startTime;
//        const endTime = day.endTime;

//        const isValidMorning =
//          startTime >= validMorningStart && endTime <= validMorningEnd;
//        const isValidAfternoon =
//          startTime >= validAfternoonStart && endTime <= validAfternoonEnd;

//        if (!(isValidMorning || isValidAfternoon))
//        {
//            return { isValid: false, message: `Thời gian ngày ${ day.dayOfWeek} không hợp lệ.` };
//        }

//        const hoursWorked =
//          (new Date(`1970 - 01 - 01T${ endTime }`) -
//            new Date(`1970 - 01 - 01T${ startTime }`)) /
//          1000 /
//          3600;

//        if (hoursWorked <= 0)
//        {
//            return { isValid: false, message: `Số giờ làm việc ngày ${ day.dayOfWeek} không hợp lệ.` };
//        }

//        totalHours += hoursWorked;
//    }

//    if (totalHours < 100)
//    {
//        return { isValid: false, message: `Tổng số giờ làm việc phải >= 100 giờ.Hiện tại là ${ totalHours} giờ.` };
//    }

//    return { isValid: true };
//};

//// Kiểm tra dữ liệu trước khi gửi
//const validationResult = validateWorkDays(workScheduleData.workDays);
//if (!validationResult.isValid)
//{
//    alert(validationResult.message);
//}
//else
//{
//    registerWorkSchedule(workScheduleData);
//}
