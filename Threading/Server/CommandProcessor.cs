using System.Collections.Concurrent;
using System.Linq;

namespace Server
{
    public class CommandProcessor
    {
        private readonly BlockingCollection<AppTask> _allTasks;
        private readonly AppTaskQueue _queue;
        private int _lastTaskId;

        public CommandProcessor(BlockingCollection<AppTask> allTasks, AppTaskQueue queue)
        {
            _allTasks = allTasks;
            _queue = queue;
        }

        public void Process(ICommand command)
        {
            dynamic dynamicCommand = command;
            ProcessCommand(dynamicCommand);
        }

        private void ProcessCommand(StopCommand command)
        {
            var task = _allTasks.Single(at => at.Id == command.TaskId);
            _queue.Pause(task);
        }

        private void ProcessCommand(StartCommand command)
        {
            var task = _allTasks.Single(at => at.Id == command.TaskId);
            _queue.Enqueue(task);
        }

        private void ProcessCommand(CreateCommand command)
        {
            var appTask = new AppTask
            {
                Id = ++_lastTaskId,
                Steps = command.Steps,
                Delay = command.Delay,
                DependentTaskIds = command.DependentTaskIds,
                Status = TaskStatus.Created
            };
            _allTasks.Add(appTask);

            if (command.StartUponCreation)
            {
                _queue.Enqueue(appTask);
            }
        }
    }
}