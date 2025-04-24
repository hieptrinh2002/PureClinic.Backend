using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Application.BusinessObjects.AppointmentHealthServices
{
    public class AppointmentHealthServiceViewModel
    {
        public int AppointmentId { get; set; }
        public string HealthServiceName { get; set; } = string.Empty;
        public int HealthServiceId { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public AppointmentHealthServiceStatus Status { get; set; } = AppointmentHealthServiceStatus.NotStarted;
        public double Price { get; set; }
    }
}
