using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Core.Entities.Business
{
    public class FilterRequestViewModel
    {
    }

    public class FilterAppointmentRequestViewModel
    {
        public DateTime? StartTime { get; set; }   

        public DateTime? EndTime { get; set; } = DateTime.Now;

        public int Top { get; set; } = 10;

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Confirmed;

        public string SortBy { get; set; } = "AppoimentDate";

        public string SortOrder { get; set; } = "desc";

        public string? DoctorId { get; set; }

        public string? PatientId { get; set; }
    }
}
