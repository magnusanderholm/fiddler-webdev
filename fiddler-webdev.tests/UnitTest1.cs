using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fiddler.Webdev.Model;


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

        [TestMethod]
        public void ArteryConnectionUtil()
        {
            var blc = new Fiddler.Webdev.BrowserLink.BrowserLinkConfiguration();
            var connections = blc.GetAllBrowserLinkConnections().ToArray();
            // Microsoft.VisualStudio.Web.PageInspector.Runtime, Version=12.3.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
            // Microsoft.VisualStudio.Web.BrowserLink.Runtime.ArteryConnectionUtil

        }
    }
}
