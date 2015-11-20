using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Jobs
{
    public abstract class Job
    {
        public ManualResetEvent DoneEvent = new ManualResetEvent(false);
        public int Id { get; set; }
        public List<Job> RequiredJobs { get; } = new List<Job>(); // move to standalone
        public double Percent => Iterations > 0 ? CurrentIteration / (double) Iterations : 1;
        public virtual JobStatus Status { get; protected set; } = JobStatus.Created;
        public virtual int Iterations { get; set; }
        public virtual int CurrentIteration { get; set; }
        public abstract void Cancel();
        public abstract void Reset();
        public abstract void Queue();
        public abstract Task Schedule();
        public abstract void Pause();
        public abstract void Stop();
        public abstract Task Start();
    }
}