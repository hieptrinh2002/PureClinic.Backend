
namespace PureLifeClinic.Application.BusinessObjects.Feedbacks.Doctor.Response
{
    public class DoctorFeedbackSummaryViewModel
    {
        public int DoctorId { get; set; }
        public double AverageRating { get; set; }
        public int TotalFeedbacks { get; set; }
    }
}
