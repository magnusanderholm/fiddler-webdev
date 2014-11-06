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
        private object anything;
        
        [TestInitialize]
        public void Initialize()
        {
            eb = new EventBus();
        }

        [TestMethod]
        public void SubscribeAndPublish()
        {            
            var subscription0 = eb.SubscribeTo<EventBusTest, Uri>(OnIntegerReceived);
            var subscription1 = eb.SubscribeTo<EventBusTest, Uri>(OnAnythingReceived);
            eb.Publish(this, new Uri("http://ten"));
            Assert.AreEqual("http://ten/", msg);
            Assert.AreEqual(new Uri("http://ten/").ToString(), ((Uri)anything).ToString());
            
            eb.Publish(this, new Uri("http://eleven"));
            Assert.AreEqual("http://eleven/", msg);
            Assert.AreEqual(new Uri("http://eleven/").ToString(), ((Uri)anything).ToString());
            

            subscription0.Dispose();
            eb.Publish(this, new Uri("http://twelve"));
            Assert.AreEqual("http://eleven/", msg);
            Assert.AreEqual(new Uri("http://twelve/").ToString(), ((Uri)anything).ToString());            
        }

        [TestMethod]
        public void SubscribeAndPublishReceiveAnything()
        {
            var subscription = eb.SubscribeTo<EventBusTest, object>(OnAnythingReceived);
            eb.Publish(this, "foobar");
            Assert.AreEqual("foobar", (string)anything);
            
            eb.Publish(this, 10);
            Assert.AreEqual(10, (int)anything);

            subscription.Dispose();
            eb.Publish(this, new Uri("http://twelve"));
            Assert.AreEqual(new Uri("http://eleven/").ToString(), ((Uri)anything).ToString());
        }

        protected void OnIntegerReceived(EventBusTest sender, Uri url)
        {
            msg = url.ToString();
        }

        protected void OnAnythingReceived(EventBusTest sender, object o)
        {
            anything = o;
        }
    }
}
