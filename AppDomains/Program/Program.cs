namespace Program
{
    using System;
    using System.IO;

    using AppDomainsMAF;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var rootFolder = Path.Combine(Environment.CurrentDirectory, "Pipeline");

            var manager = new PluginManager(rootFolder);
            manager.Load("ExamplePlugin1");
            Console.WriteLine(manager.Plugin.DoStuff());
            manager.TryUnload();
            manager.Load("ExamplePlugin2");
            Console.WriteLine(manager.Plugin.DoStuff());
            manager.TryUnload();
            manager.Load("ExamplePlugin3");
            Console.WriteLine(manager.Plugin.DoStuff());
            manager.TryUnload();
            Console.ReadLine();
        }
    }
}