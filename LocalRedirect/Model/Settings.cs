namespace Fiddler.LocalRedirect.Config
{
    using System.Collections.Generic;
    using System.Linq;
    public partial class Settings : IEnumerable<UrlRule>
    {
        public static Settings CreateDefault()
        {
            var settings = new Settings() { UrlRules = new UrlRule[] { } };
            return settings;
        }
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
