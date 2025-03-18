using System.Data;

namespace PureLifeClinic.Core.Interfaces.IRepositories
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
