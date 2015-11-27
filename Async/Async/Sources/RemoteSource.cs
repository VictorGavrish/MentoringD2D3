namespace Sources
{
    using System.Threading.Tasks;

    using Sources.Tools;

    public class RemoteSource : ISource
    {
        private readonly string url;

        public RemoteSource(string url)
        {
            this.url = url;
        }

        public async Task<int[]> GetNextArrayAsync()
        {
            return await JsonDownloader.DownloadSerializedJsonDataAsync<int[]>(this.url);
        }
    }
}