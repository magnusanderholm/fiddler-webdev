using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fiddler.LocalRedirect.Model;

namespace VSAutoResponder.Tests
{
    [TestClass]
    public class EventBusTest
    {
        private EventBus eb;
        private string msg;
        
        [TestInitialize]
        public void Initialize()
        {
            eb = new EventBus();
        }

        [TestMethod]
        public void SubscribeAndPublish()
        {            
            var subscription = eb.Subscribe<EventBusTest, Uri>(OnIntegerReceived);
            eb.Publish(this, new Uri("http://ten"));
            Assert.AreEqual("http://ten/", msg);
            eb.Publish(this, new Uri("http://eleven"));
            Assert.AreEqual("http://eleven/", msg);
            
            subscription.Dispose();
            eb.Publish(this, new Uri("http://twelve"));
            Assert.AreEqual("http://eleven/", msg);
        }

        protected void OnIntegerReceived(EventBusTest sender, Uri url)
        {
            msg = url.ToString();
        }
    }
}
