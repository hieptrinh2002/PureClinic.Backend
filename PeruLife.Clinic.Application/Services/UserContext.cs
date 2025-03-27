using PureLifeClinic.Application.Interfaces.IServices;

namespace PureLifeClinic.Application.Services
{
    public class UserContext : IUserContext
    {
        public string UserId { get; set; }
    }
}
