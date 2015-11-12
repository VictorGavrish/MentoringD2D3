using System.Linq;
using System.Text;

namespace Server
{
    public static class ConsoleTaskExtensions
    {
        private const int DefaultProgressBarWidth = 30;

        public static string GetStatusLine(this AppTask task)
        {
            return $"{task.GetIdentifierString()} | {task.GetProgressBar()} | {task.GetStatusString()}";
        }

        public static string GetProgressBar(this AppTask task, uint width = 0)
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

        public static string GetStatusString(this AppTask task)
        {
            var stringBuilder = new StringBuilder(task.Status.ToString());
            if (task.Status == TaskStatus.Queued)
            {
                if (task.PlaceInQueue.HasValue)
                {
                    stringBuilder.Append($" ({task.PlaceInQueue.Value})");
                }
                if (task.Delay.HasValue)
                {
                    stringBuilder.Append($" | delayed by {task.Delay.Value:G}");
                }
                if (task.DependentTaskIds.Any())
                {
                    stringBuilder.Append(" | waiting for task");
                    stringBuilder.Append(task.DependentTaskIds.Count == 1 ? " " : "s ");
                    foreach (var id in task.DependentTaskIds)
                    {
                        stringBuilder.Append($"{id} ");
                    }
                    stringBuilder.Append("to complete");
                }
            }
            return stringBuilder.ToString();
        }

        public static string GetIdentifierString(this AppTask task)
        {
            return $"#{task.Id:D2} ({task.Steps:D2})";
        }
    }
}