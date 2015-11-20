using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Jobs
{
    public class ParentJob : Job
    {
        public override JobStatus Status
        {
            get
            {
                var childTaskArray = ChildJobs.Select(ct => ct.Status).ToArray();

                if (childTaskArray.All(s => s == JobStatus.Complete))
                {
                    return JobStatus.Complete;
                }
                if (childTaskArray.All(s => s == JobStatus.Created))
                {
                    return JobStatus.Created;
                }

                if (childTaskArray.Any(s => s == JobStatus.Cancelled))
                {
                    return JobStatus.Cancelled;
                }
                if (childTaskArray.Any(s => s == JobStatus.Running))
                {
                    return JobStatus.Running;
                }
                if (childTaskArray.Any(s => s == JobStatus.Scheduled))
                {
                    return JobStatus.Scheduled;
                }
                if (childTaskArray.Any(s => s == JobStatus.Queued))
                {
                    return JobStatus.Queued;
                }
                if (childTaskArray.Any(s => s == JobStatus.Stopped))
                {
                    return JobStatus.Stopped;
                }
                if (childTaskArray.Any(s => s == JobStatus.Paused))
                {
                    return JobStatus.Paused;
                }

                return JobStatus.Stopped;
            }
        }

        public override int Iterations
        {
            get { return ChildJobs.Sum(at => at.Iterations); }
        }

        public override int CurrentIteration
        {
            get { return ChildJobs.Sum(at => at.CurrentIteration); }
        }

        public ConcurrentBag<Job> ChildJobs { get; } = new ConcurrentBag<Job>();

        private void DoAllChildren(Action<Job> action)
        {
            foreach (var job in ChildJobs)
            {
                action(job);
            }
        }

        public override void Cancel()
        {
            DoAllChildren(j => j.Cancel());
        }

        public override void Reset()
        {
            DoAllChildren(j => j.Reset());
        }

        public override void Queue()
        {
            DoAllChildren(j => j.Queue());
        }

        public override Task Schedule()
        {
            var tasks = new List<Task>();
            DoAllChildren(j => tasks.Add(j.Schedule()));
            return Task.WhenAll(tasks);
        }

        public override void Pause()
        {
            DoAllChildren(j => j.Pause());
        }

        public override void Stop()
        {
            DoAllChildren(j => j.Stop());
        }

        public override Task Start()
        {
            var tasks = new List<Task>();
            DoAllChildren(j => tasks.Add(j.Start()));
            return Task.WhenAll(tasks);
        }
    }
}