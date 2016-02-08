namespace Task.DB
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Xml;

    public class DynamicProxyTypeResolver : DataContractResolver
    {
        public override bool TryResolveType(
            Type type, 
            Type declaredType, 
            DataContractResolver knownTypeResolver, 
            out XmlDictionaryString typeName, 
            out XmlDictionaryString typeNamespace)
        {
            if (type.FullName.Contains("DynamicProxies"))
            {
                var name = type.Name.Split('_').First();
                var nameSpace = "myNamespace";

                typeName = new XmlDictionaryString(XmlDictionary.Empty, name, 0);
                typeNamespace = new XmlDictionaryString(XmlDictionary.Empty, nameSpace, 0);

                return true;
            }

            return knownTypeResolver.TryResolveType(type, declaredType, knownTypeResolver, out typeName, out typeNamespace);
        }

        public override Type ResolveName(
            string typeName, 
            string typeNamespace, 
            Type declaredType, 
            DataContractResolver knownTypeResolver)
        {
            if (typeNamespace == "myNamespace")
            {
                var assembly = Assembly.GetExecutingAssembly();

                var type = assembly.GetType("Task.DB." + typeName);

                return type;
            }

            return knownTypeResolver.ResolveName(typeName, typeNamespace, declaredType, knownTypeResolver);
        }
    }
}