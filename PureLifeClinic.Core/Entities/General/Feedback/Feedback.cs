using System.ComponentModel.DataAnnotations.Schema;

namespace PureLifeClinic.Core.Entities.General.Feedback
{
    public class Feedback : Base<int>
    {
        public int PatientId { get; set; }
        [ForeignKey(nameof(PatientId))]
        public Patient? Patient { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public bool IsReported { get; set; } = false;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
