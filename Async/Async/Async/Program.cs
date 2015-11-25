using System;
using System.Collections.Generic;
using Sources;

namespace Async
{
    internal class Program
    {
        private const string Url = "http://localhost:50328/api/numbers/";

        private static void Main(string[] args)
        {
            Console.WriteLine("Enter the number of local sources:");
            var local = Console.ReadLine();
            int localParsed;
            if (int.TryParse(local, out localParsed))
            {
                var sourceList = new List<LocalSource>();
                for (var i = 0; i < localParsed; i++)
                {
                    sourceList.Add(new LocalSource());
                }
                var calc = new Calculator(sourceList);
                var result = calc.Calculate().Result;
                Console.WriteLine($"Local result: {result}");
            }

            //Console.WriteLine("Enter the number of remote sources:");
            //var remote = Console.ReadLine();
            //int remoteParsed;
            //if (int.TryParse(remote, out remoteParsed))
            //{
            //    var sourceList = new List<RemoteSource>();
            //    for (var i = 0; i < remoteParsed; i++)
            //    {
            //        sourceList.Add(new RemoteSource(Url));
            //    }
            //    var calc = new Calculator(sourceList);
            //    var result = calc.Calculate().Result;
            //    Console.WriteLine($"Remote result: {result}");
            //}
            //Console.ReadLine();
        }
    }
}