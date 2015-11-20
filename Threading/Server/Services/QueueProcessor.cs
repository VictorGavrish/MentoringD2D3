using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Server.Jobs;

namespace Server.Services
{
    public class QueueProcessor : IDisposable
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly JobQueue _jobQueue;
        private readonly AutoResetEvent _refreshEvent = new AutoResetEvent(false);
        private readonly SemaphoreSlim _semaphore;

        public QueueProcessor(int maxConcurrentJobs, JobQueue jobQueue)
        {
            _semaphore = new SemaphoreSlim(maxConcurrentJobs);
            _jobQueue = jobQueue;
            Task.Factory.StartNew(TaskStartLoop, _cancellationTokenSource.Token);
            _jobQueue.JobAdded += JobQueueOnJobAdded;
        }

        private void JobQueueOnJobAdded(object sender, EventArgs eventArgs)
        {
            _refreshEvent.Set();
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }

        private void TaskStartLoop()
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                _refreshEvent.WaitOne();
                while (true)
                {
                    _semaphore.Wait();
                    StandaloneJob job;
                    if (!_jobQueue.TryDequeue(out job))
                    {
                        _semaphore.Release();
                        break;
                    }
                    job.Start().ContinueWith(t =>
                    {
                        _semaphore.Release();
                        _refreshEvent.Set();
                    });
                }
            }
        }

        public void Stop(Job task)
        {
            task.Stop();
            _semaphore.Release();
            _refreshEvent.Set();
        }
    }
}