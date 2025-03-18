using RabbitMQ.Client;

namespace PureLifeClinic.Infrastructure.BackgroundServices.RabbitMQ.Connection
{
    public interface IRabbitMQConnection
    {
        public IConnection Connection { get; }
    }
}
