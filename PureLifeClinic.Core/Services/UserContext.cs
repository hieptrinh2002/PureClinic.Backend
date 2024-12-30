using PureLifeClinic.Core.Interfaces.IServices;

namespace PureLifeClinic.Core.Services
{
    public class UserContext : IUserContext
    {
        public string UserId { get; set; }
    }
}
