namespace XML
{
    using System.Collections.Generic;

    public class City
    {
        public string Name { get; set; }

        public IEnumerable<Person> People { get; set; }
    }
}