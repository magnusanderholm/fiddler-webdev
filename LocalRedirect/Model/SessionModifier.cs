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
        }

        public IEnumerable<ISessionModifier> Modifiers { get; private set; }

        public Fiddler.Session Session { get; private set; }

        public void PeekAtResponseHeaders()
        {            
            Apply(Modifiers, Session, m => m.PeekAtResponseHeaders);            
        }

        public void RequestAfter()
        {
            Apply(Modifiers, Session, m => m.RequestAfter);            
        }

        public void RequestBefore()
        {
            Apply(Modifiers, Session, m => m.RequestBefore);            
        }

        public void ResponseAfter()
        {
            Apply(Modifiers, Session, m => m.ResponseAfter);            
        }

        public void ResponseBefore()
        {
            Apply(Modifiers, Session, m => m.ResponseBefore);
        }

        public void BeforeReturningError()
        {
            Apply(Modifiers, Session, m => m.BeforeReturningError);            
        }

        private static void Apply(IEnumerable<ISessionModifier> modifiers, Session session, Func<ISessionModifier, Action<Session>> method)
        {
            if (modifiers != null && modifiers.Any() && session != null)
                foreach (var m in modifiers)
                    method(m)(session);
        }
    }
    
}
