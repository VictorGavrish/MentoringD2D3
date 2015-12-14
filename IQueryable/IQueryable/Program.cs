namespace IQueryable
{
    using System;
    using System.Linq;
    using System.Xml;

    using IQueryable.Entities;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var doc = new XmlDocument();
            doc.Load(@"Resources\countries.xml");
            var nodes = doc.SelectNodes("//person");
            foreach (XmlElement node in nodes)
            {
                Console.WriteLine(node.InnerXml);
            }

            var people = new CountryData<Country>().SelectMany(c => c.Cities).SelectMany(c => c.People);
            Console.WriteLine(string.Join("\n", people.Select(p => p.ToString())));

            Console.ReadLine();
        }
    }
}