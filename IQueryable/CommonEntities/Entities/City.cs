namespace CommonEntities.Entities
{
    using System.Collections;

    public class City
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IList People { get; set; }
    }
}