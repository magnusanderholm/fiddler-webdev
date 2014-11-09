namespace Fiddler.LocalRedirect.BrowserLink
{
    using System;
    using System.Collections.Generic;
    using System.Security.AccessControl;
    using System.Threading;

    public class BrowserLinkConnection
    {
        private static readonly TimeSpan ArteryStartupTimeout = TimeSpan.FromMilliseconds(1500.0);

        public BrowserLinkConnection(string connectionString, string sslConnectionString, string requestSignalName, string readySignalName, IEnumerable<string> projectPaths)
        {
            this.ConnectionString = connectionString;
            this.SslConnectionString = sslConnectionString;
            this.RequestSignalName = requestSignalName;
            this.ReadySignalName = readySignalName;
            this.ProjectPaths = projectPaths;
            
        }

        public string GetHtmlScript(Uri url, BrowserIdentifier browserIdentifier)
        {
            return GetInitializationDataScript(
                Guid.NewGuid(), 
                browserIdentifier.Id, 
                url.Scheme == "https" ? SslConnectionString : ConnectionString);
        }        

        public string RequestSignalName { get; private set; }
		
        public string ReadySignalName { get; private set; }
		
        public string ConnectionString { get; private set; }
		
        public string SslConnectionString { get; private set; }
		
        public IEnumerable<string> ProjectPaths { get; private set; }

        private static string GetInitializationDataScript(Guid requestId, string appName, string connectionString)
        {            
            var htmlFormat =
@"
<script type='application/json' id='__browserLink_initializationData'>
    {{""appName"":""{0}"", ""requestId"":""{1}""}}  
</script>
<script type=""text/javascript"" src=""{2}"" async=""async""></script>
";
            return string.Format(htmlFormat, appName, requestId.ToString("N"), connectionString);            
        }


        internal  bool SignalArteryForStartup()
        {
            if (RequestSignalName != null && ReadySignalName != null)
            {
                SendSignal(RequestSignalName);
                return WaitForSignal(ReadySignalName);
            }
            return false;
        }
        private static void SendSignal(string signalName)
        {
            // TODO Make safer. May throw.
            try
            {
                using (var eventWaitHandle = EventWaitHandle.OpenExisting(signalName, EventWaitHandleRights.Modify))
                {
                    eventWaitHandle.Set();
                }                      
            }
            catch (Exception)
            {
            }
            
        }
        private static bool WaitForSignal(string signalName)
        {
            bool ret = true;
            try
            {
                using (var eventWaitHandle = EventWaitHandle.OpenExisting(signalName, EventWaitHandleRights.Synchronize))
                {
                    ret = eventWaitHandle.WaitOne(ArteryStartupTimeout);
                }
            }
            catch (Exception)
            {
            }            
            return ret;          
        }

    }
}
