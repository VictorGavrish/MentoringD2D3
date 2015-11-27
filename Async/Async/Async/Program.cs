namespace Async
{
    using System;
    using System.Collections.Generic;

    using Sources;

    internal class Program
    {
        private const string Url = "http://localhost:50505/api/numbers/";

        private static void Main(string[] args)
        {
            Console.WriteLine("Enter the number of local sources:");
            var local = Console.ReadLine();
            int localParsed;
            if (int.TryParse(local, out localParsed))
            {
                var sourceList = new List<ISource>();
                
                for (var i = 0; i < localParsed; i++)
                {
                    sourceList.Add(new LocalSource(i + 1, ErrorReportingType.SkipSource));
                }

                var sourceList2 = new List<ISource>();
                sourceList2.Add(new AggregateSource(localParsed + 1, ErrorReportingType.SkipSource, sourceList));
                for (var i = localParsed + 1; i < localParsed * 2 + 1; i++)
                {
                    sourceList2.Add(new RemoteSource(Url, i + 1, ErrorReportingType.SkipError));
                }

                var calc = new Calculator(sourceList2);
                var result = calc.Calculate().Result;
                Console.WriteLine($"Local result: {result}");
            }

            Console.ReadLine();
        }
    }
}