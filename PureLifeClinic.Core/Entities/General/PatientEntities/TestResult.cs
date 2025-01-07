namespace PureLifeClinic.Core.Entities.General
{
    public class TestResult
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string NormalRange { get; set; }
        public ResultStatus Status { get; set; }
    }

    public enum ResultStatus
    {
        Normal,
        Abnormal
    }
}
