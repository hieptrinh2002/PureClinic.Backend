using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Application.BusinessObjects.AuthViewModels
{
    public class LoginViewModel
    {
        public string? UserName { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
