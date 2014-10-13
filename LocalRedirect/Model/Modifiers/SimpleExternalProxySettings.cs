using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fiddler.LocalRedirect.Model.Modifiers
{
    // TODO When executed it will render a simple html page in the repsonse.
    //      This page will contain ip + port (proxy settings) that can be copied
    //      to clipboard just by clicking. It will also contain the certificate link
    //      and also inform the user if bcert is not installed.
    public class SimpleExternalProxySettings : ISessionModifier
    {
        // public string const

        public SimpleExternalProxySettings()
        {

        }

        public void PeekAtResponseHeaders(Session session)
        {            
        }

        public void RequestAfter(Session session)
        {
         
        }

        public void RequestBefore(Session session)
        {
         
        }

        public void ResponseAfter(Session session)
        {
         
        }

        public void ResponseBefore(Session session)
        {
           // OK Get IP and port of fiddler
           // Generate a nice looking HTML page
           // where we can copy ip + port for easy proxy setting on phone.
           // also generate a link for the certificate. Check if bcert is installed
           // otherwise issue a warning about that on the page as well.
        }

        public void BeforeReturningError(Session session)
        {
         
        }
    }
}
