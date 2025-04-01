using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Application.BusinessObjects.AuthViewModels.ForgotPassword
{
    public class ForgotPasswordRequestViewModel
    {
        public string Email { get; set; }

        public string ClientUrl { get; set; }
    }
}
