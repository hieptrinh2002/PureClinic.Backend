using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Application.BusinessObjects.AppointmentHealthServices
{
    public class AppointmentHealthServiceCreateViewModel
    {
        public int AppointmentId { get; set; }
        public int HealthServiceId { get; set; }
        public int RoomId { get; set; }
        public AppointmentHealthServiceStatus Status { get; set; } = AppointmentHealthServiceStatus.NotStarted;
        public double Price { get; set; }
        public int SortOrder { get; set; } = 0;
    }
}
