using System.Linq;
using System.Text;
using Server.Jobs;

namespace Server.Tools
{
    public static class Other
    {
        public static string GetIdentifierString(this StandaloneJob task)
        {
            return $"#{task.Id:D2} ({task.Iterations:D2})";
            task.GetIdentifierString();
        }
    }

    public static class ConsoleJobExtensions
    {
        private const int DefaultProgressBarWidth = 30;

        public static string GetStatusLine<T>(this T task) where T : Job
        {
            return $"{task.GetIdentifierString()} | {task.GetProgressBar()} | {task.GetStatusString()}";
        }

        public static string GetProgressBar<T>(this T task, uint width = 0) where T : Job
        {
            if (width == 0)
            {
                width = DefaultProgressBarWidth;
            }
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < width; i++)
            {
                stringBuilder.Append(i < task.Percent * width ? "*" : " ");
            }
            stringBuilder.Append($" | {task.Percent:00.0%}");
            return stringBuilder.ToString();
        }

        public static string GetStatusString<T>(this T task) where T : Job
        {
            return task.Status.ToString();
        }

        public static string GetIdentifierString(this Job task)
        {
            return $"#{task.Id:D2} ({task.Iterations:D2})";
        }



        public static string GetType(this Job job)
        {
            if (job is StandaloneJob)
                return "Standalone";
            if (job is ParentJob)
                return "Parent";
            return "Unknown";

            job.GetIdentifierString();
        }
    }
}