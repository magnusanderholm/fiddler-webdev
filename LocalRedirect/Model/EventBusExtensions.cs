using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Fiddler.LocalRedirect.Model
{
    public static class EventBusExtensions
    {
        public static void PublishPropertyChangedOnEventBus(this INotifyPropertyChanged o, IEventBus eventBus, object sender = null)
        {
            PropertyChangedEventHandler h = (s, e) => eventBus.Publish(sender ?? s, e);
            o.PropertyChanged -= h;
            o.PropertyChanged += h;
        }

        public static void PublishCollectionChangedOnEventBus(this INotifyCollectionChanged o, IEventBus eventBus, object sender = null)
        {
            NotifyCollectionChangedEventHandler h = (s, e) => eventBus.Publish(sender ?? s, e);
            o.CollectionChanged -= h;
            o.CollectionChanged += h;
        }
    }
}
