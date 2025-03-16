using RabbitMQ.Client;

namespace PureLifeClinic.Core.BackgroundServices.RabbitMQ.Connection
{
    public interface IRabbitMQConnection
    {
        public IConnection Connection { get; }
    }
}
