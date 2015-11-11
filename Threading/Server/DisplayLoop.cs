using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class Display
    {
        private readonly BlockingCollection<AppTask> _allTasks;

        public Display(BlockingCollection<AppTask> allTasks)
        {
            _allTasks = allTasks;
        }

        public void Start(CancellationToken cancellationToken)
        {
            Task.Run(() => DisplayLoop(), cancellationToken);
        }

        private void DisplayLoop()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Task list:");
                foreach (var task in _allTasks.Where(t => t.Status != TaskStatus.Complete))
                {
                    Console.WriteLine(task.GetStatusLine());
                }
                Thread.Sleep(500);
            }
        }
    }
}