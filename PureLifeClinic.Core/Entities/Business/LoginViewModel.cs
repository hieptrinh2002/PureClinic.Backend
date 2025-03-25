using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Core.Entities.Business
{
    public class LoginViewModel
    {
        public string? UserName { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
