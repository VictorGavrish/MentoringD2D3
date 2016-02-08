namespace Server
{
    using System;
    using System.Diagnostics;

    using Common.Logging;
    using Common.Logging.Simple;

    using Quartz;
    using Quartz.Collection;
    using Quartz.Impl;

    internal class Program
    {
        private static readonly ILog Log;

        static Program()
        {
            LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter { Level = LogLevel.Info };
            Log = LogManager.GetLogger(typeof(Program));
        }

        private static void Main(string[] args)
        {
            var scheduler = StdSchedulerFactory.GetDefaultScheduler();

            var intervals = GenerateIntervals();

            var jobDictionary = CreateJobs(intervals);

            scheduler.Start();

            scheduler.ScheduleJobs(jobDictionary, true);

            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();

            scheduler.Shutdown();
        }

        private static int[] GenerateIntervals()
        {
            var random = new Random();

            var numberOfJobs = random.Next(2, 5);
            Log.InfoFormat("Number of jobs: {0}", numberOfJobs);

            var intervals = new int[numberOfJobs];
            for (var index = 0; index < numberOfJobs; index++)
            {
                intervals[index] = random.Next(2, 10);
                Log.InfoFormat("Job {0} interval: {1}", index, intervals[index]);
            }

            return intervals;
        }

        private static System.Collections.Generic.Dictionary<IJobDetail, ISet<ITrigger>> CreateJobs(int[] intervals)
        {
            Log.Info("Building jobs");

            var jobDictionary = new System.Collections.Generic.Dictionary<IJobDetail, ISet<ITrigger>>();

            for (var index = 0; index < intervals.Length; index++)
            {
                var interval = intervals[index];
                Log.InfoFormat("Creating Job {0} with interval {1}...", index, interval);

                var job =
                    JobBuilder.Create<SendMessageJob>()
                        .WithIdentity("sendMessageJob" + index)
                        .UsingJobData("message", $"Job {index} with interval {interval} from process {Process.GetCurrentProcess().Id}")
                        .Build();

                var trigger =
                    TriggerBuilder.Create()
                        .WithIdentity("trigger" + index)
                        .StartNow()
                        .WithSimpleSchedule(x => x.WithIntervalInSeconds(interval).RepeatForever())
                        .Build();

                var set = new HashSet<ITrigger> { trigger };

                jobDictionary.Add(job, set);
            }

            return jobDictionary;
        }
    }
}