using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Application.BusinessObjects.LabResultViewModels
{
    public class LabResultUpdateViewModel
    {
        public List<TestResultViewModel> Results { get; set; }
        public LabTestStatus Status { get; set; }
    }
  
    public class TestResultViewModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string NormalRange { get; set; }
        public ResultStatus Status { get; set; }
    }
}
