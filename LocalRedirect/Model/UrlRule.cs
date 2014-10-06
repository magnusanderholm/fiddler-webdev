namespace Fiddler.LocalRedirect.Config
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using System.Linq;

    public partial class UrlRule : IEnumerable<ChildSetting>
    {
        public static UrlRule CreateDefault()
        {
            var rule = new UrlRule();
            var children = new List<ChildSetting>();
            children.Add(new Redirect(rule, "localhost:80", false));
            children.Add(new HeaderScript(rule));
            children.Add(new BrowserLink(rule));
            return rule;
        }

        [XmlIgnore()]
        public string Scheme
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Url)
                  ? new Uri(Url).Scheme.ToUpper()
                  : "NONE";
            }
        }

        public IEnumerator<ChildSetting> GetEnumerator()
        {
            return Children.AsEnumerable().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
