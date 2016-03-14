namespace XML
{
    using System.Linq;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var querybale = new YellowBookQuery(@"YellowBook.xml");

            var result = querybale.Where(p => p.Age > 0).OrderBy(p => p.FirstName).ToList();
        }
    }
}