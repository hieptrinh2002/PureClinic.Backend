using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Core.Entities.Business
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
