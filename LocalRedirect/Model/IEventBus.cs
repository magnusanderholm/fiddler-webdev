namespace Fiddler.LocalRedirect.Model
{
    using System;

    public interface IEventBus
    {
        void Publish<TMessage>(object sender, TMessage message);
        IDisposable SubscribeTo<TSender, TMessage>(Action<TSender, TMessage> onRecieved);
    }
}
