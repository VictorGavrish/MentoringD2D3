using System.Collections.Concurrent;
using System.Threading.Tasks;
using Server.Jobs;

namespace Server.Services
{
    public class Scheduler
    {
        private readonly JobQueue _jobQueue;

        public Scheduler(JobQueue jobQueue)
        {
            _jobQueue = jobQueue;
        }

        public void Schedule(StandaloneJob task)
        {
            task.Schedule().ContinueWith(t => _jobQueue.Enqueue(task), TaskContinuationOptions.NotOnCanceled);
        }
    }
}