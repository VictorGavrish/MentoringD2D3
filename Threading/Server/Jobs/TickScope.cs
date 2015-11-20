using System;

namespace Server.Jobs
{
    public class TickScope : IDisposable
    {
        private readonly TickBarrier _tickBarrier;

        public TickScope(TickBarrier tickBarrier)
        {
            _tickBarrier = tickBarrier;
        }

        void IDisposable.Dispose()
        {
            _tickBarrier.LeaveTickScope();
        }
    }
}