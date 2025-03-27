using PureLifeClinic.Application.BusinessObjects.AuthViewModels.Token;

namespace PureLifeClinic.Application.BusinessObjects.AuthViewModels
{
    public class AuthResultViewModel
    {
        public string? AccessToken { get; set; }

        public RefreshTokenViewModel RefreshToken { get; set; }

        public string UserEmail { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; }
    }
}
