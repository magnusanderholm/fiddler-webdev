namespace Fiddler.Webdev.Model
{
    using System;
    using System.Collections.ObjectModel;

    public class SortedObservableCollection<T> : ObservableCollection<T> where T:IComparable<T>
    {
        public SortedObservableCollection()
            : base()
        {
        }
        
        protected override void InsertItem(int unsortedIndex, T item)
        {
            // Recalculate insertion index based on sort order.
            int sortedIndex = 0;
            for (sortedIndex = 0; sortedIndex < this.Count; ++sortedIndex)
            {
                if (item.CompareTo(this[sortedIndex]) < 1)
                    break;
            }                            
            base.InsertItem(sortedIndex, item);
        }

        protected override void MoveItem(int oldIndex, int newIndex)
        {
            throw new NotImplementedException();
            // base.MoveItem(oldIndex, newIndex);
        }

        protected override void SetItem(int index, T item)
        {
            throw new NotImplementedException();
            // base.SetItem(index, item);
        }
    }
}
