namespace Fiddler.LocalRedirect.Model
{
    using Fiddler.LocalRedirect.Model;
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract()]
    [Serializable()]    
    public abstract class Modifier: ModifierBase
    {
        private UrlRule parent;
        public Modifier()
            : this(null)
        {         
        }

        public Modifier(UrlRule parent)
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
