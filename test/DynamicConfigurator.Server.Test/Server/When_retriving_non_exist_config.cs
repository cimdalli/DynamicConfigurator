using ClientTest;
using FluentAssertions;
using Nancy.Testing;
using NUnit.Framework;

namespace DynamicConfigurator.Test.Server
{

    public class Test2Settings
    {
        public string Application { get; set; }
        public PersistenceSettings Persistence { get; set; }
        public string NotPrimitive { get; set; }
        public PersistenceSettings NotComplex { get; set; }
    }

    public class PersistenceSettings
    {
        public Persistence Mongo { get; set; }
    }

    public class Persistence
    {
        public string Url { get; set; }
    }

    [TestFixture]
    public class Test
    {
        BrowserResponse _result;

        [OneTimeSetUp]
        public void Setup()
        {
            _result = BrowserTestSuite.Server.Get("test1");
        }

        [Test]
        public void it_should_return_empty()
        {
            _result.ShouldBeEquivalentTo(string.Empty);
        }
    }
}
