using System;
using System.Collections.Generic;
using Async.Sources;

namespace Async
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Enter the number of sources:");
            var input = Console.ReadLine();
            int number;
            if (int.TryParse(input, out number))
            {
                var sourceList = new List<LocalSource>();
                for (int i = 0; i < number; i++)
                {
                    sourceList.Add(new LocalSource());
                }
                var calc = new Calculator(sourceList);
                var result = calc.Calculate().Result;
                Console.WriteLine(result);
            }
            Console.ReadLine();
        }
    }
}