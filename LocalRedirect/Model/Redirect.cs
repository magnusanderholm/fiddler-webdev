namespace Fiddler.LocalRedirect.Config
{
    using System;
    using System.Xml.Serialization;

    public partial class Redirect : ChildSetting
    {        
        public Redirect(UrlRule parent, string host, bool forceUnminfied)
            : base(parent)
        {            
            this.ToHost = host;
            this.ForceUnminified = ForceUnminified;
        }

        [XmlIgnore()]                                           
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
            if (IsEnabled && CanRedirect)
            {
                // Prevent this request from going through an upstream proxy              
                session.bypassGateway = true;
                // override host and port. Host can be IP or name but most likely ip.
                session["x-overrideHost"] = ToHost.ToString();

                if (!ForceUnminified)
                {
                    session.url = session.url.Replace(".min.css", ".css");
                    session.url = session.url.Replace(".min.js", ".js");
                }

                // We always use http for local redirects since https goes bananas because the certificates are different.             
                // its also easier to handle in the iis.
                session.oRequest.headers.UriScheme = "http";
            }
        }
    }
}
