namespace Sources
{
    using System.Threading.Tasks;

    public interface ISource
    {
        Task<int[]> GetNextArrayAsync();
    }
}