using PureLifeClinic.Application.BusinessObjects.EmailViewModels;
using PureLifeClinic.Infrastructure.BackgroundServices.RabbitMQ.Connection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PureLifeClinic.Infrastructure.BackgroundServices.RabbitMQ.Consumers
{
    internal class EmailConsumer
    {
        private readonly IRabbitMQConnection _connection;

        public EmailConsumer(IRabbitMQConnection connection)
        {
            _connection = connection;
        }

        public async Task ConsumeAsync()
        {
            using var channel = await _connection.Connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queue: "email_queue", durable: true, exclusive: false, autoDelete: false);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = System.Text.Json.JsonSerializer.Deserialize<MailRequestViewModel>(body);
                //Console.WriteLine($"Received email: {message?.To}");
                // Thực hiện logic gửi email tại đây
                await Task.CompletedTask;
            };

            await channel.BasicConsumeAsync(queue: "email_queue", autoAck: true, consumer: consumer);
            await Task.Delay(-1); // Giữ consumer chạy
        }
    }
}
