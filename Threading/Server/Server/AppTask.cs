using System.Threading;

namespace Server
{
    public class AppTask
    {
        public int Id { get; set; }
        public int Duration { get; set; }
        public double Percent { get; set; }
        public TaskStatus Status { get; set; }

        public void Perform()
        {
            Status = TaskStatus.Running;
            for (var i = 1; i <= Duration; i++)
            {
                Thread.Sleep(1000);
                Percent = i / (double) Duration;
            }
            Status = TaskStatus.Complete;
        }
    }
}