namespace Fiddler.LocalRedirect.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;

    public class ChangeObserver
    {
        public ChangeObserver(EventHandler<EventArgs> changed = null)
        {
            if (changed != null)
                Changed += changed;
        }

        public event EventHandler<EventArgs> Changed;

        public void Register(object o)
        {
            var notifyPropertyChanged = o as INotifyPropertyChanged;
            var notifyCollectionChanged = o as INotifyCollectionChanged;

            if (notifyPropertyChanged != null)
            {
                notifyPropertyChanged.PropertyChanged -= OnPropertyChanged;
                notifyPropertyChanged.PropertyChanged += OnPropertyChanged;                
            }

            if (notifyCollectionChanged != null)
            {
                notifyCollectionChanged.CollectionChanged -= OnChanged;
                notifyCollectionChanged.CollectionChanged += OnChanged;
            }
        }

        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        protected virtual void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (object o in e.NewItems)
                {
                    if (o is INotifyPropertyChanged || o is INotifyCollectionChanged)
                    {
                        Register(o);
                    }                                                                           
                }
            }
        }
    
        protected virtual void OnChanged(object sender, EventArgs e)
        {
            // If we CAN look at the property (using reflection should be simple enough) 
            // then we can check the value of said property and call Register on it.
            var notifyPropertyChangedEventArgs = e as PropertyChangedEventArgs;
            var notifyCollectionChangedEventArgs = e as NotifyCollectionChangedEventArgs;
            
            var h = Changed;
            if (h != null)
                h(sender, e);
        }

        private bool TryGetPropertyValue(string propertyName, out object val)
        {
            bool ret = false;
            val = null;

            return ret;
        }
    }
}
