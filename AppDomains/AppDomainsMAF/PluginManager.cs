﻿namespace AppDomainsMAF
{
    using System;
    using System.AddIn.Hosting;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Security.Policy;

    using HostView;

    public class PluginManager : IDisposable
    {
        private readonly IList<AddInToken> addIns;

        private readonly Dictionary<string, AppDomain> domains = new Dictionary<string, AppDomain>();

        private readonly DirectoryInfo rootDirectory;

        public PluginManager(string rootFolder)
        {
            AddInStore.Update(rootFolder);
            this.addIns = AddInStore.FindAddIns(this.SupportedAddInType, rootFolder).ToList();
            this.rootDirectory = new DirectoryInfo(rootFolder);
        }

        public Type SupportedAddInType { get; } = typeof(IPlugin);

        public IEnumerable<IPlugin> LoadAll()
        {
            var domainSetup = new AppDomainSetup
            {
                ApplicationBase = AppDomain.CurrentDomain.BaseDirectory,
                ShadowCopyFiles = "true",
                ShadowCopyDirectories = this.rootDirectory.FullName
            };
            foreach (var token in this.addIns)
            {
                var domain = AppDomain.CreateDomain(token.Name, new Evidence(), domainSetup);
                this.domains.Add(token.Name, domain);
                yield return token.Activate<IPlugin>(domain);
            }
        }

        public void UnloadAll()
        {
            foreach (var domain in this.domains.Values)
            {
                AppDomain.Unload(domain);
            }
        }

        public IPlugin Load(string addInName)
        {
            if (this.domains.ContainsKey(addInName))
            {
                throw new PluginException("Plugin with this name is already loaded");
            }

            var addIn = this.addIns.Single(ai => ai.Name == addInName);
            var domainSetup = new AppDomainSetup
            {
                ApplicationBase = AppDomain.CurrentDomain.BaseDirectory,
                ShadowCopyFiles = "true",
                ShadowCopyDirectories = this.rootDirectory.FullName
            };
            var domain = AppDomain.CreateDomain(addIn.Name, new Evidence(), domainSetup);
            this.domains.Add(addInName, domain);

            return addIn.Activate<IPlugin>(domain);
        }

        public bool TryUnload(string addInName)
        {
            AppDomain domain;
            if (!this.domains.TryGetValue(addInName, out domain))
            {
                throw new PluginException("Plugin with this name is not loaded");
            }

            try
            {
                AppDomain.Unload(domain);
                this.domains.Remove(addInName);
            }
            catch (CannotUnloadAppDomainException ex)
            {
                return false;
            }

            return true;
        }

        public void Dispose()
        {
            this.UnloadAll();
            this.addIns.Clear();
            this.domains.Clear();
        }
    }
}