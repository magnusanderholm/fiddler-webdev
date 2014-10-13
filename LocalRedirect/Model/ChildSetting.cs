namespace Fiddler.LocalRedirect.Model
{
    using Fiddler.LocalRedirect.Model;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract()]
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
                
        public UrlRule Parent 
        {
            get { return parent; }
            set { pC.Update(ref parent, value); }
        }        
    }
}
