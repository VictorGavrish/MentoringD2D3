using System;
using System.Collections.Concurrent;
using System.Net;
using Server.Jobs;
using Server.Services;

namespace Server
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var maxConcurrentJobs = GetMaxConcurrentJobs();
            var allTaskList = new BlockingCollection<Job>();
            var socketServer = new SocketServer(IPAddress.Loopback, 6666);
            var jobQueue = new JobQueue();
            var scheduler = new Scheduler(jobQueue);
            using (var queueProcessor = new QueueProcessor(maxConcurrentJobs, jobQueue))
            {
                var commandProcessor = new CommandProcessor(allTaskList, scheduler, jobQueue, queueProcessor);
                using (var dispatcher = new CommandDispatcherService(socketServer, commandProcessor))
                using (var display = new DisplayService(allTaskList, jobQueue))
                {
                    Console.ReadLine();
                }
            }
        }

        private static int GetMaxConcurrentJobs()
        {
            int maxConcurrentJobs;
            while (true)
            {
                Console.Write("Please enter the maximum amount of concurrent tasks: ");
                var input = Console.ReadLine();

                if (int.TryParse(input, out maxConcurrentJobs))
                {
                    break;
                }
                Console.WriteLine($"Error parsing as integer: {input}");
            }
            return maxConcurrentJobs;
        }
    }
}