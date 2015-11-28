namespace Sources
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
}