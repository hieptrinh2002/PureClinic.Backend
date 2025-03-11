using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Core.Interfaces.IServices
{
    public interface ITokenService
    {
        Task<ResponseViewModel<GenarateTokenViewModel>> GenerateJwtToken(int userId);
        RefreshToken GenerateRefreshToken();
    }

}
