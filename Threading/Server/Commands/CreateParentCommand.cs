using System.Collections.Generic;

namespace Server.Commands
{
    public class CreateParentCommand : ICommand
    {
        public List<int> ChildTaskIds { get; set; } = new List<int>();
    }
}