namespace PureLifeClinic.Application.BusinessObjects.AuthViewModels.Token
{
    public class RefreshTokenViewModel
    {
        public string Token { get; set; }

        public DateTime ExpireOn { get; set; }

        public DateTime CreateOn { get; set; }
    }
}
