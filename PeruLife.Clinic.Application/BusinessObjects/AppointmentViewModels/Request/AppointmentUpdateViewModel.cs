using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Application.BusinessObjects.AppointmentViewModels.Request
{
    public class AppointmentUpdateViewModel
    {
        public DateTime AppointmentDate { get; set; }

        public string? Reason { get; set; }

        public int? DoctorId { get; set; }

        public AppointmentStatus Status { get; set; }

        public string? ReferredPerson { get; set; }
    }
}
