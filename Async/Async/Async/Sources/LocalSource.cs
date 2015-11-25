using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Async.Sources
{
    public class LocalSource : ISource
    {
        public LocalSource()
        {
            _id = Interlocked.Increment(ref _lastId);
        }

        private static int _lastId;
        private readonly int _id;
        private const int MinRandom = 0;
        private const int MaxRandom = 1000;
        private const int MinCount = 1000;
        private const int MaxCount = 3000;
        private static readonly Lazy<Random> LazyRandom = new Lazy<Random>(LazyThreadSafetyMode.ExecutionAndPublication);
        private const int SleepBetweenNumbers = 3;
        private static Random Random => LazyRandom.Value;

        public async Task<int[]> GetNextArrayAsync()
        {
            var count = Random.Next(MinCount, MaxCount + 1);
            Console.WriteLine($"Inside Local Source #{_id}. Generating {count} numbers...");
            var numbers = await GetNumbersAsync(count);
            return numbers.ToArray();
        }

        private async Task<IEnumerable<int>> GetNumbersAsync(int count)
        {
            return await Task.Factory.StartNew(() => GetNumbers(count));
        }

        private IEnumerable<int> GetNumbers(int count)
        {
            for (var i = 0; i < count; i++)
            {
                Thread.Sleep(SleepBetweenNumbers);
                yield return Random.Next(MinRandom, MaxRandom + 1);
            }
            Console.WriteLine($"Inside Local Source #{_id}. Finished generating numbers");
        }
    }
}