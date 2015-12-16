namespace AppDomains
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Security.Policy;

    public class PluginWrapper
    {
        private readonly string assemblyName;

        private readonly AppDomain pluginDomain;

        private readonly string typeName;

        public PluginWrapper(Type pluginType)
        {
            Contract.Requires(pluginType != null);
            Contract.Requires(typeof(IPlugin).IsAssignableFrom(pluginType));

            ////Contract.Requires(typeof(MarshalByRefObject).IsAssignableFrom(pluginType));
            this.assemblyName = pluginType.Assembly.GetName().Name;
            this.typeName = pluginType.FullName;
            var domainSetup = new AppDomainSetup { ApplicationBase = AppDomain.CurrentDomain.BaseDirectory };
            this.pluginDomain = AppDomain.CreateDomain(this.typeName, new Evidence(), domainSetup);
        }

        public IPlugin Plugin { get; private set; }

        public void Start()
        {
            Contract.Requires(this.Plugin == null);
            Contract.Ensures(this.Plugin != null);

            this.Plugin = (IPlugin)this.pluginDomain.CreateInstanceAndUnwrap(this.assemblyName, this.typeName);
        }

        public bool TryUnload()
        {
            Contract.Requires(this.Plugin != null);

            try
            {
                AppDomain.Unload(this.pluginDomain);
            }
            catch (CannotUnloadAppDomainException ex)
            {
                return false;
            }

            this.Plugin = null;
            return true;
        }
    }
}