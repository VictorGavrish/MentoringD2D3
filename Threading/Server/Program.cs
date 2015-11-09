using System;
using System.Collections.Concurrent;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class Program
    {
        private static readonly BlockingCollection<AppTask> Tasks = new BlockingCollection<AppTask>();

        private static void Main(string[] args)
        {
            var cancelationSource = new CancellationTokenSource();
            Task.Run(() => StartServerLoop(), cancelationSource.Token);
            Task.Run(() => StartDisplayLoop(), cancelationSource.Token);
            Console.ReadKey();
            cancelationSource.Cancel();
        }

        private static void StartDisplayLoop()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Task list:");
                foreach (var task in Tasks) // put in task
                {
                    Console.WriteLine(task.GetStatusLine());
                }
                Thread.Sleep(500);
            }
        }

        private static void StartServerLoop()
        {
            var listener = new SocketListener(IPAddress.Loopback, 6666);
            var lastTaskId = 1;

            // create queue // show tick count

            while (true)
            {
                var task = listener.GetNextTask();
                task.Id = lastTaskId;
                Tasks.Add(task);
                Task.Factory.StartNew(() => task.Perform(), TaskCreationOptions.AttachedToParent);
                lastTaskId++;
            }
        }
    }

    public static class ConsoleTaskExtensions
    {
        const int ProgressBarWidth = 30;

        public static string GetStatusLine(this AppTask task)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"#{task.Id:D2} ({task.Duration:D2}) | ");
            stringBuilder.Append(task.GetProgressBar());
            stringBuilder.Append(task.Status == TaskStatus.Complete ? " | Complete" : " | Running");
            return stringBuilder.ToString();
        }

        public static string GetProgressBar(this AppTask task, int width = 0)
        {
            if (width == 0)
            {
                width = ProgressBarWidth;
            }
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < width; i++)
            {
                stringBuilder.Append(i < task.Percent * width ? "*" : " ");
            }
            return stringBuilder.ToString();
        }
    }
}