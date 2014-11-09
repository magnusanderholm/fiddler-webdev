namespace Fiddler.LocalRedirect.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class SessionModifier
    {
        public static readonly SessionModifier Empty = new SessionModifier();
        private static readonly ILogger logger = LogManager.CreateCurrentClassLogger();

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

        private static void ApplySessionModification(IEnumerable<ISessionModifier> modifiers, Session session, Func<ISessionModifier, Action<Session>> method, [CallerMemberName]string callingMember = "")
        {
            if (session != null && modifiers != null && modifiers.Any())
            {
                foreach (var m in modifiers)
                {
                    try
                    {
                        method(m)(session);
                    }
                    catch (Exception e)
                    {
                        logger.Error(() => string.Format("Unexpected exception in {0}.{1}", m.GetType().FullName, callingMember), e);
                    }
                }
            }

        }
    }

}
