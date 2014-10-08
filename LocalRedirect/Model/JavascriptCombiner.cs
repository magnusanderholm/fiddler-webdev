namespace Fiddler.LocalRedirect.Config
{
    using System;
    using System.ComponentModel;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract(Name="javascriptcombiner", Namespace="")]
    public class JavascriptCombiner : ChildSetting
    {                
        public JavascriptCombiner()
        {
            Initialize();
        }

        private void Initialize()
        {
            
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
    }
}
