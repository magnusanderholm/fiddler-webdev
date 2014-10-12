namespace Fiddler.LocalRedirect.Model
{
    using System;
    using System.ComponentModel;
    using System.Runtime.Serialization;

    [DataContract(Name="redirect", Namespace="")]
    [Modifier(Order = 0, IsEnabled = true)] 
    public class Redirect : ChildSetting
    {
        private string toUrl;        

        private Redirect()
        {
            Initialize();
        }

        private void Initialize()
        {
            this.toUrl = "http://localhost";
        }

        public Redirect(UrlRule parent)
            : base(parent)
        {
            Initialize();
        }        
               
        [DataMember(Name="tourl", IsRequired=false), DefaultValue("")]
        public string ToUrl
        {
            get { return this.toUrl; }
            set 
            {
                var val = value;
                if (!string.IsNullOrEmpty(val))
                    val = new Uri(val, UriKind.Absolute).ToString();
                pC.Update(ref toUrl, val); 
            }
        }
                        

        public override void RequestBefore(Session session)
        {
            base.RequestBefore(session);
            if (IsEnabled)            
                session.fullUrl = new Uri(new Uri(ToUrl), session.PathAndQuery).ToString();
        }
                
        [OnDeserializing]
        private void DeserializationInitializer(StreamingContext ctx)
        {
            this.Initialize();
        }
    }
}
