﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Fiddler.LocalRedirect.Model
{
    // TODO We could create a way to inject javascripts and css into head of any page
    //      by just looking at a html fragment (of head) file located in the root of the IIS site or similiar
    //      that html file is then inserted into head at the bottom. easy peasy. Best thing would be if we could
    //      use our redirect rule or another rule for that. Ie the html fragment is attached to the rule. That way it would
    //      be very easy to customize different parts of a customer sharepoint site without actually deploying anything at all.
    //      almost like simulating a master page.
    public class RedirectEngine
    {
        private Settings settings;
        private object _lock = new object();        

        public Settings Settings 
        {
            get
            {
                lock (_lock)
                    return settings;

            }
            
            set
            {
                lock (_lock)
                    settings = value;
            } 

        }
        
        // Modify the oSession object with redirect information.
        public bool TryRedirect(Fiddler.Session oSession)
        {
            // Cannot override HTTPS connects and point to a 
            // local https site because the certificates will be different
            if (Settings == null || oSession.HTTPMethodIs("CONNECT")) 
                return false;

            // Find best matching redirect rule (ie the one that is longest and matches the url in oSession). 
            var settings = Settings;
            var sessionUrl = oSession.fullUrl.ToLower(); // Hmm host must match fully???. Otherwise we'll get hits on https:// .. but we do not know where to.            
            var bestMatchingRedirect = settings
                .Redirects
                .Where(r => r.IsValid && r.IsEnabled && string.Compare(r.Host, oSession.hostname, true) == 0)
                .OrderByDescending(r => r.Url.Length)
                .FirstOrDefault(r => sessionUrl.StartsWith(r.Url));

            if (bestMatchingRedirect != null)
            {
                if (!bestMatchingRedirect.UseMinified)
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
