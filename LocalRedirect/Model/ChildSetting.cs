namespace Fiddler.LocalRedirect.Config
{
    using Fiddler.LocalRedirect.Model;
    using System.Xml.Serialization;
    
    public partial class ChildSetting: ISessionModifier
    {
        public ChildSetting()
        {
        }

        public ChildSetting(UrlRule parent)
        {
            Parent = parent;
        }

        // Must be set during deserialization.
        [XmlIgnore()]
        public UrlRule Parent { get; set; }

        public virtual void PeekAtResponseHeaders(Session session)
        {            
        }

        public virtual void RequestAfter(Session session)
        {            
        }

        public virtual void RequestBefore(Session session)
        {            
        }

        public virtual void ResponseAfter(Session session)
        {            
        }

        public virtual void ResponseBefore(Session session)
        {         
        }

        public virtual void BeforeReturningError(Session session)
        {            
        }
    }
}
