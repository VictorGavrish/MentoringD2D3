namespace WebAPI.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;

    using Sources;

    public class NumbersController : ApiController
    {
        // GET api/numbers
        public async Task<int[]> Get()
        {
            var source = new LocalSource();
            return await source.GetNextArrayAsync();
        }
    }
}