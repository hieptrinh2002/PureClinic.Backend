using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Application.BusinessObjects.AppointmentViewModels.Request
{
    public class InPersonAppointmentCreateViewModel
    {
        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string? Email { get; set; }

        public Gender? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public DateTime AppointmentDate { get; set; }

        public string Reason { get; set; } = "Anual health check";
        public string? ReferredPerson { get; set; }

        public int DoctorId { get; set; }
    }
}
