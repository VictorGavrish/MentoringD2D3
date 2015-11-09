using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Enter task:");
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
                int task;
                if (int.TryParse(input, out task))
                {
                    try
                    {
                        SocketClient.SimpleSend(input);
                    }
                    catch (ConnectionException ce)
                    {
                        Console.WriteLine(ce.Message);
                    }
                }
                else
                {
                    Console.WriteLine($"Error parsing {input}");
                }
            }
        }
    }
}
