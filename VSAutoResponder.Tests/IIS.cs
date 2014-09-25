using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fiddler.VSAutoResponder.Model;
using System.Linq;
namespace VSAutoResponder.Tests
{
    [TestClass]
    public class IISTests
    {
        [TestMethod]
        public void GetBindings()
        {
            var iis = new IIS();
            var sites = iis.GetSites(new Uri("https://dev.blob")).ToArray();

        }
    }
}
