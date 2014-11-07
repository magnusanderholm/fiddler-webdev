namespace Fiddler.LocalRedirect.Model
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract(Name = "mirroredbrowsing", Namespace = "")]
    [Modifier(Order = 0, IsEnabled=false)] 
    [Serializable()]
    [XmlRoot(Namespace = "", ElementName = "mirroredbrowsing")]
    public class MirroredBrowsing : Modifier
    {
        private MirroredBrowsing()
            : this(null)
        {
           
        }

        public MirroredBrowsing(UrlRule parent)
            : base(parent)
        {        
         
        }                                              
    }
}
