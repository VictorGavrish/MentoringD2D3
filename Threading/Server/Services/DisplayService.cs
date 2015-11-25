using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Server.Jobs;
using Server.Tools;

namespace Server.Services
{
    public class DisplayService : IDisposable
    {
        private readonly BlockingCollection<Job> _allTasks;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly JobQueue _jobQueue;

        public DisplayService(BlockingCollection<Job> allTasks, JobQueue jobQueue)
        {
            _allTasks = allTasks;
            _jobQueue = jobQueue;
            Task.Factory.StartNew(DisplayLoop, _cancellationTokenSource.Token);
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }

        private void DisplayLoop()
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                Console.Clear();
                Console.WriteLine($"TICKS: {TickBarrier.TicksTotal}");
                Console.WriteLine("Running: ");
                foreach (var task in _allTasks.Where(t => t.Status == JobStatus.Running || t.Status == JobStatus.Paused))
                {
                    Console.WriteLine(task.GetStatusLine());
                }
                Console.WriteLine("Queued: ");
                {
                    foreach (var standaloneJob in _jobQueue.GetSnapshot())
                    {
                        Console.Write($"{standaloneJob.GetIdentifierString()} ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("Scheduled: ");
                foreach (var job in _allTasks.Where(t => t.Status == JobStatus.Scheduled))
                {
                    Console.WriteLine($"{job.GetIdentifierString()} {job.GetStatusString()}");
                }
                Console.WriteLine("Not started: ");
                foreach (var job in _allTasks.Where(t => t.Status == JobStatus.Created || t.Status == JobStatus.Stopped))
                {
                    Console.WriteLine($"{job.GetIdentifierString()} {job.GetStatusString()}");
                }
                Console.WriteLine("Completed: ");
                foreach (var job in _allTasks.Where(t => t.Status == JobStatus.Complete))
                {
                    Console.WriteLine($"{job.GetIdentifierString()} {job.GetStatusString()}");
                }

                Thread.Sleep(500);
            }
        }
    }
}