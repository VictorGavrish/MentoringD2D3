namespace Sources
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class LocalSource : ISource
    {
        private const int MaxCount = 3000;

        private const int MaxRandom = 1000;

        private const int MinCount = 1000;

        private const int MinRandom = 0;

        private const int SleepBetweenNumbers = 3;

        private static readonly Lazy<Random> LazyRandom = new Lazy<Random>(LazyThreadSafetyMode.ExecutionAndPublication);

        private static int lastId;

        private readonly int id;

        public LocalSource()
        {
            this.id = Interlocked.Increment(ref lastId);
        }

        private static Random Random => LazyRandom.Value;

        public async Task<int[]> GetNextArrayAsync()
        {
            var count = Random.Next(MinCount, MaxCount + 1);
            Console.WriteLine($"Inside Local Source #{this.id}. Generating {count} numbers...");
            var numbers = await this.GetNumbersAsync(count);
            return numbers.ToArray();
        }

        private IEnumerable<int> GetNumbers(int count)
        {
            for (var i = 0; i < count; i++)
            {
                Thread.Sleep(SleepBetweenNumbers);
                yield return Random.Next(MinRandom, MaxRandom + 1);
            }

            Console.WriteLine($"Inside Local Source #{this.id}. Finished generating numbers");
        }

        private async Task<IEnumerable<int>> GetNumbersAsync(int count)
        {
            return await Task.Factory.StartNew(() => this.GetNumbers(count));
        }
    }
}