namespace Fiddler.LocalRedirect.Model
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    public class ObservableItemCollection<T> : ObservableCollection<T> where T:INotifyPropertyChanged
    {
        // TODO Can we avoid getting duplicates in Add???

        protected override void OnCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                    item.PropertyChanged += OnItemPropertyChanged;
            }
            
            base.OnCollectionChanged(e);
        }

        public event EventHandler<ItemPropertyChangedEventArgs<T>> ItemPropertyChanged;

        private void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var h = ItemPropertyChanged;
            if (h != null)
                h(this, new ItemPropertyChangedEventArgs<T>((T)sender, e.PropertyName));
        }
    }

    public class ItemPropertyChangedEventArgs<T> : EventArgs
    {
        public ItemPropertyChangedEventArgs(T item, string propertyName)
        {
            Item = item;
            PropertyName = propertyName;
        }

        public T Item { get; private set; }

        public string PropertyName { get; private set; }
    }

}
