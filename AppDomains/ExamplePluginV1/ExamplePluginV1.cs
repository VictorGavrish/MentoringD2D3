namespace ExamplePluginV1
{
    using System.AddIn;

    using PluginView;

    [AddIn("ExamplePlugin1", Version = "1.0.0.0")]
    public class ExamplePluginV1 : IPlugin
    {
        public string DoStuff()
        {
            return "From example plugin 1";
        }
    }
}