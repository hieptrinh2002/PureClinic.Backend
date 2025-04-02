namespace PureLifeClinic.Core.Enums
{
    public enum QueueStatus
    {
        Waiting,           // Đã lấy số, đang chờ gọi
        Called,            // Đã gọi tên
        NoShow,            // Đã gọi nhưng không có phản hồi
        Expired,           // Hết hiệu lực, phải lấy số lại
        Completed
    }
}
