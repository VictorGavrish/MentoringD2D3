namespace Task
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Task.DB;
    using Task.TestHelpers;

    [TestClass]
    public class SerializationSolutions
    {
        private Northwind context;

        private DataContractResolver resolver;

        [TestInitialize]
        public void Initialize()
        {
            this.context = new Northwind();
            this.resolver = new DynamicProxyTypeResolver();
        }

        [TestMethod]
        public void SerializationCallbacks()
        {
            var dataContractSerializerSettings = new DataContractSerializerSettings
            {
                DataContractResolver = this.resolver
            };
            var dataContractSerializer = new DataContractSerializer(typeof(Category), dataContractSerializerSettings);

            var tester = new XmlDataContractSerializerTester<Category>(dataContractSerializer, true);

            var category = this.context.Categories.First();

            var categoryBack = tester.SerializeAndDeserialize(category);
        }

        [TestMethod]
        public void ISerializable()
        {
            var dataContractSerializerSettings = new DataContractSerializerSettings
            {
                DataContractResolver = this.resolver
            };

            var dataContractSerializer = new DataContractSerializer(
                typeof(IEnumerable<Product>), 
                dataContractSerializerSettings);

            var products = this.context.Products.ToList();

            var tester = new XmlDataContractSerializerTester<IEnumerable<Product>>(dataContractSerializer, true);

            var productsBack = tester.SerializeAndDeserialize(products);
        }

        [TestMethod]
        public void ISerializationSurrogate()
        {
            var orderDetails = this.context.OrderDetails.Include(x => x.Product).ToList();

            var selector = new SurrogateSelector();

            selector.AddSurrogate(
                typeof(OrderDetail), 
                new StreamingContext(StreamingContextStates.Persistence, null), 
                new OrderDetailsSerializationSurrogate());

            var binaryFormatter = new BinaryFormatter { SurrogateSelector = selector };

            var tester = new BinaryDataContractSerializerTester<IEnumerable<OrderDetail>>(binaryFormatter, true);

            var orderDetailsBack = tester.SerializeAndDeserialize(orderDetails);
        }

        [TestMethod]
        public void IDataContractSurrogate()
        {
            var orders = this.context.Orders.ToArray();
            var knownTypes = new List<Type> { typeof(Order) };

            IDataContractSurrogate surrogate = new OrderSurrogate();
            var tester =
                new XmlDataContractSerializerTester<IEnumerable<Order>>(
                    new DataContractSerializer(
                        typeof(IEnumerable<Order>), 
                        knownTypes, 
                        int.MaxValue, 
                        false, 
                        true, 
                        surrogate,
                        this.resolver), 
                    true);

            var ordersBack = tester.SerializeAndDeserialize(orders);
        }
    }
}