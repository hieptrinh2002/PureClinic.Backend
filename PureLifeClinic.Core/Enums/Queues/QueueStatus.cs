namespace PureLifeClinic.Core.Enums.Queues
{
    public enum QueueStatus
    {
        Waiting,           // Đã lấy số, đang chờ gọi
        NoShow,            // Đã gọi nhưng không có phản hồi
        Expired,           // Hết hiệu lực, phải lấy số lại
        InProgress,
        Completed
    }
}
