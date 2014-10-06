namespace Fiddler.LocalRedirect.Config
{
    using System.Collections.Generic;
    using System.Linq;
    public partial class Settings : IEnumerable<UrlRule>
    {

        public IEnumerator<UrlRule> GetEnumerator()
        {
            return UrlRules.AsEnumerable().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
