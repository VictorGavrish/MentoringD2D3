namespace PluginView
{
    using System.AddIn.Pipeline;

    [AddInBase]
    public interface IPlugin
    {
        string DoStuff();
    }
}