using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class Scheduler
    {
        private readonly BlockingCollection<AppTask> _allTasks;
        private readonly SemaphoreSlim _semaphore;
        private readonly ConcurrentDictionary<int, AppTask> _waitingQueue = new ConcurrentDictionary<int, AppTask>();
        private readonly ConcurrentQueue<AppTask> _readyToStartQueue = new ConcurrentQueue<AppTask>(); 
        private int _queueNumber;

        public Scheduler(BlockingCollection<AppTask> allTasks, SemaphoreSlim semaphore)
        {
            _allTasks = allTasks;
            _semaphore = semaphore;
        }

        public void Enqueue(AppTask task)
        {
            task.Status = TaskStatus.Scheduled;
            if (!_waitingQueue.TryAdd(Interlocked.Increment(ref _queueNumber), task))
            {
                return;
            }

            task.Task.ContinueWith(t =>
            {
                _semaphore.Release();
                StartRefresh();
            });

            if (task.Delay.HasValue)
            {
                Task.Run(() =>
                {
                    Thread.Sleep(task.Delay.Value);
                    task.Delay = null;
                    StartRefresh();
                });
            }
            StartRefresh();
        }

        public void Pause(AppTask task)
        {
            task.PauseEvent.Reset();
            _semaphore.Release();
            StartRefresh();
        }

        public void StartRefresh()
        {
            Task.Run(() => RefreshReady());
        }

        private void RefreshReady()
        {
            while (true)
            {
                var keyValuePair = _waitingQueue.OrderBy(kvp => kvp.Key).FirstOrDefault(kvp => ShouldStart(kvp.Value));
                var taskToStart = keyValuePair.Value;
                var taskQueueNumber = keyValuePair.Key;

                if (taskToStart == null)
                {
                    break;
                }

                if (_waitingQueue.TryRemove(taskQueueNumber, out taskToStart))
                {
                    _readyToStartQueue.Enqueue(taskToStart);
                }
            }
            TryStartNext();
        }

        private void TryStartNext()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    _semaphore.Wait();
                    AppTask taskToStart;
                    var success = _readyToStartQueue.TryDequeue(out taskToStart);
                    if (!success)
                    {
                        _semaphore.Release();
                        return;
                    }

                    StartTask(taskToStart);
                }
            });
        }

        private static void StartTask(AppTask taskToStart)
        {
            if (taskToStart.Status == TaskStatus.Paused)
            {
                taskToStart.PauseEvent.Set();
            }
            else
            {
                taskToStart.Task.Start();
            }
        }

        private bool ShouldStart(AppTask task)
        {
            if (task.Delay.HasValue)
            {
                return false;
            }

            return task.DependentTaskIds.All(id => _allTasks.FirstOrDefault(t => t.Id == id)?.Status == TaskStatus.Complete);
        }
    }
}