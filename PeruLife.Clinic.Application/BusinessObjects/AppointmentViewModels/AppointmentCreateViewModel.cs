namespace PureLifeClinic.Application.BusinessObjects.AppointmentViewModels
{
    public class AppointmentCreateViewModel
    {
        public DateTime AppointmentDate { get; set; }

        public string? Reason { get; set; }

        public string? OtherReason { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }
    }
}
