namespace Fiddler.LocalRedirect.Model
{
    public interface ISessionModifier
    {
        void PeekAtResponseHeaders(Fiddler.Session session);

        void RequestAfter(Fiddler.Session session);

        void RequestBefore(Fiddler.Session session);

        void ResponseAfter(Fiddler.Session session);

        void ResponseBefore(Fiddler.Session session);

        void BeforeReturningError(Fiddler.Session session);
    }
}
