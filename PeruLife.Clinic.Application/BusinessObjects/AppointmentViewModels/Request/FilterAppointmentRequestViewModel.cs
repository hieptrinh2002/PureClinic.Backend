using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Application.BusinessObjects.AppointmentViewModels.Request
{
    public class FilterAppointmentRequestViewModel
    {
        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; } = DateTime.Now;

        public int Top { get; set; } = 10;

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Confirmed;

        public string SortBy { get; set; } = "AppointmentDate";

        public string SortOrder { get; set; } = SortOrderType.DESC;

        public string? DoctorId { get; set; }

        public string? PatientId { get; set; }
    }
}
