using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Application.BusinessObjects.AuthViewModels.ForgotPassword
{
    public class ForgotPasswordRequestViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string ClientUrl { get; set; }
    }
}
