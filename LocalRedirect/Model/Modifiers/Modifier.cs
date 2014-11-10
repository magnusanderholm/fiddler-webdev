namespace Fiddler.LocalRedirect.Model
{
    using Fiddler.LocalRedirect.Model;
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using System.Linq;

    [DataContract()]
    [Serializable()]    
    public abstract class Modifier: ModifierBase, IComparable<Modifier>
    {
        private UrlRule parent;        
        private string compareValue = string.Empty;
        
        public Modifier()
            : this(null)
        {         
            
        }

        public Modifier(UrlRule parent)
            : base()
        {
            Parent = parent;
            var t = GetType();

            var modAtr = (ModifierAttribute)t.GetCustomAttributes(typeof(ModifierAttribute), true).FirstOrDefault();
            compareValue = modAtr != null
                ? modAtr.Order.ToString("{000000000}") + "#" + t.FullName
                : t.FullName;         
        }
                
        [XmlIgnore()]
        public UrlRule Parent 
        {
            get { return parent; }
            set { pC.Update(ref parent, value); }
        }

        public int CompareTo(Modifier other)
        {
            if (other == null)
                return 1; // This instance preceedes other
            if (other == this)
                return 0;

            return string.Compare(compareValue, other.compareValue);
        }         
    }
}
