namespace IQueryable
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using CommonEntities.Entities;

    public class CountryData : IOrderedQueryable<Country>
    {
        public CountryData()
        {
            this.Provider = new CountryQueryProvider();
            this.Expression = Expression.Constant(this);
        }

        public CountryData(CountryQueryProvider provider, Expression expression)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (!typeof(IQueryable<Country>).IsAssignableFrom(expression.Type))
            {
                throw new ArgumentOutOfRangeException(nameof(expression));
            }

            this.Provider = provider;
            this.Expression = expression;
        }

        public Type ElementType => typeof(Person);

        public Expression Expression { get; }

        public IQueryProvider Provider { get; }

        public IEnumerator<Country> GetEnumerator()
        {
            return this.Provider.Execute<IEnumerable<Country>>(this.Expression).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Provider.Execute<IEnumerable>(this.Expression).GetEnumerator();
        }
    }
}