namespace PureLifeClinic.Core.Enums
{
    public enum PatientStatus
    {
        New,              // Bệnh nhân mới, chưa điều trị
        UnderTreatment,   // Đang điều trị
        Stable,           // Tình trạng đã ổn định
        Discharged,       // Đã xuất viện
        Referred          // Được chuyển viện
    }
}
