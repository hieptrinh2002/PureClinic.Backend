using PureLifeClinic.Application.BusinessObjects.DoctorViewModels.Response;
using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Application.BusinessObjects.AppointmentViewModels.Response
{
    public class PatientAppointmentViewModel
    {
        public DateTime AppointmentDate { get; set; }

        public string Reason { get; set; }

        public AppointmentStatus Status { get; set; }

        public DoctorViewModel Doctor { get; set; }
    }
}
