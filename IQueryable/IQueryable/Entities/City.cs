namespace IQueryable.Entities
{
    using System.Collections.Generic;

    public class City
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IList<Person> People { get; set; }
    }
}