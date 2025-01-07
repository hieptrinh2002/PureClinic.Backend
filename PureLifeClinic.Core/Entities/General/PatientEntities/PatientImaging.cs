namespace PureLifeClinic.Core.Entities.General
{
    // lưu trữ các hình ảnh y tế của bệnh nhân, ví dụ như X-Ray hoặc CT Scan, bao gồm các thuộc tính như loại hình ảnh,
    // URL hình ảnh, ngày tải lên, báo cáo và tình trạng.
    public class PatientImaging
    {
        public ImagingType Type { get; set; }
        public string ImageUrl { get; set; }
        public DateTime UploadDate { get; set; }
        public string Report { get; set; }
        public ImagingStatus Status { get; set; }
    }
    public enum ImagingType
    {
        XRay,
        CTScan,
        MRI,
        Ultrasound
    }

    public enum ImagingStatus
    {
        Normal,
        UnderReview,
        Abnormal
    }
}
