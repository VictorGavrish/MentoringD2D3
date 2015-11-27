namespace Sources
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class AggregateSource : ISource
    {
        private readonly int id;

        private readonly IEnumerable<ISource> sources;

        public AggregateSource(int id, ErrorReportingType type, IEnumerable<ISource> sources)
        {
            this.id = id;
            this.sources = sources;
            this.ErrorReportingType = type;
        }

        public ErrorReportingType ErrorReportingType { get; }

        public async Task<Result<SourceResult>> GetNextArrayAsync()
        {
            var resultList = new List<int?[]>();
            foreach (var source in this.sources)
            {
                var result = await source.GetNextArrayAsync();
                if (result.Success)
                {
                    resultList.Add(result.Value.Values);
                }
            }

            if (!resultList.Any())
            {
                return Result.Fail<SourceResult>("");
            }

            var sign = 1;
            var values = new List<int?>();
            foreach (var value in this.WeaveCollections(resultList))
            {
                if (!value.HasValue)
                {
                    switch (this.ErrorReportingType)
                    {
                        case ErrorReportingType.SkipError:
                            continue;
                        case ErrorReportingType.NullError:
                            break;
                        case ErrorReportingType.SkipSource:
                            return Result.Fail<SourceResult>($"Aggregate source #{this.id} encountered corrupted number");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                values.Add(value);
                sign = -sign;
            }

            var sourceResult = new SourceResult(this.id, values.ToArray());
            return Result.Ok(sourceResult);
        }

        private IEnumerable<T> WeaveCollections<T>(IEnumerable<IEnumerable<T>> enumerable)
        {
            var enumerators = enumerable.Select(e => e.GetEnumerator()).ToList();

            while (true)
            {
                var exit = true;
                foreach (var enumerator in enumerators.Where(enumerator => enumerator.MoveNext()))
                {
                    exit = false;
                    yield return enumerator.Current;
                }

                if (exit)
                {
                    break;
                }
            }
        }
    }

    public class LocalSource : ISource
    {
        private const double ExceptionChance = 0.0001;

        private const int MaxCount = 3000;

        private const int MaxRandom = 1000;

        private const int MinCount = 1000;

        private const int MinRandom = 0;

        private const int SleepBetweenNumbers = 3;

        private static readonly Lazy<Random> LazyRandom = new Lazy<Random>(LazyThreadSafetyMode.ExecutionAndPublication);

        private readonly int id;

        public LocalSource(int id, ErrorReportingType errorReportingType)
        {
            this.id = id;
            this.ErrorReportingType = errorReportingType;
        }

        public ErrorReportingType ErrorReportingType { get; }

        private static Random Random => LazyRandom.Value;

        public async Task<Result<SourceResult>> GetNextArrayAsync()
        {
            var count = Random.Next(MinCount, MaxCount + 1);
            Console.WriteLine($"Inside Local Source #{this.id}. Generating {count} numbers...");
            try
            {
                var numbers = await this.GetNumbersAsync(count);
                var sourceResult = new SourceResult(this.id, numbers.ToArray());
                return Result.Ok(sourceResult);
            }
            catch (SourceException)
            {
                return Result.Fail<SourceResult>($"Failed to get the numbers in source {this.id}");
            }
        }

        private IEnumerable<int?> GetNumbers(int count)
        {
            for (var i = 0; i < count; i++)
            {
                Thread.Sleep(SleepBetweenNumbers);
                if (Random.NextDouble() < ExceptionChance)
                {
                    switch (this.ErrorReportingType)
                    {
                        case ErrorReportingType.SkipError:
                            continue;
                        case ErrorReportingType.NullError:
                            yield return null;
                            break;
                        case ErrorReportingType.SkipSource:
                            throw new SourceException();
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                yield return Random.Next(MinRandom, MaxRandom + 1);
            }

            Console.WriteLine($"Inside Local Source #{this.id}. Finished generating numbers");
        }

        private async Task<IEnumerable<int?>> GetNumbersAsync(int count)
        {
            return await Task.Factory.StartNew(() => this.GetNumbers(count));
        }
    }
}