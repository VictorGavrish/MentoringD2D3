namespace WebAPI.Repositories
{
    using System.Collections.Generic;

    using IQueryable.Entities;

    public class CountryRepository
    {
        public static IEnumerable<Country> GetAll()
        {
            return new[]
                {
                    new Country
                        {
                            Id = 1, 
                            Name = "Belarus", 
                            Cities =
                                new List<City>
                                    {
                                        new City
                                            {
                                                Id = 1, 
                                                Name = "Gomel", 
                                                People =
                                                    new List<Person>
                                                        {
                                                            new Person { Age = 27, Id = 1, Name = "Victor" }, 
                                                            new Person { Age = 17, Id = 2, Name = "Alexander" }, 
                                                            new Person { Age = 11, Id = 3, Name = "Dmitriy" }
                                                        }
                                            }
                                    }
                        }
                };
        }
    }
}