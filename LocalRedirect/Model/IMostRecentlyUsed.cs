namespace Fiddler.LocalRedirect.Model
{
    using System.Collections.Generic;

    public interface IMostRecentlyUsed<TItem> : IEnumerable<TItem>
    {
        void Touch(TItem item);
    }
}
