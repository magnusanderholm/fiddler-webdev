namespace Fiddler.LocalRedirect.Model
{
    using System;
    using System.ComponentModel;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract(Name="javascriptcombiner", Namespace="")]
    [Modifier(Order = 0, IsEnabled=false)] 
    [Serializable()]
    [XmlRoot(Namespace = "", ElementName = "javascriptcombiner")]
    public class JavascriptCombiner : ChildSetting
    {
        private JavascriptCombiner()
        {
            Initialize();
        }

        public JavascriptCombiner(UrlRule parent)
            : base(parent)
        {                                    
        }        
                                       
        [OnDeserializing]
        private void DeserializationInitializer(StreamingContext ctx)
        {
            this.Initialize();
        }

        private void Initialize()
        {

        }
    }
}
