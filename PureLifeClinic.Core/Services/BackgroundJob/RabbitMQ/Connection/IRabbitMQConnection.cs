using RabbitMQ.Client;

namespace PureLifeClinic.Core.Services.BackgroundJob.RabbitMQ.Connection
{
    public interface IRabbitMQConnection
    {
        public IConnection Connection { get; }  
    }
}
