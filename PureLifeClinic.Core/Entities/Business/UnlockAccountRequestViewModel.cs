using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Core.Entities.Business
{
    public class UnlockAccountRequestViewModel
    {
        [Required]
        public int UserId { get; set; }
    }
}
