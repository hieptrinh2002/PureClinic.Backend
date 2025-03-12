using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Core.Entities.General
{
    public class Allergy : Base<int>
    {
        public string Substance { get; set; }
        public string Reaction { get; set; }
        public AllergySeverity Severity { get; set; }
    }
}
