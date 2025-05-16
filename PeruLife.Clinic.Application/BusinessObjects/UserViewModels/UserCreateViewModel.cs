using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Application.BusinessObjects.UserViewModels
{
    public class UserCreateViewModel
    {
        public string FullName { get; set; }

        public string UserName { get; set; }

        public string? Email { get; set; }

        public string Password { get; set; }

        public int RoleId { get; set; }
    }
}
