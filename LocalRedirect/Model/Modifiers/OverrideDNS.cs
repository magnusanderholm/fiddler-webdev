namespace Fiddler.LocalRedirect.Model
{
    using System;
    using System.ComponentModel;
    using System.Net;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract(Name="overridedns", Namespace="")]
    [Modifier(Order = 1, IsEnabled = true)] 
    [Serializable()]
    [XmlRoot(Namespace = "", ElementName = "overridedns")]
    public class OverrideDNS : Modifier
    {
        private string toHost;
        private ushort toPort;

        private OverrideDNS()
            : this(null)
        {            
        }
        
        public OverrideDNS(UrlRule parent)
            : base(parent)
        {
            Initialize();
        }        
               
        [DataMember(Name="tohost", IsRequired=false), DefaultValue("127.0.0.1")]
        [XmlAttribute(AttributeName = "tohost")]
        public string ToHost
        {
            get { return toHost; }
            set 
            {                
                if (value != null)
                {
                    // Just make sure that the string is a valid ipv4 address
                    var ipAddress = IPAddress.Parse(value);
                }
                pC.Update(ref toHost, value);
            }
        }

        [DataMember(Name = "toport", IsRequired = false), DefaultValue(80)]
        [XmlAttribute(AttributeName = "toport")]
        public ushort ToPort
        {
            get { return toPort; }
            set { pC.Update(ref toPort, value); }
        }
        
                
        public override void RequestBefore(Session session)
        {
            base.RequestBefore(session);
            if (IsEnabled)
            {
                // Prevent this request from going through an upstream proxy              
                // override host and port. Host can be IP or name but most likely ip.
                session.bypassGateway = true;                
                session["x-overrideHost"] = string.Format("{0}:{1}",ToHost, ToPort);
                
                // We always use http for local redirects since https goes bananas because the certificates are different.             
                // its also easier to handle in the iis.
                session.oRequest.headers.UriScheme = "http";
            }
        }


        private void Initialize()
        {
            this.toHost = "127.0.0.1";
            toPort = 80;
        }

        [OnDeserializing]
        private void DeserializationInitializer(StreamingContext ctx)
        {
            this.Initialize();
        }

   
    }
}
