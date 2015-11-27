namespace Sources
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Sources.Tools;

    public class RemoteSource : ISource
    {
        private readonly int id;

        private readonly string url;

        public RemoteSource(string url, int id, ErrorReportingType errorReportingType)
        {
            this.url = url;
            this.id = id;
            this.ErrorReportingType = errorReportingType;
        }

        public ErrorReportingType ErrorReportingType { get; }

        public async Task<Result<SourceResult>> GetNextArrayAsync()
        {
            var values = await JsonDownloader.DownloadSerializedJsonDataAsync<int?[]>(this.url);
            
            switch (this.ErrorReportingType)
            {
                case ErrorReportingType.SkipError:
                    values = values.Where(value => value.HasValue).ToArray();
                    return Result.Ok(new SourceResult(this.id, values));
                case ErrorReportingType.NullError:
                    return Result.Ok(new SourceResult(this.id, values));
                case ErrorReportingType.SkipSource:
                    return Result.Fail<SourceResult>("Failed to get remote result");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}