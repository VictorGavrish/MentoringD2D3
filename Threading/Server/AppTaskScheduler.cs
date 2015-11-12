using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class AppTaskScheduler
    {
        private readonly ConcurrentDictionary<int, AppTask> _readyToStart = new ConcurrentDictionary<int, AppTask>();
        private readonly SemaphoreSlim _semaphore;
        private readonly AutoResetEvent _refreshEvent = new AutoResetEvent(false);

        public AppTaskScheduler(SemaphoreSlim semaphore)
        {
            _semaphore = semaphore;
            Task.Factory.StartNew(TaskStartLoop);
        }

        public void Pause(AppTask appTask)
        {
            appTask.PauseEvent.Reset();
            _semaphore.Release();
        }

        public void Refresh()
        {
            _refreshEvent.Set();
        }

        public void Schedule(int queueNumber, AppTask appTask)
        {
            appTask.Status = TaskStatus.Scheduled;
            _readyToStart.TryAdd(queueNumber, appTask);
        }

        private void TaskStartLoop()
        {
            while (true)
            {
                _refreshEvent.WaitOne();
                while (true)
                {
                    _semaphore.Wait();
                    var keyValuePair = _readyToStart.FirstOrDefault();
                    var taskToStart = keyValuePair.Value;
                    var taskQueueNumber = keyValuePair.Key;

                    if (taskToStart == null || !_readyToStart.TryRemove(taskQueueNumber, out taskToStart))
                    {
                        _semaphore.Release();
                        break;
                    }

                    StartTask(taskToStart);
                }
            }
        }

        private void StartTask(AppTask taskToStart)
        {
            if (taskToStart.Status == TaskStatus.Stopped)
            {
                taskToStart.PauseEvent.Set();
            }
            else
            {
                taskToStart.Task.ContinueWith(t => _semaphore.Release());
                taskToStart.Task.Start();
            }
        }
    }
}