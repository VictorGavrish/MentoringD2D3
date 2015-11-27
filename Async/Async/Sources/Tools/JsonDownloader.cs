namespace Sources.Tools
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    public static class JsonDownloader
    {
        public static async Task<T> DownloadSerializedJsonDataAsync<T>(string url)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var jsonData = await httpClient.GetStringAsync(url);
                    return !string.IsNullOrEmpty(jsonData) ? JsonConvert.DeserializeObject<T>(jsonData) : default(T);
                }
                catch (Exception)
                {
                    return default(T);
                }
            }
        }
    }
}