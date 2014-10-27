using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;

namespace Fiddler.LocalRedirect.Model
{    
    public class MostRecentlyUsed<T> : IEnumerable<T>, INotifyCollectionChanged
    {        
        private readonly RegistryKey key;
        private readonly string registryValueName;
        private static string separator = Environment.NewLine;
        private readonly List<T> files = new List<T>();
        private readonly int maxItems;
        private readonly Func<IEnumerable<T>, string> serializer;
        private readonly Func<string, IEnumerable<T>> deserializer;
        private readonly Func<T, T,int> comparer;
        private readonly T[] constantItems;

        public MostRecentlyUsed(
            string registryKey, 
            string registryValueName, 
            int maxItems,
            Func<IEnumerable<T>, string> serializer,
            Func<string, IEnumerable<T>> deserializer, 
            Func<T,T, int> comparer, 
            params T[] constantItems)
        {
            this.constantItems = constantItems;
            this.serializer = serializer;
            this.deserializer = deserializer;
            this.comparer = comparer;
            this.maxItems = maxItems;
            this.registryValueName = registryValueName;
            

            // TODO Perhaps we shall always list the default location???? 
            this.key = Registry.CurrentUser.CreateSubKey(registryKey); // Make sure key exists.                  
            this.files.AddRange(deserializer((string)key.GetValue(registryValueName, String.Empty)));
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void AddOrUpdate(T path)
        {            
            var idx = files.FindIndex(f => comparer(f,path) == 0);
            if (idx >= 0)
            {
                var tmp = files[idx];
                files.RemoveAt(idx);
                OnDeleted(tmp, idx);
            }
            
            files.Add(path);
            OnAdded(path, files.Count - 1);
            key.SetValue(registryValueName, serializer(files));
        }

        public IEnumerator<T> GetEnumerator()
        {
            return constantItems.Union(files).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected virtual void OnDeleted(T t, int index)
        {
            index += constantItems.Length;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, t, index));
        }

        protected virtual void OnAdded(T t, int index)
        {
            index += constantItems.Length;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, t, index));
        }


        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            var h = CollectionChanged;
            if (h != null)
                h(this, e);
        }
    }
}
