using Fiddler.Webdev.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSAutoResponder.Tests
{
    [TestClass]
    public class UrlRuleTests
    {
        [TestMethod]
        public void ConnectRequestShallMatchHttpsRule()
        {
            var rule = new UrlRule(null) { IsEnabled = true };
            rule.UrlString = "https://www.google.se/foobar";
            Assert.IsTrue(rule.IsMatch(new Uri("http://www.google.se:443"), true));
            
            rule.UrlString = "https://www.google.se";
            Assert.IsTrue(rule.IsMatch(new Uri("http://www.google.se:443"), true));

            rule.UrlString = "https://www.google.se:553";
            Assert.IsTrue(rule.IsMatch(new Uri("http://www.google.se:553"), true));
        }

        [TestMethod]
        public void ConnectRequestShallNotMatchHttpRule()
        {
            var rule = new UrlRule(null) { IsEnabled = true };
            rule.UrlString = "http://www.google.se/foobar";
            Assert.IsFalse(rule.IsMatch(new Uri("http://www.google.se:443"), true));

            rule.UrlString = "http://www.google.se";
            Assert.IsFalse(rule.IsMatch(new Uri("http://www.google.se:443"), true));
        }

        [TestMethod]
        public void NoConnectGivesPartialHttpRuleMatchBehaviour()
        {
            var rule = new UrlRule(null) { IsEnabled = true };
            rule.UrlString = "http://www.google.se/foobar";


            Assert.IsFalse(rule.IsMatch(new Uri("http://www.google.se:443"), false));
            Assert.IsFalse(rule.IsMatch(new Uri("http://www.google.se/"), false));
            Assert.IsFalse(rule.IsMatch(new Uri("http://www.google.se/fo"), false));
            Assert.IsTrue(rule.IsMatch(new Uri("http://www.google.se/foobar"), false));
            Assert.IsTrue(rule.IsMatch(new Uri("http://www.google.se/foobar/"), false));
            Assert.IsTrue(rule.IsMatch(new Uri("http://www.google.se/foobar/1"), false));
        }
    }
}
