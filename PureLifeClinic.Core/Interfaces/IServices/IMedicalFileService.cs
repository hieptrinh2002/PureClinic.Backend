using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Core.Interfaces.IServices
{
    public interface IMedicalFileService: IBaseService<MedicalFileViewModel>
    {
        new Task<ResponseViewModel<string>> Create(MedicalFileCreateViewModel model, CancellationToken cancellationToken);
        Task<ResponseViewModel<List<MedicalFile>>> CreateMultipleAsync(MedicalFileMultiCreateViewModel model, CancellationToken cancellationToken);
    }
}
