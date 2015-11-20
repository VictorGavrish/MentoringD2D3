using System;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Services
{
    public class CommandDispatcherService : IDisposable
    {
        private readonly CommandProcessor _processor;
        private readonly SocketServer _server;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        
        public CommandDispatcherService(SocketServer server, CommandProcessor processor)
        {
            _server = server;
            _processor = processor;
            Task.Factory.StartNew(CommandLoop, _cancellationTokenSource.Token);
        }

        private void CommandLoop()
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    var command = _server.GetNextCommand();
                    Task.Factory.StartNew(() => _processor.Process(command));
                }
                catch (Exception e) { }
            }
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}