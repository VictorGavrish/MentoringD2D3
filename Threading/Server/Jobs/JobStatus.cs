namespace Server.Jobs
{
    public enum JobStatus
    {
        Created,
        Scheduled,
        Queued,
        Running,
        Stopped,
        Paused,
        Complete,
        Cancelled
    }
}