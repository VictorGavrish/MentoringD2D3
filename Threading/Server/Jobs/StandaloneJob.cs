using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Jobs
{
    public class StandaloneJob : Job
    {
        private static readonly TickBarrier TickBarrier = new TickBarrier();
        private readonly CancellationToken _cancellationToken;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEventSlim _pauseEvent = new ManualResetEventSlim(true);
        private readonly Task _task;

        public StandaloneJob()
        {
            _cancellationToken = _cancellationTokenSource.Token;
            _task = new Task(Action, _cancellationToken);
        }

        public int? DelayInSeconds { get; set; }
        public Action Step { get; set; } = () => Thread.Sleep(1000);
        public bool HasRequirements => DelayInSeconds.HasValue || RequiredJobs.Any();
        public bool RequirementsMet { get; set; }

        private void Action()
        {
            using (TickBarrier.EnterTickScope())
            {
                Status = JobStatus.Running;
                for (CurrentIteration = 0; CurrentIteration < Iterations; CurrentIteration++)
                {
                    if (_cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }
                    TickBarrier.SignalAndWait(_cancellationToken);
                    Step();
                    WaitIfPauseRequested();
                }
                DoneEvent.Set();
                Status = JobStatus.Complete;
            }
        }

        private void WaitIfPauseRequested()
        {
            if (_pauseEvent.IsSet)
            {
                return;
            }
            Status = JobStatus.Paused;
            TickBarrier.LeaveTickScope();
            _pauseEvent.Wait(_cancellationToken);
            TickBarrier.EnterTickScope();
            Status = JobStatus.Running;
        }

        public override void Cancel()
        {
            _cancellationTokenSource.Cancel();
            Status = JobStatus.Cancelled;
        }

        public override void Reset()
        {
            CurrentIteration = 0;
        }

        public override void Queue()
        {
            Status = JobStatus.Queued;
        }

        public override Task Schedule()
        {
            Status = JobStatus.Scheduled;
            var tasks = new List<Task>();
            if (DelayInSeconds.HasValue)
            {
                tasks.Add(Task.Factory.StartNew(() => Thread.Sleep(1000 * DelayInSeconds.Value), _cancellationToken));
            }
            if (RequiredJobs.Any())
            {
                tasks.Add(Task.Factory.StartNew(() => WaitHandle.WaitAll(RequiredJobs.Select(rj => rj.DoneEvent).ToArray()), _cancellationToken));
            }
            return Task.WhenAll(tasks).ContinueWith(t => RequirementsMet = true, TaskContinuationOptions.NotOnCanceled);
        }

        public override void Pause()
        {
            _pauseEvent.Reset();
            Status = JobStatus.Paused;
        }

        public override void Stop()
        {
            _pauseEvent.Reset();
            Status = JobStatus.Stopped;
        }

        public override Task Start()
        {
            if (_task.Status == TaskStatus.Created)
            {
                _task.Start();
            }
            if (!_pauseEvent.IsSet)
            {
                _pauseEvent.Set();
            }
            return _task;
        }
    }
}