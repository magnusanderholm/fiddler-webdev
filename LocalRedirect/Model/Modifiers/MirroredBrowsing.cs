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
        {
            Initialize();
        }

        public MirroredBrowsing(UrlRule parent)
            : base(parent)
        {        
         
        }        
                                       
        [OnDeserializing]
        private void DeserializationInitializer(StreamingContext ctx)
        {
            this.Initialize();
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext ctx)
        {
            pC.Enabled = true;
        }
        

        private void Initialize()
        {

        }
    }
}
