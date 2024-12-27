
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;

namespace Project.Core.Entities.Business
{
    public class AuthResultViewModel
    {
        public string? AccessToken { get; set; }

        public RefreshTokenViewModel RefreshToken { get; set; } 
    }
}
