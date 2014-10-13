using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VSAutoResponder.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var url = new Uri("https://foo", UriKind.Absolute);
            var port = url.GetComponents(UriComponents.HostAndPort, UriFormat.SafeUnescaped);

        }
    }
}
