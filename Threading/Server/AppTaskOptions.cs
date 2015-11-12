using System;
using System.Collections.Generic;

namespace Server
{
    public class AppTaskOptions
    {
        public AppTaskOptions()
        {
        }

        public int Id { get; set; }
        public int? PlaceInQueue { get; set; }
        public int Duration { get; set; }
        public TimeSpan? Delay { get; set; }
        public List<int> DependentTaskIds { get; set; }
    }
}