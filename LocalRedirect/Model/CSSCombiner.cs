namespace Fiddler.LocalRedirect.Config
{
    using System;
    using System.ComponentModel;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract(Name="csscombiner", Namespace="")]
    public class CSSCombiner : ChildSetting
    {                
        public CSSCombiner()
        {
            Initialize();
        }

        private void Initialize()
        {
            
        }

        public CSSCombiner(UrlRule parent)
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
