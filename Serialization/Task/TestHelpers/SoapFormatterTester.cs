namespace Task.TestHelpers
{
    using System.IO;
    using System.Runtime.Serialization.Formatters.Soap;

    public class SoapFormatterTester<T> : SerializationTester<T, SoapFormatter>
    {
        public SoapFormatterTester(SoapFormatter serializer, bool showResult = false)
            : base(serializer, showResult)
        {
        }

        internal override T Deserialize(MemoryStream stream)
        {
            return (T)this.Serializer.Deserialize(stream);
        }

        internal override void Serialize(T data, MemoryStream stream)
        {
            this.Serializer.Serialize(stream, data);
        }
    }
}