namespace IQueryable.Entities
{
    using System.Collections.Generic;

    public class Country
    {
        public IList<City> Cities { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }
    }
}