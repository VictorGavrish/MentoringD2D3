using System;
using System.Collections.Generic;

namespace Server
{
    public class CreateCommand : ICommand
    {
        public TimeSpan? Delay { get; set; }
        public bool StartUponCreation { get; set; }
        public List<int> DependentTaskIds { get; set; } = new List<int>();
        public int Steps { get; set; }
        public List<int> ChildTaskIds { get; set; } 
    }
}