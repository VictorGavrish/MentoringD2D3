using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class AppTask
    {
        public readonly ManualResetEventSlim PauseEvent = new ManualResetEventSlim(true);

        public AppTask()
        {
            Task = new Task(ActionWrapper);
        }

        public Task Task { get; private set; }
        public int Id { get; set; }
        public int? PlaceInQueue { get; set; }
        public int Steps { get; set; }
        public double Percent { get; set; }
        public TaskStatus Status { get; set; }
        public TimeSpan? Delay { get; set; }
        public List<int> DependentTaskIds { get; set; }
        public ConcurrentBag<ManualResetEvent> BeforeCompletionEvents { get; set; } = new ConcurrentBag<ManualResetEvent>();
        public ManualResetEvent CompletedEvent { get; set; } = new ManualResetEvent(false);

        public Action Step { get; set; } = () => Thread.Sleep(1000);
        public Action<AppTask> Action { get; set; } = appTask =>
        {
            for (var i = 0; i < appTask.Steps; i++)
            {
                appTask.WaitIfStopRequested();
                appTask.Percent = i / (double)appTask.Steps;
                appTask.Step();
            }
            appTask.Percent = 0.9999;
            appTask.Status = TaskStatus.Waiting;
            appTask.Percent = 1;
            WaitOnChildProcesses(appTask);
            appTask.CompletedEvent.Set();
        };

        private static void WaitOnChildProcesses(AppTask appTask)
        {
            var waitingEvents = appTask.BeforeCompletionEvents.ToArray();
            if (waitingEvents.Any())
            {
                WaitHandle.WaitAll(waitingEvents);
            }
        }

        public void WaitIfStopRequested()
        {
            if (PauseEvent.IsSet)
            {
                return;
            }
            Status = TaskStatus.Stopped;
            PauseEvent.Wait();
            Status = TaskStatus.Running;
        }

        private void ActionWrapper()
        {
            Status = TaskStatus.Running;
            Action(this);
            Status = TaskStatus.Complete;
        }
    }
}