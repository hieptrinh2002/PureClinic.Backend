using PureLifeClinic.Application.BusinessObjects.FileViewModels;

namespace PureLifeClinic.Application.BusinessObjects.MedicalFileViewModels
{
    public class MedicalFileMultiCreateViewModel
    {
        public int MedicalReportId { get; set; }
        public List<FileUploadViewModel> Files { get; set; } = new List<FileUploadViewModel>();
    }
}
