namespace XML
{
    using System;

    public class QueryParseException : Exception
    {
        public QueryParseException(string msg)
            : base(msg)
        {
        }
    }
}