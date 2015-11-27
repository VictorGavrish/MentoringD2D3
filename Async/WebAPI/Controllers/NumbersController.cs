namespace WebAPI.Controllers
{
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Sources;

    public class NumbersController : ApiController
    {
        private static int lastId;

        // GET api/numbers
        public async Task<int?[]> Get()
        {
            var source = new LocalSource(Interlocked.Increment(ref lastId), ErrorReportingType.NullError);
            var result = await source.GetNextArrayAsync();
            return result.Value.Values;
        }
    }
}