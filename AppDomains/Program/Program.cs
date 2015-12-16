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
            var plugin1 = manager.Load("ExamplePlugin1");
            var plugin2 = manager.Load("ExamplePlugin2");
            var plugin3 = manager.Load("ExamplePlugin3");
            Console.WriteLine(plugin1.DoStuff());
            Console.WriteLine(plugin2.DoStuff());
            Console.WriteLine(plugin3.DoStuff());
            manager.TryUnload("ExamplePlugin1");
            manager.TryUnload("ExamplePlugin2");
            manager.TryUnload("ExamplePlugin3");
            Console.ReadLine();
        }
    }
}