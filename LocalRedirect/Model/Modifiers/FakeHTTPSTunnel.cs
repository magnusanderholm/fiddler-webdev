﻿namespace Fiddler.LocalRedirect.Model
{
    public class FakeHTTPSTunnel : ISessionModifier
    {
        public FakeHTTPSTunnel()
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
            if (session.HTTPMethodIs("CONNECT"))                
                session["x-replywithtunnel"] = "FakeTunnel so we can get the actual HTTPS requests.";   
        }

        public void ResponseAfter(Session session)
        {
            
        }

        public void ResponseBefore(Session session)
        {
            
        }

        public void BeforeReturningError(Session session)
        {
            
        }     
    }
}
