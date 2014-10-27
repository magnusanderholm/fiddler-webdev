namespace Fiddler.LocalRedirect.Model
{
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;

    public class MostRecentlyUsed<TItem> : IMostRecentlyUsed<TItem> where TItem:class
    {

        private readonly string registryValueName;
        private readonly RegistryKey registryKey;
        private readonly List<TItem> items = new List<TItem>();
        private readonly int maxItems;
        private readonly Func<IEnumerable<TItem>, string> serializer;
        private readonly Func<string, IEnumerable<TItem>> deserializer;
        private readonly Func<TItem, TItem, int> comparer;


        public MostRecentlyUsed(
            string registryKey,
            string registryValueName,
            int maxItems,
            Func<IEnumerable<TItem>, string> serializer,
            Func<string, IEnumerable<TItem>> deserializer,
            Func<TItem, TItem, int> comparer)
        {
            this.serializer = serializer;
            this.deserializer = deserializer;
            this.comparer = comparer;
            this.maxItems = maxItems;
            this.registryValueName = registryValueName;

            this.registryKey = Registry.CurrentUser.CreateSubKey(registryKey); // Make sure key exists.            
            items.AddRange(deserializer((string)this.registryKey.GetValue(registryValueName, String.Empty)));
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void Touch(TItem item)
        {
            var eventArgs = new List<NotifyCollectionChangedEventArgs>();
            
            var existingItemIndex = items.FindIndex(f => comparer(f, item) == 0);            
            if (existingItemIndex >= 0) 
            {
                item = items[existingItemIndex];
                items.RemoveAt(existingItemIndex);                  
            }

            items.Insert(0, item);
            if (existingItemIndex >= 0)
                eventArgs.Add(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, item, 0, existingItemIndex));
            else eventArgs.Add(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, 0));


            if (items.Count > maxItems) 
            {
                var removedItem = items.Last();
                items.RemoveAt(items.Count - 1);
                eventArgs.Add(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItem, items.Count - 1));
            }

            foreach (var e in eventArgs)
                OnCollectionChanged(e);

            registryKey.SetValue(registryValueName, serializer(items));
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            var h = CollectionChanged;
            if (h != null)
                h(this, e);
        }        
    }
}