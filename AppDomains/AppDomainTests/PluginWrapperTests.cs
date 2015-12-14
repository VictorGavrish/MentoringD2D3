namespace AppDomainTests
{
    using System;

    using AppDomains;

    using FluentAssertions;

    using Xunit;

    public class PluginWrapperTests
    {
        [Fact]
        public void PluginWrapperCanCreatePlugins()
        {
            // arrange

            // act
            var wrapper = new PluginWrapper(typeof(ExamplePlugin));
            wrapper.Start();
            
            // assert
            wrapper.Plugin.Should().BeOfType<ExamplePlugin>();
            wrapper.Plugin.DoStuff().Should().BeEquivalentTo("AppDomainTests.PluginWrapperTests+ExamplePlugin");
        }

        [Fact]
        public void PluginWrapperCanStopPlugins()
        {
            // arrange
            var wrapper = new PluginWrapper(typeof(ExamplePlugin));
            wrapper.Start();

            // act
            var success = wrapper.TryUnload();

            // assert
            success.ShouldBeEquivalentTo(true);
            wrapper.Plugin.Should().Be(null);
        }

        private class ExamplePlugin : MarshalByRefObject, IPlugin
        {
            public string DoStuff()
            {
                return AppDomain.CurrentDomain.FriendlyName;
            }
        }
    }
}