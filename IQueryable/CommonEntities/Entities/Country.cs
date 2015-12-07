namespace CommonEntities.Entities
{
    using System.Collections;

    public class Country
    {
        public IList Cities { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }
    }
}