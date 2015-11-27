namespace Async
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Threading.Tasks;

    using Sources;

    public class Calculator
    {
        private readonly IEnumerable<ISource> sources;

        public Calculator(IEnumerable<ISource> sources)
        {
            this.sources = sources;
        }

        public async Task<BigInteger> Calculate()
        {
            Console.WriteLine("Inside Caluclator. Requesting data from sources...");
            var results = this.RequestResults();
            Console.WriteLine("Inside Calculator. Starting calculation of per-source results...");
            var sourceResults = await Task.Run(() => CalculatePerSource(results));
            Console.WriteLine("Inside Calculator. Starting calculation of final result...");
            var finalResult = await Task.Run(() => CalculateFinalResult(sourceResults));
            return finalResult;
        }

        private static BigInteger CalculateFinalResult(IEnumerable<long> sourceResults)
        {
            var results = sourceResults.ToArray();
            if (!results.Any())
            {
                return BigInteger.Zero;
            }
            var result = results.Select(l => new BigInteger(l)).Aggregate((a, b) => a * b);
            Console.WriteLine("Inside Calculator. Finished cacluating the final result");
            return result;
        }

        private static IEnumerable<long> CalculatePerSource(IEnumerable<SourceResult> results)
        {
            var count = 0;
            foreach (var result in results)
            {
                var sign = result.Id % 2 == 0 ? -1 : 1;
                Console.WriteLine(
                    $"Inside Calculator. Starting calculation for response #{++count} from source #{result.Id}");
                var perSourceCalc = result.Values.Where(i => i.HasValue).Sum(i => (long)i.Value) * sign;
                Console.WriteLine($"Inside Calculator. Calculation for response #{count} complete: {perSourceCalc}");
                yield return perSourceCalc;
            }
        }

        private IEnumerable<SourceResult> RequestResults()
        {
            var tasks = this.sources.Select(source => source.GetNextArrayAsync()).ToList();
            Console.WriteLine("Inside Calculator. Requests sent. Wating for results");

            var count = 0;
            while (tasks.Any())
            {
                var tasksSnapshot = tasks.ToArray();
                var completedTask = Task.WhenAny(tasksSnapshot).Result;
                Console.WriteLine($"Inside Calculator. Received response #{++count}");
                tasks.Remove(completedTask);
                if (completedTask.Result.Failure)
                {
                    Console.WriteLine($"Failed to receive data: {completedTask.Result.Error}");
                    continue;
                }

                yield return completedTask.Result.Value;
            }
        }
    }
}