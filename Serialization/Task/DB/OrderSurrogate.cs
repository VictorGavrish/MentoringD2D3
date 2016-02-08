namespace Task.DB
{
    using System;
    using System.CodeDom;
    using System.Collections.ObjectModel;
    using System.Reflection;
    using System.Runtime.Serialization;

    public class OrderSurrogate : IDataContractSurrogate
    {
        public object GetCustomDataToExport(Type clrType, Type dataContractType)
        {
            throw new NotImplementedException();
        }

        public object GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType)
        {
            throw new NotImplementedException();
        }

        public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData)
        {
            throw new NotImplementedException();
        }

        public CodeTypeDeclaration ProcessImportedType(CodeTypeDeclaration typeDeclaration, CodeCompileUnit compileUnit)
        {
            throw new NotImplementedException();
        }

        public void GetKnownCustomDataTypes(Collection<Type> customDataTypes)
        {
            throw new NotImplementedException();
        }

        public Type GetDataContractType(Type type)
        {
            return typeof(Order).IsAssignableFrom(type) ? typeof(OrderSurrogated) : type;
        }

        public object GetObjectToSerialize(object obj, Type targetType)
        {
            var order = obj as Order;

            if (order == null)
            {
                return obj;
            }

            var orderSurrogated = new OrderSurrogated
            {
                CustomerID = order.CustomerID, 
                EmployeeID = order.EmployeeID, 
                Freight = order.Freight, 
                OrderDate = order.OrderDate, 
                OrderID = order.OrderID, 
                RequiredDate = order.RequiredDate, 
                ShipAddress = order.ShipAddress, 
                ShipCity = order.ShipCity, 
                ShipCountry = order.ShipCountry, 
                ShipName = order.ShipName, 
                ShipPostalCode = order.ShipPostalCode, 
                ShipRegion = order.ShipRegion, 
                ShipVia = order.ShipVia, 
                ShippedDate = order.ShippedDate
            };

            return orderSurrogated;
        }

        public object GetDeserializedObject(object obj, Type targetType)
        {
            var orderSurrogated = obj as OrderSurrogated;

            if (orderSurrogated == null)
            {
                return obj;
            }

            var order = new Order
            {
                CustomerID = orderSurrogated.CustomerID, 
                EmployeeID = orderSurrogated.EmployeeID, 
                Freight = orderSurrogated.Freight, 
                OrderDate = orderSurrogated.OrderDate, 
                OrderID = orderSurrogated.OrderID, 
                RequiredDate = orderSurrogated.RequiredDate, 
                ShipAddress = orderSurrogated.ShipAddress, 
                ShipCity = orderSurrogated.ShipCity, 
                ShipCountry = orderSurrogated.ShipCountry, 
                ShipName = orderSurrogated.ShipName, 
                ShipPostalCode = orderSurrogated.ShipPostalCode, 
                ShipRegion = orderSurrogated.ShipRegion, 
                ShipVia = orderSurrogated.ShipVia, 
                ShippedDate = orderSurrogated.ShippedDate
            };

            return order;
        }
    }
}