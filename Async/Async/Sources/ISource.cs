using System.Threading.Tasks;

namespace Sources
{
    public interface ISource
    {
        Task<int[]> GetNextArrayAsync();
    }
}