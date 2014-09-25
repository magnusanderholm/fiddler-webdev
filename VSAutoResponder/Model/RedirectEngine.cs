using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fiddler.VSAutoResponder.Model
{
    public class RedirectEngine
    {
        public RedirectEngine(Settings settings)
        {
            Settings = settings;
        }

        public Settings Settings {get; set;}

        // TODO Add a method were we can deminify minified css/js in the response as well. That would be really cool.
        //      use minified code as key and store in a dictionary so we can look it up for future reference faster

        // Modify the oSession object with redirect information.
        public bool TryRedirect(Fiddler.Session oSession)
        {
            if (oSession.HTTPMethodIs("CONNECT"))
                return false;

            // Find best matching redirect rule (ie the one that is longest and matces the url in oSession).            
            var bestMatchingRedirect = Settings
                .Redirects
                .OrderByDescending(r => r.Url.AbsoluteUri.Length)
                .FirstOrDefault(r => oSession.fullUrl.ToLower().StartsWith(r.Url.AbsoluteUri.ToLower()));

            if (bestMatchingRedirect != null)
            {
                if (bestMatchingRedirect.UseMinified)
                {
                    oSession.url = oSession.url.Replace(".min.css", ".css");
                    oSession.url = oSession.url.Replace(".min.js", ".js");
                }
                
                oSession.bypassGateway = true;  // Prevent this request from going through an upstream proxy
                oSession["x-overrideHost"] = "127.0.0.1"; // Point to a localhost site with same host name as original request.                
                // We always use http for local redirects since https goes bananas because the certificates are different.             
                // its also easier to handle in the iis.
                oSession.oRequest.headers.UriScheme = "http"; 
            }
                        
            return bestMatchingRedirect != null;
        }
    }
}
