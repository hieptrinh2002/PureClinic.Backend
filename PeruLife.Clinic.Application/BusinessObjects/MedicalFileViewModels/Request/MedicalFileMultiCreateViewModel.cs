using PureLifeClinic.Application.BusinessObjects.FileViewModels;

namespace PureLifeClinic.Application.BusinessObjects.MedicalFileViewModels.Request
{
    public class MedicalFileMultiCreateViewModel
    {
        public int MedicalReportId { get; set; }
        public List<FileUploadViewModel> Files { get; set; } = new List<FileUploadViewModel>();
    }
}
