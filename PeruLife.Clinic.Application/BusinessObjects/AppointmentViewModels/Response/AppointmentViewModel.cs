using PureLifeClinic.Application.BusinessObjects.DoctorViewModels.Response;
using PureLifeClinic.Application.BusinessObjects.PatientsViewModels;
using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Application.BusinessObjects.AppointmentViewModels.Response

{
    public class AppointmentViewModel
    {
        public int Id { get; set; }

        public DateTime AppointmentDate { get; set; }

        public string Reason { get; set; }

        public AppointmentStatus Status { get; set; }

        public PatientViewModel? Patient { get; set; }

        public DoctorViewModel? Doctor { get; set; }
    }
}
