namespace IQueryable
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var data = new CountryData();
            var result = data.Single(c => c.Name == "Belarus");

            Console.ReadLine();
        }
    }
}