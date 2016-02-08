namespace Task
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Task.DB;
    using Task.TestHelpers;

    [TestClass]
    public class SerializationSolutions
    {
        private Northwind context;

        [TestInitialize]
        public void Initialize()
        {
            this.context = new Northwind();
        }

        [TestMethod]
        public void SerializationCallbacks()
        {
            var dataContractResolver = new DeclaredTypeResolver();
            var dataContractSerializerSettings = new DataContractSerializerSettings
            {
                DataContractResolver = dataContractResolver
            };
            var dataContractSerializer = new DataContractSerializer(typeof(Category), dataContractSerializerSettings);

            var tester = new XmlDataContractSerializerTester<Category>(dataContractSerializer, true);

            var category = this.context.Categories.First();

            var categoryBack = tester.SerializeAndDeserialize(category);
        }

        [TestMethod]
        public void ISerializable()
        {
            this.context.Configuration.ProxyCreationEnabled = false;

            var tester = new XmlDataContractSerializerTester<IEnumerable<Product>>(
                new NetDataContractSerializer(), 
                true);
            var products = this.context.Products.ToList();

            tester.SerializeAndDeserialize(products);
        }

        [TestMethod]
        public void ISerializationSurrogate()
        {
            this.context.Configuration.ProxyCreationEnabled = false;

            var tester = new XmlDataContractSerializerTester<IEnumerable<Order_Detail>>(
                new NetDataContractSerializer(), 
                true);
            var orderDetails = this.context.Order_Details.ToList();

            tester.SerializeAndDeserialize(orderDetails);
        }

        [TestMethod]
        public void IDataContractSurrogate()
        {
            this.context.Configuration.ProxyCreationEnabled = true;
            this.context.Configuration.LazyLoadingEnabled = true;

            var tester =
                new XmlDataContractSerializerTester<IEnumerable<Order>>(
                    new DataContractSerializer(typeof(IEnumerable<Order>)), 
                    true);
            var orders = this.context.Orders.ToList();

            tester.SerializeAndDeserialize(orders);
        }
    }
}