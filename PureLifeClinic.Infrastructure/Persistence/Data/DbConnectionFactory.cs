using Microsoft.Extensions.Options;
using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Interfaces.IRepositories;
using System.Data.SqlClient;
using System.Data;

namespace PureLifeClinic.Infrastructure.Persistence.Data
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly DatabaseSettings _dbSettings;

        public DbConnectionFactory(IOptions<DatabaseSettings> dbSettings)
        {
            _dbSettings = dbSettings.Value;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_dbSettings.PrimaryDbConnection);
        }
    }
}
