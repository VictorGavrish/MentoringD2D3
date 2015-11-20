using System;
using System.Collections.Generic;

namespace Server.Commands
{
    public class CreateCommand : ICommand
    {
        public int? DelayInSeconds { get; set; }
        public bool StartUponCreation { get; set; }
        public List<int> DependentTaskIds { get; set; } = new List<int>();
        public int Iterations { get; set; }
        public List<int> ChildTaskIds { get; set; }
    }
}