namespace ExamplePluginV3
{
    using System.AddIn;

    using PluginView;

    [AddIn("ExamplePlugin3", Version = "1.0.0.0")]
    public class ExamplePluginV3 : IPlugin
    {
        public string DoStuff()
        {
            return "From example plugin 3";
        }
    }
}
