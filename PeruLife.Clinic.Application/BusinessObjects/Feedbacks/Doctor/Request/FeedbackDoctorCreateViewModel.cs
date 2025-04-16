namespace PureLifeClinic.Application.BusinessObjects.Feedbacks.Doctor.Request
{
    public class FeedbackDoctorCreateViewModel
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
