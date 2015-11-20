namespace Server.Commands
{
    public class CancelCommand : ICommand
    {
        public int TaskId { get; set; }
    }
}