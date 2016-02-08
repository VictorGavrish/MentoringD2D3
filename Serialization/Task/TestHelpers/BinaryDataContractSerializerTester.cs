namespace Task.TestHelpers
{
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    public class BinaryDataContractSerializerTester<T> : SerializationTester<T, BinaryFormatter>
    {
        public BinaryDataContractSerializerTester(BinaryFormatter serializer, bool showResult = false) 
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