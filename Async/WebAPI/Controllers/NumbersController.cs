using System.Threading.Tasks;
using System.Web.Http;
using Sources;

namespace WebAPI.Controllers
{
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