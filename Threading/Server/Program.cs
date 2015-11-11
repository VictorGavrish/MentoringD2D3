using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;

namespace Server
{
    public class Program
    {
        private static void Main(string[] args)
        {
            SemaphoreSlim semaphore;
            while (true)
            {
                Console.WriteLine("Please enter the maximum amount of concurrent tasks:");
                var input = Console.ReadLine();
                int maxConcurrentTasks;
                if (int.TryParse(input, out maxConcurrentTasks))
                {
                    semaphore = new SemaphoreSlim(maxConcurrentTasks);
                    break;
                }
                Console.WriteLine($"Error parsing as integer: {input}");
            }

            var cancelationSource = new CancellationTokenSource();

            var allTaskList = new BlockingCollection<AppTask>();

            var scheduler = new Scheduler(allTaskList, semaphore);
            var listener = new SocketListener(IPAddress.Loopback, 6666);

            var display = new Display(allTaskList);
            var commandProcessor = new CommandProcessor(scheduler, allTaskList, listener);

            commandProcessor.Start(cancelationSource.Token);
            display.Start(cancelationSource.Token);

            Console.ReadKey();
            cancelationSource.Cancel();
        }
    }
}