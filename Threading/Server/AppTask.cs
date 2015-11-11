using System;
using System.Collections.Generic;
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
        public int Duration { get; set; }
        public double Percent { get; set; }
        public TaskStatus Status { get; set; }
        public TimeSpan? Delay { get; set; }
        public List<int> DependentTaskIds { get; set; }

        public Action<AppTask> Action { get; set; } = appTask =>
        {
            for (var i = 1; i <= appTask.Duration; i++)
            {
                appTask.WaitIfPauseRequested();
                Thread.Sleep(1000);
                appTask.Percent = i / (double) appTask.Duration;
            }
        };

        public void WaitIfPauseRequested()
        {
            if (PauseEvent.IsSet)
            {
                return;
            }
            Status = TaskStatus.Paused;
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