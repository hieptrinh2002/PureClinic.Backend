using RabbitMQ.Client;

namespace PureLifeClinic.Infrastructure.BackgroundServices.RabbitMQ.Connection
{
    public class RabbitMQConnection : IRabbitMQConnection, IDisposable
    {
        private IConnection? _connection;
        public IConnection Connection => _connection!;

        public RabbitMQConnection()
        {
            InitializeConnection();
        }

        private async void InitializeConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
            };
            _connection = await factory.CreateConnectionAsync();
        }
        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
