using PureLifeClinic.Application.BusinessObjects.DoctorViewModels;
using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Application.BusinessObjects.AppointmentViewModels
{
    public class PatientAppointmentViewModel
    {
        public DateTime AppointmentDate { get; set; }

        public string Reason { get; set; }

        public AppointmentStatus Status { get; set; }

        public DoctorViewModel Doctor { get; set; }
    }
}
