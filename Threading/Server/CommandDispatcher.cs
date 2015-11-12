using System;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class CommandDispatcher
    {
        private readonly SocketListener _listener;
        private readonly CommandProcessor _processor;

        public CommandDispatcher(SocketListener listener, CommandProcessor processor)
        {
            _listener = listener;
            _processor = processor;
        }

        public void Start(CancellationToken cancellationToken)
        {
            Task.Run(() => CommandLoop(), cancellationToken);
        }

        private void CommandLoop()
        {
            while (true)
            {
                try
                {
                    var command = _listener.GetNextCommand();
                    Task.Run(() => _processor.Process(command));
                }
                catch (Exception e)
                {
                }
            }
        }

        
    }
}