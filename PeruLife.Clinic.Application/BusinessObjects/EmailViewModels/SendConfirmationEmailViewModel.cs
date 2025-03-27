using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Application.BusinessObjects.EmailViewModels
{
    public class SendConfirmationEmailViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string ConfirmationLink { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}
