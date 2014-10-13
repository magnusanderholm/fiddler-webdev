namespace Fiddler.LocalRedirect.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SessionModifier
    {
        public static readonly SessionModifier Empty = new SessionModifier();

        public SessionModifier()
        {
            Modifiers = new ISessionModifier[] { };
        }

        public SessionModifier(Fiddler.Session session, IEnumerable<ISessionModifier> modifiers)
        {
            if (session == null)
                throw new ArgumentNullException("session");
            if (modifiers == null)
                throw new ArgumentNullException("modifiers");

            Modifiers = modifiers;
            Session = session;

            // FAKE HTTPS tunnels.
            if (!Session.HTTPMethodIs("CONNECT"))            
                Session["x-replywithtunnel"] = "FakeTunnel so we can get the actual HTTPS requests.";                            
        }

        public IEnumerable<ISessionModifier> Modifiers { get; private set; }

        public Fiddler.Session Session { get; private set; }

        public void PeekAtResponseHeaders()
        {            
            ApplySessionModification(Modifiers, Session, m => m.PeekAtResponseHeaders);            
        }

        public void RequestAfter()
        {
            ApplySessionModification(Modifiers, Session, m => m.RequestAfter);            
        }

        public void RequestBefore()
        {
            ApplySessionModification(Modifiers, Session, m => m.RequestBefore);            
        }

        public void ResponseAfter()
        {
            ApplySessionModification(Modifiers, Session, m => m.ResponseAfter);            
        }

        public void ResponseBefore()
        {
            ApplySessionModification(Modifiers, Session, m => m.ResponseBefore);
        }

        public void BeforeReturningError()
        {
            ApplySessionModification(Modifiers, Session, m => m.BeforeReturningError);            
        }

        private static void ApplySessionModification(IEnumerable<ISessionModifier> modifiers, Session session, Func<ISessionModifier, Action<Session>> method)
        {
            if (session != null && !session.HTTPMethodIs("CONNECT") && modifiers != null && modifiers.Any())
                foreach (var m in modifiers)                    
                    method(m)(session);
        }
    }
    
}
