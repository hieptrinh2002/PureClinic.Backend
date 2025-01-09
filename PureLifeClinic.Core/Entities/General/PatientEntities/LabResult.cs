namespace PureLifeClinic.Core.Entities.General
{
    // chứa thông tin kết quả xét nghiệm của bệnh nhân, bao gồm các kết quả cụ thể và trạng thái của xét nghiệm.
    public class LabResult: Base<int>
    {
        public DateTime TestDate { get; set; }
        public string TestType { get; set; }
        public List<TestResult> Results { get; set; }
        public LabTestStatus TestStatus { get; set; }
    }

    public enum LabTestStatus
    {
        Completed,
        Pending,
        InProgress
    }

    public class TestResult : Base<int>
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
