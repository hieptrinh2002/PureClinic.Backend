using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Core.Entities.Business
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
