using PureLifeClinic.Application.BusinessObjects.FileViewModels.Response;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Application.Extentions.Mapping
{
    public static class FileMappingExts
    {
        public static MedicalFile MapToMedicalFile(this FileInfoVM fileInfoVM)
        {
            return new MedicalFile
            {
                FileName = fileInfoVM.FileName,
                FilePath = fileInfoVM.FilePath,
                FileSize = fileInfoVM.FileSize,
                FileType = fileInfoVM.FileType,
            };
        }
    }
}
