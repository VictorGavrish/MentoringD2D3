using System;

namespace Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Enter command:");
            while (true)
            {
                // two numbers: duration and optional delay
                // two commands: start -n - starts new
                // start --in
                // start --after
                // start id
                // start -s : start a task after creating
                // stop id

                // can cancellation token be 

                // two clients

                var input = Console.ReadLine();
                var response = SocketClient.SimpleSend(input);
                Console.WriteLine(response);
            }
        }
    }
}