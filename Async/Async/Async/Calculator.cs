using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Async.Sources;

namespace Async
{
    public class Calculator
    {
        private readonly IEnumerable<ISource> _sources;

        public Calculator(IEnumerable<ISource> sources)
        {
            _sources = sources;
        }

        public async Task<BigInteger> Calculate()
        {
            Console.WriteLine("Inside Caluclator. Requesting data from sources...");
            var results = RequestResults();
            Console.WriteLine("Inside Calculator. Starting calculation of per-source results...");
            var sourceResults = await Task.Factory.StartNew(() => CalculatePerSource(results));
            Console.WriteLine("Inside Calculator. Starting calculation of final result...");
            var finalResult = await Task.Factory.StartNew(() => CalculateFinalResult(sourceResults));
            return finalResult;
        }

        private IEnumerable<int[]> RequestResults()
        {
            var tasks = _sources.Select(source => source.GetNextArrayAsync()).ToList();
            Console.WriteLine("Inside Calculator. Requests sent. Wating for results");
            var count = 0;
            while (tasks.Any())
            {
                var tasksSnapshot = tasks.ToArray();
                var completedTask = Task.WhenAny(tasksSnapshot).Result;
                Console.WriteLine($"Inside Calculator. Received response #{++count}");
                tasks.Remove(completedTask);
                yield return completedTask.Result;
            }
        }

        private static IEnumerable<long> CalculatePerSource(IEnumerable<int[]> results)
        {
            var sign = 1;
            var count = 0;
            foreach (var result in results)
            {
                Console.WriteLine($"Inside Calculator. Starting calculation for response #{++count}");
                var perSourceCalc = result.Sum(i => (long) i) * sign;
                Console.WriteLine($"Inside Calculator. Calculation for response #{count} complete: {perSourceCalc}");
                yield return perSourceCalc;
                sign = -sign;
            }
        }

        private static BigInteger CalculateFinalResult(IEnumerable<long> sourceResults)
        {
            var result = sourceResults.Select(l => new BigInteger(l)).Aggregate((a, b) => a * b);
            Console.WriteLine("Inside Calculator. Finished cacluating the final result");
            return result;
        }
    }
}