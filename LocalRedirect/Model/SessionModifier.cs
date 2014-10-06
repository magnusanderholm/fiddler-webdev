namespace Fiddler.LocalRedirect.Model
{
    using System.Collections.Generic;

    public class SessionModifier
    {
        public static readonly SessionModifier Empty = new SessionModifier(null, new ISessionModifier[]{});

        public SessionModifier(Fiddler.Session session, IEnumerable<ISessionModifier> modifiers)
        {
            Modifiers = modifiers;
            Session = session;
        }

        public IEnumerable<ISessionModifier> Modifiers { get; private set; }

        public Fiddler.Session Session { get; private set; }

        public void PeekAtResponseHeaders()
        {
            foreach (var m in Modifiers)
                m.PeekAtResponseHeaders(Session);
        }

        public void RequestAfter()
        {
            foreach (var m in Modifiers)
                m.RequestAfter(Session);
        }

        public void RequestBefore()
        {
            foreach (var m in Modifiers)
                m.RequestBefore(Session);
        }

        public void ResponseAfter()
        {
            foreach (var m in Modifiers)
                m.ResponseAfter(Session);
        }

        public void ResponseBefore()
        {
            foreach (var m in Modifiers)
                m.ResponseBefore(Session);
        }

        public void BeforeReturningError()
        {
            foreach (var m in Modifiers)
                m.BeforeReturningError(Session);
        }
    }
}
