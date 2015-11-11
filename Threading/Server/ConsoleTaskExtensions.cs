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
            return stringBuilder.ToString();
        }

        public static string GetStatusString(this AppTask task)
        {
            var statusString = task.Status.ToString();
            if (task.PlaceInQueue.HasValue)
            {
                statusString += $" ({task.PlaceInQueue.Value})";
            }
            return statusString;
        }

        public static string GetIdentifierString(this AppTask task)
        {
            return $"#{task.Id:D2} ({task.Duration:D2})";
        }
    }
}