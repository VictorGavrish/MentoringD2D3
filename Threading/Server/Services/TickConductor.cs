//using System;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Server.Services
//{
//    public class TickConductor : IDisposable
//    {
//        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
//        private readonly int _tickMilliseconds;
//        private CancellationToken _token;

//        public TickConductor(int tickMilliseconds)
//        {
//            _tickMilliseconds = tickMilliseconds;
//            _token = _cancellationTokenSource.Token;
//            Task.Factory.StartNew(StartCoordinating, _token);
//        }

//        public Barrier TickBarrier { get; } = new Barrier(1);

//        public int TickCount { get; set; }

//        public void Dispose()
//        {
//            _cancellationTokenSource.Cancel();
//        }

//        private void StartCoordinating()
//        {
//            while (true)
//            {
//                if (_token.IsCancellationRequested)
//                {
//                    return;
//                }
//                TickBarrier.
//                TickCount++;
//                Thread.Sleep(_tickMilliseconds + 1);
//            }
//        }
//    }
//}