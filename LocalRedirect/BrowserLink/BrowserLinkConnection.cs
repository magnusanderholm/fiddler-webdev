namespace Fiddler.LocalRedirect.BrowserLink
{
    using Fiddler.LocalRedirect.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Security.AccessControl;
    using System.Text;
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

        public string GetHtmlScript(bool isHttps, string browser="Chrome")
        {
            return GetInitializationDataScript(
                Guid.NewGuid().ToString("N"), 
                browser, 
                isHttps ? SslConnectionString : ConnectionString);
        }

        public void Attach(HtmlAgilityPack.HtmlDocument doc)
        {            
            //SignalArteryForStartup();
        }

        public string RequestSignalName { get; private set; }
		
        public string ReadySignalName { get; private set; }
		
        public string ConnectionString { get; private set; }
		
        public string SslConnectionString { get; private set; }
		
        public IEnumerable<string> ProjectPaths { get; private set; }

//        public string HtmlScript { get; private set; }




        private static string GetInitializationDataScript(string requestId, string appName, string connectionString)
        {            
            var htmlFormat =
@"
<script type='application/json' id='__browserLink_initializationData'>
    {{""appName"":""{0}"", ""requestId"":""{1}""}}  
</script>
<script type=""text/javascript"" src=""{2}"" async=""async""></script>
";
            return string.Format(htmlFormat, appName, requestId, connectionString);            
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
