namespace AppDomainsMAF
{
    using System;
    using System.AddIn.Hosting;
    using System.Collections.ObjectModel;
    using System.Linq;

    using HostView;

    public class PluginManager
    {
        private readonly Collection<AddInToken> addIns;

        private AppDomain domain;

        public IPlugin Plugin { get; private set; }

        public PluginManager(string rootFolder)
        {
            var warnings = AddInStore.Update(rootFolder);
            foreach (var warning in warnings)
            {
                Console.WriteLine(warning);
            }
            this.addIns = AddInStore.FindAddIns(this.SupportedAddInType, rootFolder);
        }

        public Type SupportedAddInType { get; } = typeof(IPlugin);

        /// <exception cref="PluginException">Plugin already loaded</exception>
        public void Load(string addInName)
        {
            if (this.Plugin != null)
            {
                throw new PluginException("Plugin already loaded");
            }

            var addIn = this.addIns.Single(ai => ai.Name == addInName);
            this.domain = AppDomain.CreateDomain(addIn.Name);
            this.Plugin = addIn.Activate<IPlugin>(AddInSecurityLevel.FullTrust, addIn.Name);
        }

        /// <exception cref="PluginException">No plugin loaded</exception>
        public bool TryUnload()
        {
            if (this.Plugin == null)
            {
                throw new PluginException("No plugin loaded");
            }

            try
            {
                AppDomain.Unload(this.domain);
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