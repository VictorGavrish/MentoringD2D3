using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;

namespace Client
{
    public class SocketClient
    {
        //private readonly IPAddress _ipAddress;
        //public SocketClient(IPAddress ipAddress)
        //{
        //    _ipAddress = ipAddress;
        //}

        public static int SimpleSend(string input)
        {
            // Data buffer for incoming data.
            var bytes = new byte[1024];

            input = $"{input}<EOF>";

            // Connect to a remote device.
            // Establish the remote endpoint for the socket.
            var remoteEndPoint = new IPEndPoint(IPAddress.Loopback, 6666);

            // Create a TCP/IP  socket.
            var sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Connect the socket to the remote endpoint.
            sender.Connect(remoteEndPoint);

            // Encode the data string into a byte array.
            var msg = Encoding.UTF8.GetBytes(input);

            // Send the data through the socket.
            var bytesSent = sender.Send(msg);

            // Receive the response from the remote device.
            var bytesRec = sender.Receive(bytes);
            var echo = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            
            // Release the socket.
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();

            if (echo != input)
            {
                throw new ConnectionException("Echoed data does not equal sent data");
            }

            return 1;
        }
    }
}