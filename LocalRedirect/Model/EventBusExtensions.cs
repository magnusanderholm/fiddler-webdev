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
        public static void PublishPropertyChangedOnEventBus(this INotifyPropertyChanged o, IEventBus eventBus)
        {
            PropertyChangedEventHandler h = (s, e) => eventBus.Publish(s, e);
            o.PropertyChanged -= h;
            o.PropertyChanged += h;
        }

        public static void PublishCollectionChangedOnEventBus(this INotifyCollectionChanged o, IEventBus eventBus)
        {
            NotifyCollectionChangedEventHandler h = (s, e) => eventBus.Publish(s, e);
            o.CollectionChanged -= h;
            o.CollectionChanged += h;
        }
    }
}
