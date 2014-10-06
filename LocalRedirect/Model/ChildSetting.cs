namespace Fiddler.LocalRedirect.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;

    [DataContract(Name = "child", Namespace = "")]
    [KnownType(typeof(BrowserLink))]
    [KnownType(typeof(HeaderScript))]
    [KnownType(typeof(Redirect))]        
    public abstract class ChildSetting : SettingBase
    {
        // Must be set during deserialization.
        public UrlRule UrlRule { get; set; }
    }
}
