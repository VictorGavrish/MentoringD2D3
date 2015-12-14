namespace HostSideAdapter
{
    using System.AddIn.Pipeline;

    using Contract;

    using HostView;

    [HostAdapter]
    public class ContractToHostViewAdapter : IPlugin
    {
        private readonly IPluginContract contract;

        public ContractToHostViewAdapter(IPluginContract contract)
        {
            this.contract = contract;
        }

        public string DoStuff()
        {
            return this.contract.DoStuff();
        }
    }
}