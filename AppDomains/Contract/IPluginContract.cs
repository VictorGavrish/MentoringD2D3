namespace Contract
{
    using System.AddIn.Contract;
    using System.AddIn.Pipeline;

    [AddInContract]
    public interface IPluginContract : IContract
    {
        string DoStuff();
    }
}