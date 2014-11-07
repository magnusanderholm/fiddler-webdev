namespace Fiddler.LocalRedirect.Model
{
    using System;
    using System.Collections.Specialized;
    using System.ComponentModel;

    public interface IEventBus
    {
        void Publish<TMessage>(object sender, TMessage message);
        IDisposable SubscribeTo<TSender, TMessage>(Action<TSender, TMessage> onRecieved);
        void PublishChanges(INotifyPropertyChanged nPc);
        void PublishChanges(INotifyCollectionChanged nCc);
    }
}
