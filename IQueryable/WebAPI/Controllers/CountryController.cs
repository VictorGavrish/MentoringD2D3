namespace WebAPI.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    using CommonEntities.Entities;

    using Serialize.Linq.Nodes;

    using WebAPI.Repositories;

    [RoutePrefix("api/country")]
    public class CountryController : ApiController
    {
        public IEnumerable<Country> Get()
        {
            return CountryRepository.GetAll();
        }

        [Route("{id}")]
        public Country Get(int id)
        {
            return CountryRepository.GetAll().Single(p => p.Id == id);
        }

        [HttpPost]
        public object Post([FromBody] ExpressionNode query)
        {
            var expression = query.ToExpression();
            return CountryRepository.GetAll().AsQueryable().Provider.Execute(expression);
        }
    }
}