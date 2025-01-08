using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Core.Entities.Business
{
    public class UnlockAccountRequest
    {
        [Required]
        public int UserId { get; set; }
    }
}
