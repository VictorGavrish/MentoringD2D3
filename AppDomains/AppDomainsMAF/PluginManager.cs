namespace AppDomainsMAF
{
    using System;
    using System.AddIn.Hosting;
    using System.Linq;

    using HostView;

    public class PluginManager
    {
        private readonly AddInToken addIn;

        private AppDomain domain;

        public IPlugin Plugin { get; private set; }

        public PluginManager(string rootFolder, string addInName)
        {
            var warnings = AddInStore.Update(rootFolder);
            foreach (var warning in warnings)
            {
                Console.WriteLine(warning);
            }
            var addIns = AddInStore.FindAddIns(this.SupportedAddInType, rootFolder);
            this.addIn = addIns.Single(ai => ai.Name == addInName);
        }

        public Type SupportedAddInType { get; } = typeof(IPlugin);

        public void Load()
        {
            this.domain = AppDomain.CreateDomain(this.addIn.Name);
            this.Plugin = this.addIn.Activate<IPlugin>(AddInSecurityLevel.FullTrust, this.addIn.Name);
        }

        public bool TryUnload()
        {
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