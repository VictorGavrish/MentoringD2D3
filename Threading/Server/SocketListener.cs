using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class SocketListener
    {
        private readonly Socket _listener;

        public SocketListener(IPAddress ipAddress, int port)
        {
            var endPoint = new IPEndPoint(ipAddress, port);
            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listener.Bind(endPoint);
        }

        public Command GetNextTask()
        {
            _listener.Listen(100);

            var handler = _listener.Accept();
            string data = null;

            while (true)
            {
                var bytes = new byte[1024];
                var bytesRec = handler.Receive(bytes);
                data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                if (data.IndexOf("<EOF>", StringComparison.Ordinal) > -1)
                {
                    break;
                }
            }

            var cutoff = data.IndexOf("<EOF>", StringComparison.Ordinal);
            data = data.Substring(0, cutoff);

            try
            {
                var command = Command.Parse(data);
                var msg = Encoding.UTF8.GetBytes("OK");
                handler.Send(msg);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
                return command;
            }
            catch (CommandParseException cpe)
            {
                var msg = Encoding.UTF8.GetBytes(cpe.Message);
                handler.Send(msg);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
                throw;
            }
        }
    }
}