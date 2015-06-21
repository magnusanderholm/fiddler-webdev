namespace Fiddler.Webdev.Model
{
    using System;
    using System.Collections.Generic;

    public class MostRecentlyUsed<TItem> : IMostRecentlyUsed<TItem>
    {
        private readonly IList<TItem> items;
        private  Func<TItem, TItem, bool> equals = ((TItem i0, TItem i1) => object.Equals(i0, i1));
        public MostRecentlyUsed(IList<TItem> items, int maxItems, Func<TItem, TItem, bool> equals = null)
        {
            this.equals = equals ?? this.equals;
            this.MaxItems = maxItems;
            this.items = items;
        }

        public int MaxItems { get; private set; }

        // Check if an item exist in the collection. If it does exist move it to the beginning of the collection
        // otherwise add it to the collection of the list.
        public void Touch(TItem item)
        {                        
            for (int idx = 0; idx < items.Count; ++idx)
            {
                if (equals(items[idx], item))
                {
                    item = items[idx];
                    items.RemoveAt(idx);
                    break;
                }
            }

            // Make room for new item if needed.
            if (items.Count + 1 >= MaxItems)
                items.RemoveAt(items.Count - 1);
            
            // add item to beginning of list.
            items.Insert(0, item);            
        }

        public System.Collections.Generic.IEnumerator<TItem> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}