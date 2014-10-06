namespace Fiddler.LocalRedirect.Config
{
    public partial class BrowserLink : ChildSetting
    {
        public override void RequestBefore(Session session)
        {
            base.RequestBefore(session);
            if (IsEnabled /*&& this.HasScript*/)
            {
                // In order to be able to inject data in the response we need to buffer it.
                session.bBufferResponse = true;
            }
        }

        public override void ResponseBefore(Session session)
        {
            base.ResponseBefore(session);
            if (IsEnabled)
            {
                // Enable browser link by injecting corresponding scripts. Don't think we need to cache
                // anything as browserlink reads its config via memory mapped files.
            } 
        }
    }
}
