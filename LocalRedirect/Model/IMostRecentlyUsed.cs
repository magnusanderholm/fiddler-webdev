using System;
using System.Collections.Generic;
using System.Collections.Specialized;
namespace Fiddler.LocalRedirect.Model
{
    public interface IMostRecentlyUsed<TItem> : IEnumerable<TItem>
    {
        void Touch(TItem item);
    }
}
