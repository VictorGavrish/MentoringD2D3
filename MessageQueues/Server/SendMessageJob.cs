namespace Server
{
    using System.Text;

    using Common.Logging;

    using Quartz;

    using RabbitMQ.Client;

    public class SendMessageJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var dataMap = context.JobDetail.JobDataMap;

            var message = dataMap.GetString("message");

            var factory = new ConnectionFactory { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("hello", false, false, false, null);

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(string.Empty, "hello", null, body);
                }
            }
        }
    }
}