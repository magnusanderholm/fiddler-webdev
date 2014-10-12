using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Fiddler.LocalRedirect.Model
{
    public class ObserveChange : Fiddler.LocalRedirect.Model.IObserveChange
    {
        public void Observe(INotifyPropertyChanged nPc)
        {
            nPc.PropertyChanged -= OnPropertyChanged;
            nPc.PropertyChanged += OnPropertyChanged;
        }

        
        public void Observe<T>(ObservableCollection<T> coll)
        {
            coll.CollectionChanged -= OnCollectionChanged;   
            coll.CollectionChanged += OnCollectionChanged;   
        }
        
        public event EventHandler<EventArgs> Changed;

        protected virtual void OnChanged(EventArgs e)
        {
            var h = Changed;
            if (h != null)
                h(this, e);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnChanged(EventArgs.Empty);
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnChanged(EventArgs.Empty);
        }
    }
}
