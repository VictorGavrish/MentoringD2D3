namespace Server
{
    public enum TaskStatus
    {
        Created,
        Queued,
        Scheduled,
        Running,
        Waiting,
        Stopped,
        Complete
    }
}