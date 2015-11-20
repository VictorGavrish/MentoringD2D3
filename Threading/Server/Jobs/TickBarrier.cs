using System.Threading;

namespace Server.Jobs
{
    public class TickBarrier
    {
        public static int TicksTotal { get; set; }
        
        private readonly Barrier _barrier = new Barrier(0, barrier => TicksTotal++);

        public TickScope EnterTickScope()
        {
            _barrier.AddParticipant();
            return new TickScope(this);
        }

        public void LeaveTickScope()
        {
            _barrier.RemoveParticipant();
        }

        public void SignalAndWait(CancellationToken cancellationToken)
        {
            _barrier.SignalAndWait(cancellationToken);
        }
    }
}