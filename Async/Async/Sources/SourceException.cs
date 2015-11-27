namespace Sources
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class SourceException : Exception
    {
        public SourceException()
        {
        }

        public SourceException(string message) : base(message)
        {
        }

        public SourceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SourceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}