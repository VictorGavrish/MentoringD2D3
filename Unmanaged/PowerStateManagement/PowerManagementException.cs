namespace PowerStateManagement
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class PowerManagementException : Exception
    {
        public PowerManagementException()
        {
        }

        public PowerManagementException(string message) : base(message)
        {
        }

        public PowerManagementException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PowerManagementException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}