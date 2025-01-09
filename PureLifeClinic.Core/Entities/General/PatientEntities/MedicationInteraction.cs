
namespace PureLifeClinic.Core.Entities.General
{
    //lưu trữ thông tin về các tương tác thuốc của bệnh nhân, ví dụ như Atenolol và Paracetamol có thể tương tác với nhau.
    public class MedicationInteraction : Base<int>
    {
        public string Medication { get; set; }
        public string InteractsWith { get; set; }
        public string Description { get; set; }
    }
}
