using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Server.Jobs;

namespace Server.Services
{
    public class JobQueue
    {
        private readonly ConcurrentQueue<QueueJob> _readyToStart = new ConcurrentQueue<QueueJob>();
        public event EventHandler JobAdded;

        public void Enqueue(StandaloneJob job)
        {
            job.Queue();
            _readyToStart.Enqueue(new QueueJob {Job = job});
            JobAdded?.Invoke(this, EventArgs.Empty);
        }

        public bool TryDequeue(out StandaloneJob job)
        {
            QueueJob queueJob;
            if (_readyToStart.TryDequeue(out queueJob))
            {
                if (queueJob.IsCancelled)
                {
                    return TryDequeue(out job);
                }
                job = queueJob.Job;
                return true;
            }
            job = null;
            return false;
        }

        public void Remove(StandaloneJob job)
        {
            foreach (var queueJob in _readyToStart.Where(qj => qj.Job == job))
            {
                queueJob.IsCancelled = true;
            }
        }

        public IList<StandaloneJob> GetSnapshot()
        {
            return _readyToStart.Select(qj => qj.Job).ToList();
        }

        private class QueueJob
        {
            public StandaloneJob Job { get; set; }
            public bool IsCancelled { get; set; }
        }
    }
}