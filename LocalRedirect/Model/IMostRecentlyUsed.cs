using System;
using System.Collections.Generic;
using System.Collections.Specialized;
namespace Fiddler.LocalRedirect.Model
{
    public interface IMostRecentlyUsed<TItem> : IEnumerable<TItem>, INotifyCollectionChanged where TItem : class
    {
        void Touch(TItem item);
    }
}
