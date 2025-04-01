using PureLifeClinic.Application.BusinessObjects.MedicalFileViewModels.Request;
using PureLifeClinic.Application.BusinessObjects.MedicalFileViewModels.Response;
using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Application.Interfaces.IServices
{
    public interface IMedicalFileService : IBaseService<MedicalFileViewModel>
    {
        new Task<ResponseViewModel<string>> Create(MedicalFileCreateViewModel model, CancellationToken cancellationToken);
        Task<ResponseViewModel<List<MedicalFile>>> CreateMultipleAsync(MedicalFileMultiCreateViewModel model, CancellationToken cancellationToken);
    }
}
