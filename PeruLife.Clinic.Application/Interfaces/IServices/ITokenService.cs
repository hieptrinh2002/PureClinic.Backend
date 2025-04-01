using PureLifeClinic.Application.BusinessObjects.AuthViewModels.Token;
using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Application.Interfaces.IServices
{
    public interface ITokenService
    {
        Task<ResponseViewModel<GenerateTokenViewModel>> GenerateJwtToken(int userId);
        RefreshToken GenerateRefreshToken();
    }

}
