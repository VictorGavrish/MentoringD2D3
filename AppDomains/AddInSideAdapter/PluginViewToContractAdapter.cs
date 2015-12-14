namespace AddInSideAdapter
{
    using System.AddIn.Pipeline;

    using Contract;

    using PluginView;

    [AddInAdapter]
    public class PluginViewToContractAdapter : ContractBase, IPluginContract
    {
        private readonly IPlugin plugin;

        public PluginViewToContractAdapter(IPlugin plugin)
        {
            this.plugin = plugin;
        }

        public string DoStuff()
        {
            return this.plugin.DoStuff();
        }
    }
}