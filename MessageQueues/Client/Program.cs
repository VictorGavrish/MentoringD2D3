namespace Client
{
    using System;
    using System.Text;

    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("hello", false, false, false, null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body;
                            var message = Encoding.UTF8.GetString(body);
                            Console.WriteLine("Recieved: {0}", message);
                        };

                    channel.BasicConsume(queue: "hello", noAck: true, consumer: consumer);

                    Console.WriteLine("Press Enter to exit");
                    Console.ReadLine();
                }
            }
        }
    }
}