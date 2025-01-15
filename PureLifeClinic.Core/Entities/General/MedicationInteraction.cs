
namespace PureLifeClinic.Core.Entities.General
{
    public class MedicationInteraction : Base<int>
    {
        public string Medication { get; set; }
        public string InteractsWith { get; set; }
        public string Description { get; set; }
    }
}
