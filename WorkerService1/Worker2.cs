using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace WorkerService1
{
    public class Worker2 : BackgroundService
    {
        private readonly ILogger<Worker2> _logger;

        public Worker2(ILogger<Worker2> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var factory = new ConnectionFactory()
                {
                    Uri = new Uri("amqp://devweek:UCpkGkG37xgDzNvG6rbkGDuVeyNc4vlcVtDorn3wPFhR9URvhDN6cSeOzL4HXN3u4Z3Ahv@rabbitmq-app:5672/platform")
                };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.BasicQos(0, 1, false);
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    _logger.LogInformation(" [x] Received WK 2 {0}", message);
                     
                    channel.BasicAck(ea.DeliveryTag, false);
                };
                channel.BasicConsume(queue: "InfoFila",
                                     autoAck: false,
                                     consumer: consumer);
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}