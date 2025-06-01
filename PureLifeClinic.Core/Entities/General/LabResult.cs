using PureLifeClinic.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace PureLifeClinic.Core.Entities.General
{
    // chứa thông tin kết quả xét nghiệm của bệnh nhân, bao gồm các kết quả cụ thể và trạng thái của xét nghiệm.
    public class LabResult: Base<int>
    {
        public int PatientId { get; set; }
        [ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; }

        public DateTime TestDate { get; set; }
        public string TestType { get; set; }
        public ICollection<TestResult> Results { get; set; } = new List<TestResult>();
        public LabTestStatus TestStatus { get; set; }
    }

    public class TestResult : Base<int>
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string NormalRange { get; set; }
        public ResultStatus Status { get; set; }
    }
}
