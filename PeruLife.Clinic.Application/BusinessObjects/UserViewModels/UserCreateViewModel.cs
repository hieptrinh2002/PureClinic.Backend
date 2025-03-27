using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Application.BusinessObjects.UserViewModels
{
    public class UserCreateViewModel
    {
        [Required, StringLength(100, MinimumLength = 2)]
        public string FullName { get; set; }

        [Required, StringLength(20, MinimumLength = 2)]
        public string UserName { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required, StringLength(50, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public int RoleId { get; set; }
    }
}
