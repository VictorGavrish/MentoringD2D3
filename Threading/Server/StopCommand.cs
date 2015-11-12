namespace Server
{
    public class StopCommand : ICommand
    {
        public int TaskId { get; set; }
    }
}