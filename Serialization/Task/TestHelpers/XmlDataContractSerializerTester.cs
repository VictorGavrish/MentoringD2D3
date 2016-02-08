namespace Task.TestHelpers
{
    using System.IO;
    using System.Runtime.Serialization;

    public class XmlDataContractSerializerTester<T> : SerializationTester<T, XmlObjectSerializer>
    {
        public XmlDataContractSerializerTester(XmlObjectSerializer serializer, bool showResult = false)
            : base(serializer, showResult)
        {
        }

        internal override T Deserialize(MemoryStream stream)
        {
            return (T)this.Serializer.ReadObject(stream);
        }

        internal override void Serialize(T data, MemoryStream stream)
        {
            this.Serializer.WriteObject(stream, data);
        }
    }
}