namespace Fiddler.LocalRedirect.Model
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public static class IEnumerableExtensions
    {
        public static ObservableCollection<TItem> ToObservableCollection<TItem>(this IEnumerable<TItem> enumerable)
        {
            var c = new ObservableCollection<TItem>();
            c.AddRange(enumerable);
            return c;
        }

        public static void AddRange<TItem>(this ObservableCollection<TItem> c, IEnumerable<TItem> items)
        {
            foreach (var i in items)
                c.Add(i);
        }
    }
}
