using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class AppTaskQueue
    {
        private readonly BlockingCollection<AppTask> _allTasks;
        private readonly AutoResetEvent _refreshEvent = new AutoResetEvent(false);
        private readonly AppTaskScheduler _scheduler;
        private readonly ConcurrentDictionary<int, AppTask> _waitingQueue = new ConcurrentDictionary<int, AppTask>();
        private int _queueNumber;

        public AppTaskQueue(BlockingCollection<AppTask> allTasks, AppTaskScheduler scheduler)
        {
            _allTasks = allTasks;
            _scheduler = scheduler;
            Task.Factory.StartNew(TaskScheduleLoop);
        }

        public void Enqueue(AppTask task)
        {
            task.Status = TaskStatus.Queued;
            task.PlaceInQueue = Interlocked.Increment(ref _queueNumber);
            if (!_waitingQueue.TryAdd(task.PlaceInQueue.Value, task))
            {
                return;
            }

            task.Task.ContinueWith(t => _refreshEvent.Set());

            if (task.Delay.HasValue)
            {
                Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(task.Delay.Value);
                    task.Delay = null;
                    _refreshEvent.Set();
                });
            }
            _refreshEvent.Set();
        }

        public void Pause(AppTask task)
        {
            _scheduler.Pause(task);
            _refreshEvent.Set();
        }

        private void TaskScheduleLoop()
        {
            while (true)
            {
                _refreshEvent.WaitOne();
                while (true)
                {
                    var keyValuePair = _waitingQueue.FirstOrDefault(kvp => ShouldStart(kvp.Value));
                    var taskToStart = keyValuePair.Value;
                    var taskQueueNumber = keyValuePair.Key;

                    if (taskToStart == null)
                    {
                        break;
                    }

                    if (_waitingQueue.TryRemove(taskQueueNumber, out taskToStart))
                    {
                        _scheduler.Schedule(taskQueueNumber, taskToStart);
                    }
                }
                _scheduler.Refresh();
            }
        }

        private bool ShouldStart(AppTask task)
        {
            if (task.Delay.HasValue)
            {
                return false;
            }

            return task.DependentTaskIds.All(id => _allTasks.SingleOrDefault(t => t.Id == id)?.Status == TaskStatus.Complete);
        }
    }
}