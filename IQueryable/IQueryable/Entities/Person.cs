namespace IQueryable.Entities
{
    public class Person
    {
        public int Age { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return $"{this.Name}, {this.Age}";
        }
    }
}