namespace Fiddler.LocalRedirect.Model
{
    using Fiddler.LocalRedirect.Model;
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract()]
    [Serializable()]    
    [KnownType(typeof(UrlRule)), XmlInclude(typeof(UrlRule))]
    [KnownType(typeof(Redirect)), XmlInclude(typeof(Redirect))]
    [KnownType(typeof(BrowserLink)), XmlInclude(typeof(BrowserLink))]
    [KnownType(typeof(HeaderScript)), XmlInclude(typeof(HeaderScript))]
    [KnownType(typeof(ForceUnminified)), XmlInclude(typeof(ForceUnminified))]
    [KnownType(typeof(JavascriptCombiner)), XmlInclude(typeof(JavascriptCombiner))]
    [KnownType(typeof(CSSCombiner)), XmlInclude(typeof(CSSCombiner))]
    [KnownType(typeof(ForceSharepointDebugJavascript)), XmlInclude(typeof(ForceSharepointDebugJavascript))]
    [KnownType(typeof(OverrideDNS)), XmlInclude(typeof(OverrideDNS))]
    [KnownType(typeof(DisableCache)), XmlInclude(typeof(DisableCache))]
    [KnownType(typeof(FileResponse)), XmlInclude(typeof(FileResponse))]    
    public abstract class ChildSetting: Setting
    {
        private UrlRule parent;
        public ChildSetting()
            : this(null)
        {         
        }

        public ChildSetting(UrlRule parent)
            : base()
        {
            Parent = parent;
        }
                
        [XmlIgnore()]
        public UrlRule Parent 
        {
            get { return parent; }
            set { pC.Update(ref parent, value); }
        }        
    }
}
