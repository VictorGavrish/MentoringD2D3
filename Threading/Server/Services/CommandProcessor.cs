using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using Server.Commands;
using Server.Jobs;

namespace Server.Services
{
    public class CommandProcessor
    {
        private readonly BlockingCollection<Job> _allJobs;
        private readonly JobQueue _jobQueue;
        private readonly QueueProcessor _queueProcessor;
        private readonly Scheduler _scheduler;
        private int _lastTaskId;

        public CommandProcessor(BlockingCollection<Job> allJobs, Scheduler scheduler, JobQueue jobQueue, QueueProcessor queueProcessor)
        {
            _allJobs = allJobs;
            _scheduler = scheduler;
            _jobQueue = jobQueue;
            _queueProcessor = queueProcessor;
        }

        public void Process(ICommand command)
        {
            dynamic dynamicCommand = command;
            ProcessCommand(dynamicCommand);
        }

        private void ProcessCommand(CreateCommand command)
        {
            var jobToCreate = new StandaloneJob
            {
                Id = Interlocked.Increment(ref _lastTaskId),
                Iterations = command.Iterations,
                DelayInSeconds = command.DelayInSeconds
            };
            jobToCreate.RequiredJobs.AddRange(command.DependentTaskIds.Select(id => _allJobs.Single(t => t.Id == id)));

            _allJobs.Add(jobToCreate);

            if (command.StartUponCreation)
            {
                StartJob(jobToCreate);
            }
        }

        private void ProcessCommand(CreateParentCommand command)
        {
            var parentJob = new ParentJob {Id = Interlocked.Increment(ref _lastTaskId)};
            foreach (var childJob in command.ChildTaskIds.Select(childJobId => _allJobs.Single(j => j.Id == childJobId)))
            {
                parentJob.ChildJobs.Add(childJob);
            }
            _allJobs.Add(parentJob);
        }

        private void ProcessCommand(StartCommand command)
        {
            dynamic job = _allJobs.Single(at => at.Id == command.TaskId);
            StartJob(job);
        }

        private void ProcessCommand(StopCommand command)
        {
            dynamic job = _allJobs.Single(at => at.Id == command.TaskId);
            StopJob(job);
        }

        private void ProcessCommand(PauseCommand command)
        {
            var job = _allJobs.Single(at => at.Id == command.TaskId);
            job.Pause();
        }

        private void ProcessCommand(CancelCommand command)
        {
            dynamic job = _allJobs.Single(at => at.Id == command.TaskId);
            CancelJob(job);
        }

        private void ProcessCommand(ResetCommand command)
        {
            dynamic job = _allJobs.Single(at => at.Id == command.TaskId);
            if (command.IsStopSet)
            {
                StopJob(job);
            }
            job.Reset();
        }

        private void StartJob(StandaloneJob job)
        {
            if (job.HasRequirements)
            {
                _scheduler.Schedule(job);
            }
            else
            {
                _jobQueue.Enqueue(job);
            }
        }

        private void StartJob(ParentJob parentJob)
        {
            foreach (dynamic job in parentJob.ChildJobs)
            {
                StartJob(job);
            }
        }

        private void StopJob(StandaloneJob job)
        {
            _jobQueue.Remove(job);
            _queueProcessor.Stop(job);
            job.Stop();
        }

        private void StopJob(ParentJob parentJob)
        {
            foreach (dynamic job in parentJob.ChildJobs)
            {
                StopJob(job);
            }
        }

        private void CancelJob(StandaloneJob job)
        {
            _jobQueue.Remove(job);
            job.Cancel();
        }

        private void CancelJob(ParentJob parentJob)
        {
            foreach (dynamic job in parentJob.ChildJobs)
            {
                CancelJob(job);
            }
        }
    }
}