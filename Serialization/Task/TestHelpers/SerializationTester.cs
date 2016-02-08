namespace Task.TestHelpers
{
    using System;
    using System.IO;

    public abstract class SerializationTester<TData, TSerializer>
    {
        private readonly bool showResult;

        protected TSerializer Serializer { get; set; }

        public SerializationTester(TSerializer serializer, bool showResult = false)
        {
            this.Serializer = serializer;
            this.showResult = showResult;
        }

        public TData SerializeAndDeserialize(TData data)
        {
            var stream = new MemoryStream();

            Console.WriteLine("Start serialization");
            this.Serialize(data, stream);
            Console.WriteLine("Serialization finished");

            if (this.showResult)
            {
                var r = Console.OutputEncoding.GetString(stream.GetBuffer(), 0, (int)stream.Length);
                Console.WriteLine(r);
            }

            stream.Seek(0, SeekOrigin.Begin);
            Console.WriteLine("Start deserialization");
            var result = this.Deserialize(stream);
            Console.WriteLine("Deserialization finished");

            return result;
        }

        internal abstract TData Deserialize(MemoryStream stream);

        internal abstract void Serialize(TData data, MemoryStream stream);
    }
}