using PureLifeClinic.Core.Services.BackgroundJob.RabbitMQ.Connection;
using PureLifeClinic.Core.Services.BackgroundJob.RabbitMQ.Producers.Interfaces;
using RabbitMQ.Client;

namespace PureLifeClinic.Core.Services.BackgroundJob.RabbitMQ.Producers
{
    public class EmailProducer : IMessageProducer
    {
        private readonly IRabbitMQConnection _connection;
        public EmailProducer(IRabbitMQConnection connection)
        {
            _connection = connection;
        }

        public async void SendMessage<T>(T message)
        {
            using var channel = await _connection.Connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "queue_email", durable: true, exclusive: false, autoDelete: false);

            var body = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(message);

            await channel.BasicPublishAsync(exchange: "", routingKey: "email_queue", body: body);
        }
    }
}
