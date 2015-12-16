namespace ExamplePluginV2
{
    using System.AddIn;

    using PluginView;

    [AddIn("ExamplePlugin2", Version = "1.0.0.0")]
    public class ExamplePluginV2 : IPlugin
    {
        public string DoStuff()
        {
            return "From example plugin 2";
        }
    }
}
