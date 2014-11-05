namespace Fiddler.LocalRedirect.Model
{
    using System;

    public interface IEventBus
    {
        void Publish<TMessage>(object sender, TMessage message);
        IDisposable Subscribe<TSender, TMessage>(Action<TSender, TMessage> onRecieved);
    }
}
