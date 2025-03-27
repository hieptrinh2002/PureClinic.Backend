namespace PureLifeClinic.Application.BusinessObjects.FileViewModels
{
    public class FileMultiUploadViewModel
    {
        public int MedicalReportId { get; set; }

        public List<FileUploadViewModel> Files { get; set; } = new List<FileUploadViewModel>();
    }
}
