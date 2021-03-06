﻿namespace MAFTests
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

            var plugin1 = manager.Load("ExamplePlugin1");
            var plugin2 = manager.Load("ExamplePlugin2");
            var plugin3 = manager.Load("ExamplePlugin3");

            plugin1.DoStuff().Should().BeEquivalentTo("From example plugin 1");
            plugin2.DoStuff().Should().BeEquivalentTo("From example plugin 2");
            plugin3.DoStuff().Should().BeEquivalentTo("From example plugin 3");

            manager.TryUnload("ExamplePlugin1").ShouldBeEquivalentTo(true);
            manager.TryUnload("ExamplePlugin2").ShouldBeEquivalentTo(true);
            manager.TryUnload("ExamplePlugin3").ShouldBeEquivalentTo(true);

            Directory.Delete(tempDirectoryName, true);
        }
    }
}