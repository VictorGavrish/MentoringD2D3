namespace XML
{
    using System.Linq;
    using System.Linq.Expressions;

    public class YellowBookQueryProvider : IQueryProvider
    {
        private readonly string filePath;

        public YellowBookQueryProvider(string filePath)
        {
            this.filePath = filePath;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new YellowBookQuery(this, expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return (IQueryable<TElement>)new YellowBookQuery(this, expression);
        }

        public object Execute(Expression expression)
        {
            return this.Execute<Person>(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            var visitor = new YellowBookExpressionVisitor(this.filePath);

            var queryResult = visitor.Query(expression);

            return (TResult)queryResult;
        }
    }
}