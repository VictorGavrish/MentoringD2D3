namespace Server.Commands
{
    public class ResetCommand : ICommand
    {
        public int TaskId { get; set; }
        public bool IsStopSet { get; set; }
    }
}