using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class CommandProcessor
    {
        private readonly BlockingCollection<AppTask> _allTasks;
        private readonly SocketListener _listener;
        private readonly Scheduler _scheduler;
        private int _lastTaskId;

        public CommandProcessor(Scheduler scheduler, BlockingCollection<AppTask> allTasks, SocketListener listener)
        {
            _scheduler = scheduler;
            _allTasks = allTasks;
            _listener = listener;
        }

        public void Start(CancellationToken cancellationToken)
        {
            Task.Run(() => CommandLoop(), cancellationToken);
        }

        private void CommandLoop()
        {
            while (true)
            {
                try
                {
                    var command = _listener.GetNextTask();
                    ProcessCommand(command);
                }
                catch (Exception e)
                {
                }
            }
        }

        private void ProcessCommand(Command command)
        {
            switch (command.Type)
            {
            case CommandType.Create:
                ProcessCreateCommand(command);
                break;
            case CommandType.Start:
                ProcessStartCommand(command);
                break;
            case CommandType.Pause:
                ProcessPauseCommand(command);
                break;
            default:
                throw new ArgumentOutOfRangeException();
            }
        }

        private void ProcessPauseCommand(Command command)
        {
            var task = _allTasks.Single(at => at.Id == command.TaskId);
            _scheduler.Pause(task);
        }

        private void ProcessStartCommand(Command command)
        {
            var task = _allTasks.Single(at => at.Id == command.TaskId);
            _scheduler.Enqueue(task);
        }

        private void ProcessCreateCommand(Command command)
        {
            var appTask = new AppTask
            {
                Id = ++_lastTaskId,
                Duration = command.Duration,
                Delay = command.Delay,
                DependentTaskIds = command.DependentTaskIds,
                Status = TaskStatus.Stopped
            };
            _allTasks.Add(appTask);

            if (command.StartUponCreation)
            {
                _scheduler.Enqueue(appTask);
            }
        }
    }
}