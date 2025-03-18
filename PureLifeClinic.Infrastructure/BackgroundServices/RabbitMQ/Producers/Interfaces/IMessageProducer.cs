namespace PureLifeClinic.Infrastructure.BackgroundServices.RabbitMQ.Producers.Interfaces
{
    public interface IMessageProducer
    {
        void SendMessage<T>(T message);
    }
}
