using System.ComponentModel.DataAnnotations.Schema;

namespace PureLifeClinic.Core.Entities.General.Feedback
{
    public class DoctorFeedback: Feedback
    {
        public int DoctorId { get; set; }
        [ForeignKey(nameof(DoctorId))]
        public Doctor Doctor { get; set; }

        public int PatientId { get; set; }
        [ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; }
    }
}
