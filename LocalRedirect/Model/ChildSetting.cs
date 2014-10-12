namespace Fiddler.LocalRedirect.Model
{
    using Fiddler.LocalRedirect.Model;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract()]
    public abstract class ChildSetting: Setting,  ISessionModifier
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
                
        public UrlRule Parent 
        {
            get { return parent; }
            set { pC.Update(ref parent, value); }
        }        

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
