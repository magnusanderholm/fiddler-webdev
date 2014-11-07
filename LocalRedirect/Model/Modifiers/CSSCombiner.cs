namespace Fiddler.LocalRedirect.Model
{
    using System;
    using System.ComponentModel;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract(Name="csscombiner", Namespace="")]
    [Modifier(Order = 0, IsEnabled = false)] 
    [Serializable()]
    [XmlRoot(Namespace = "", ElementName = "csscombiner")]
    public class CSSCombiner : Modifier
    {
        private CSSCombiner()
        {
            
        }

        private void Initialize()
        {
            
        }

        public CSSCombiner(UrlRule parent)
            : base(parent)
        {                                    
        }        
        
    }
}
