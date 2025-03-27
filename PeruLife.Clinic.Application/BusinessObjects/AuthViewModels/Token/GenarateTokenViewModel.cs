using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Application.BusinessObjects.AuthViewModels.Token
{
    public class GenarateTokenViewModel
    {
        public string AccessToken { get; set; }
        public RefreshToken RefreshToken { get; set; }

        public DateTime CreateOn { get; set; }
        public DateTime ExpireOn { get; set; }
        public string AccessTokenId { get; set; }
    }
}
