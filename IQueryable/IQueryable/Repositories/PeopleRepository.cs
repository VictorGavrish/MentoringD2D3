namespace WebAPI.Repositories
{
    using System.Collections.Generic;

    using IQueryable.Entities;

    public class PeopleRepository
    {
        public static IEnumerable<Person> GetAll()
        {
            return new[]
                       {
                           new Person { Age = 27, Id = 1, Name = "Victor" },
                           new Person { Age = 17, Id = 2, Name = "Alexander" },
                           new Person { Age = 11, Id = 3, Name = "Dmitriy" }
                       };
        }
    }
}