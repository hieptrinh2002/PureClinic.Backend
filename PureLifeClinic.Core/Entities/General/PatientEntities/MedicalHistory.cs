namespace PureLifeClinic.Core.Entities.General
{
    //lịch sử y tế của bệnh nhân, bao gồm thông tin về các chuyến thăm khám, bác sĩ, chẩn đoán, đơn thuốc, ghi chú và tình trạng.
    public class MedicalHistory
    {
        public DateTime VisitDate { get; set; }
        public string Doctor { get; set; }
        public string Diagnosis { get; set; }
        public List<Prescription> Prescriptions { get; set; }
        public string Notes { get; set; }
        public MedicalHistoryStatus Status { get; set; }
    }
    public enum MedicalHistoryStatus
    {
        Improving,
        Stable,
        UnderTreatment,
        Discharged
    }
}
