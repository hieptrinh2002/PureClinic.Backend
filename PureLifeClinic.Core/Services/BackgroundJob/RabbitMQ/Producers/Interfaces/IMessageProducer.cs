namespace PureLifeClinic.Core.Services.BackgroundJob.RabbitMQ.Producers.Interfaces
{
    public interface IMessageProducer
    {
        void SendMessage<T>(T message);
    }
}
