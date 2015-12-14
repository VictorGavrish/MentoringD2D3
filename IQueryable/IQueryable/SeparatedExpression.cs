namespace IQueryable
{
    using System.Linq.Expressions;

    public class SeparatedExpression
    {
        public SeparatedExpression(Expression supportedExpresssion, Expression unsupportedExpression)
        {
            this.SupportedExpresssion = supportedExpresssion;
            this.UnsupportedExpression = unsupportedExpression;
        }

        public Expression SupportedExpresssion { get; }

        public Expression UnsupportedExpression { get; }
    }
}