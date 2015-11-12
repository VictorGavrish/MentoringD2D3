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
            var semaphore = GetSemaphoreFromUser();
            var cancelationSource = new CancellationTokenSource();
            var allTaskList = new BlockingCollection<AppTask>();

            var scheduler = new AppTaskScheduler(semaphore);
            var queue = new AppTaskQueue(allTaskList, scheduler);
            var listener = new SocketListener(IPAddress.Loopback, 6666);

            var display = new Display(allTaskList);
            var processor = new CommandProcessor(allTaskList, queue);
            var commandProcessor = new CommandDispatcher(listener, processor);

            commandProcessor.Start(cancelationSource.Token);
            display.Start(cancelationSource.Token);

            Console.ReadLine();

            cancelationSource.Cancel();
        }

        private static SemaphoreSlim GetSemaphoreFromUser()
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
            return semaphore;
        }
    }
}