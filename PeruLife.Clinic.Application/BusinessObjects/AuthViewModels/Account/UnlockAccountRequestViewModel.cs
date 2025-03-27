using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Application.BusinessObjects.AuthViewModels.Account
{
    public class UnlockAccountRequestViewModel
    {
        [Required]
        public int UserId { get; set; }
    }
}
