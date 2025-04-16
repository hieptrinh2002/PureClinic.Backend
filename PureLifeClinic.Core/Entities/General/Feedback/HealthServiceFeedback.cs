using System.ComponentModel.DataAnnotations.Schema;

namespace PureLifeClinic.Core.Entities.General.Feedback
{
    public class HealthServiceFeedback: Feedback
    {
        public int HealthServiceId { get; set; }
        [ForeignKey(nameof(HealthServiceId))]
        public HealthService? HealthService { get; set; }
    }
}
