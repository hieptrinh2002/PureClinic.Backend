using PureLifeClinic.Application.BusinessObjects.PatientsViewModels;
using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Application.BusinessObjects.AppointmentViewModels
{
    public class DoctorAppointmentViewModel
    {
        public DateTime AppointmentDate { get; set; }

        public string Reason { get; set; }

        public AppointmentStatus Status { get; set; }

        public PatientViewModel Patient { get; set; }
    }
}
