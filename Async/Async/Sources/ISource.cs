namespace Sources
{
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;

    public interface ISource
    {
        ErrorReportingType ErrorReportingType { get; }

        Task<Result<SourceResult>> GetNextArrayAsync();
    }
}