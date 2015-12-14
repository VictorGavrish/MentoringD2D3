namespace MAFTests
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Reflection;

    using AppDomainsMAF;

    using FluentAssertions;

    using Xunit;

    public class PluginManagerTests
    {
        [Fact]
        public void PluginWrapperCanCreatePlugins()
        {
            var pluginPath = @"Resources\Pipeline.zip";
            var tempDirectoryName = MethodBase.GetCurrentMethod().Name;
            ZipFile.ExtractToDirectory(pluginPath, tempDirectoryName);

            var rootFolder = Path.Combine(Environment.CurrentDirectory, tempDirectoryName, "Pipeline");

            var manager = new PluginManager(rootFolder, "ExamplePlugin1");
            manager.Load();
            manager.Plugin.DoStuff().Should().BeEquivalentTo("From example MAF plugin");
            manager.TryUnload().ShouldBeEquivalentTo(true);

            Directory.Delete(tempDirectoryName, true);
        }
    }
}