namespace PureLifeClinic.Core.Entities.General
{
    // thông tin về dị ứng của bệnh nhân, bao gồm chất gây dị ứng, phản ứng và mức độ nghiêm trọng.
    public class Allergy
    {
        public string Substance { get; set; }
        public string Reaction { get; set; }
        public AllergySeverity Severity { get; set; }
    }

    public enum AllergySeverity
    {
        Mild,
        Moderate,
        Severe
    }
}
