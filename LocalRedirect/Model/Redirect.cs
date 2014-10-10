namespace Fiddler.LocalRedirect.Config
{
    using System;
    using System.ComponentModel;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract(Name="redirect", Namespace="")]
    public class Redirect : ChildSetting
    {
        private string toHost;
        private bool forceUnminified;

        public Redirect()
        {
            Initialize();
        }

        private void Initialize()
        {
            this.forceUnminified = false;
            this.toHost = string.Empty;
        }

        public Redirect(UrlRule parent, string host, bool forceUnminfied)
            : base(parent)
        {            
            this.ToHost = host;
            this.ForceUnminified = ForceUnminified;
        }        
               
        [DataMember(Name="tohost", IsRequired=false), DefaultValue("")]
        public string ToHost
        {
            get { return this.toHost; }
            set { pC.Update(ref toHost, value); }
        }

        [DataMember(Name = "forceunminified", IsRequired = false), DefaultValue(false)]
        public bool ForceUnminified
        {
            get { return this.forceUnminified; }
            set { pC.Update(ref forceUnminified, value); }
        }
        
        public bool CanRedirect
        {
            get 
            { 
                return 
                    (Parent.Url.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase) ||
                    Parent.Url.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase)) &&
                    !string.IsNullOrEmpty(ToHost); 
            }
        }
        

        public override void RequestBefore(Session session)
        {
            base.RequestBefore(session);
            if (Parent.IsEnabled && IsEnabled && CanRedirect)
            {
                // Prevent this request from going through an upstream proxy              
                //session.bypassGateway = true;
                // override host and port. Host can be IP or name but most likely ip.
                // session["x-overrideHost"] = ;
                session.host = ToHost.ToString();
                // We always use http for local redirects since https goes bananas because the certificates are different.             
                // its also easier to handle in the iis.
                session.oRequest.headers.UriScheme = "http";
            }
        }
                
        [OnDeserializing]
        private void DeserializationInitializer(StreamingContext ctx)
        {
            this.Initialize();
        }
    }
}
