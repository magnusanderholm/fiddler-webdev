using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fiddler.LocalRedirect.Model
{
    public class SessionModifier
    {
        public static readonly SessionModifier Empty = new SessionModifier(null, Redirect.Empty);

        public SessionModifier(Fiddler.Session session, Redirect redirect)
        {
            Redirect = redirect;
            Session = session;
        }

        public Redirect Redirect { get; private set; }

        public Fiddler.Session Session { get; private set; }

        public void PeekAtResponseHeaders()
        {
            
        }

        public void RequestAfter()
        {
            
        }

        public void RequestBefore()
        {
            if (Redirect.IsEnabled && Redirect.CanRedirect)
            {
                // Prevent this request from going through an upstream proxy              
                Session.bypassGateway = true;
                // override host and port. Host can be IP or name but most likely ip.
                Session["x-overrideHost"] = Redirect.ToHost.ToString();

                if (!Redirect.ForceUnminified)
                {
                    Session.url = Session.url.Replace(".min.css", ".css");
                    Session.url = Session.url.Replace(".min.js", ".js");
                }

                // We always use http for local redirects since https goes bananas because the certificates are different.             
                // its also easier to handle in the iis.
                Session.oRequest.headers.UriScheme = "http"; 
            }

            if ((Redirect.IsHeaderScriptEnabled && Redirect.HasHeaderScript) || Redirect.BrowserLinkEnabled)
            {
                // In order to be able to inject data in the response we need to buffer it.
                Session.bBufferResponse = true;
            }
        }

        public void ResponseAfter()
        {
            
        }

        public void ResponseBefore()
        {
            if (Redirect.IsHeaderScriptEnabled && Redirect.HasHeaderScript && Session.oResponse.headers.ExistsAndContains("Content-Type","text/html"))
            {                 
                // TODO UseHtmlAgility pack for this
                Session.utilDecodeResponse();                                    
                Session.utilReplaceInResponse("</head>", "<!-- Injected by fiddler -->" + Redirect.HeaderScript + "</head>");    
            }

            if (Redirect.BrowserLinkEnabled)
            {
                // Enable browser link by injecting corresponding scripts. Don't think we need to cache
                // anything as browserlink reads its config via memory mapped files.
            }      
        }

        public void BeforeReturningError()
        {
            
        }
    }
}
