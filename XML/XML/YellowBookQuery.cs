namespace XML
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class YellowBookQuery : IOrderedQueryable<Person>
    {
        public YellowBookQuery(string filePath)
        {
            this.Provider = new YellowBookQueryProvider(filePath);
            this.Expression = Expression.Constant(this);
        }

        internal YellowBookQuery(IQueryProvider provider, Expression expression)
        {
            this.Provider = provider;
            this.Expression = expression;
        }

        public IEnumerator<Person> GetEnumerator()
        {
            return this.Provider.Execute<IEnumerable<Person>>(this.Expression).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public Expression Expression { get; }

        public Type ElementType => typeof(Person);

        public IQueryProvider Provider { get; }
    }
}