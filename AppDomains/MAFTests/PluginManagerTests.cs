namespace MAFTests
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Reflection;
    using System.Threading;

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
            if (Directory.Exists(tempDirectoryName))
            {
                Directory.Delete(tempDirectoryName, true);
            }
            ZipFile.ExtractToDirectory(pluginPath, tempDirectoryName);

            var rootFolder = Path.Combine(Environment.CurrentDirectory, tempDirectoryName, "Pipeline");

            var manager = new PluginManager(rootFolder);
            manager.Load("ExamplePlugin1");
            manager.Plugin.DoStuff().Should().BeEquivalentTo("From example plugin 1");
            manager.TryUnload().ShouldBeEquivalentTo(true);
            manager.Plugin.Should().Be(null);
            manager.Load("ExamplePlugin2");
            manager.Plugin.DoStuff().Should().BeEquivalentTo("From example plugin 2");
            manager.TryUnload().ShouldBeEquivalentTo(true);

            Thread.Sleep(100);
            Directory.Delete(tempDirectoryName, true);
        }
    }
}