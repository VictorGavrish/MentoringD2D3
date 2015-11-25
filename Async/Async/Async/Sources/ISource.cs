using System.Threading.Tasks;

namespace Async.Sources
{
    public interface ISource
    {
        Task<int[]> GetNextArrayAsync();
    }
}