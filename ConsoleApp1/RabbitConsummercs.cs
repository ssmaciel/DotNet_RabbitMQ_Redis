using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class RabbitConsummercs
    {

        public void Consumir()
        {
            var factory = new ConnectionFactory() 
            {
                Uri = new Uri("amqp://devweek:UCpkGkG37xgDzNvG6rbkGDuVeyNc4vlcVtDorn3wPFhR9URvhDN6cSeOzL4HXN3u4Z3Ahv@rabbitmq-app:5672/platform")
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            //channel.QueueDeclare(queue: "hello",
            //                     durable: false,
            //                     exclusive: false,
            //                     autoDelete: false,
            //                     arguments: null);
            
            channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);

                channel.BasicAck(ea.DeliveryTag, false);
            };
            channel.BasicConsume(queue: "InfoFila",
                                 autoAck: false,
                                 consumer: consumer);
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

    }
}
