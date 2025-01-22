
namespace PureLifeClinic.Core.Entities.General
{
    public class MedicineInteraction : Base<int>
    {
        public string Medicine { get; set; }
        public string InteractsWith { get; set; }
        public string Description { get; set; }
    }
}
