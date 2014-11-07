﻿namespace Fiddler.LocalRedirect.Model
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
        private static readonly StreamingContext emptyStreamingContext = new StreamingContext();
        private string compareValue = string.Empty;
        
        public Modifier()
            : this(null)
        {         
            
        }

        public Modifier(UrlRule parent)
            : base()
        {
            Parent = parent;
            OnDeserialized(emptyStreamingContext);
        }
                
        [XmlIgnore()]
        public UrlRule Parent 
        {
            get { return parent; }
            set 
            {
                if (pC.Update(ref parent, value).IsChanged)                
                    this.PublishPropertyChangedOnEventBus(parent.Parent.Events);                
            }
        }

        public int CompareTo(Modifier other)
        {
            if (other == null)
                return 1; // This instance preceedes other
            if (other == this)
                return 0;

            return string.Compare(compareValue, other.compareValue);
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext ctx)
        {
            var t = GetType();
            
            var modAtr = (ModifierAttribute)t.GetCustomAttributes(typeof(ModifierAttribute), true).FirstOrDefault();
            compareValue = modAtr != null
                ? modAtr.Order.ToString("{000000000}") + "#" + t.FullName
                : t.FullName;            
        }        
    }
}
