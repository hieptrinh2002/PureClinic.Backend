using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Application.BusinessObjects.UserViewModels
{
    public class UserUpdateViewModel
    {
        public int Id { get; set; }

        [StringLength(100, MinimumLength = 2)]
        public required string FullName { get; set; }

        [StringLength(20, MinimumLength = 2)]
        public required string UserName { get; set; }

        [Required, EmailAddress]
        public required string Email { get; set; }

        [Required]
        public int RoleId { get; set; }
    }
}
