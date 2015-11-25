using System.Threading.Tasks;
using Sources.Tools;

namespace Sources
{
    public class RemoteSource : ISource
    {
        private readonly string _url;

        public RemoteSource(string url)
        {
            _url = url;
        }

        public async Task<int[]> GetNextArrayAsync()
        {
            return await JsonDownloader.DownloadSerializedJSONDataAsync<int[]>(_url);
        }
    }
}