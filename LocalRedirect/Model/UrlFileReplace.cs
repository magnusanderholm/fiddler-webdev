namespace Fiddler.LocalRedirect.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.Serialization;
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;


    [DataContract(Name = "searchandreplace", Namespace = "")]
    public class SearchAndReplace
    {
        private readonly Regex regex;
        
        public SearchAndReplace(string name, string regex, string replace)
        {
            this.regex = new Regex(regex, RegexOptions.Compiled);
            this.Regex = regex;
            this.Name = name;
            this.Replacement = replace;
        }

        [DataMember(Name="name", IsRequired=true), DefaultValue("")]
        public string Name { get; private set; }

        [DataMember(Name = "regex", IsRequired = true), DefaultValue("")]
        public string Regex { get; private set; }

        [DataMember(Name = "replacement", IsRequired = true), DefaultValue("")]
        public string Replacement { get; private set; }
        
        public string Replace(string str)
        {
            return regex.Replace(str, Replacement);
        }

    }

    [DataContract(Name = "urlfileregexreplace", Namespace = "")]
    public class UrlFileRegexReplace : ChildSetting
    {
        private static SearchAndReplace[] availableExpressions = new SearchAndReplace[]
        {
            new SearchAndReplace("Force unminified", "", ""),
            new SearchAndReplace("Force sharepoint .debug.js", "", "")
        };

        public UrlFileRegexReplace()
        {
            this.AvailableExpressions = availableExpressions;
        }


        public UrlFileRegexReplace(UrlRule parent)
            : base(parent)
        {
        }


        public IEnumerable<SearchAndReplace> AvailableExpressions { get; private set; }



        // TODO the plugin needs two textboxes. One for search and one for replace.

        public override void RequestBefore(Session session)
        {
            base.RequestBefore(session);
            if (IsEnabled)
            {
                
                session.url = session.url.Replace(".min.css", ".css");
                session.url = session.url.Replace(".min.js", ".js");
            }
        }
    }
}
