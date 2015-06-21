namespace Fiddler.Webdev.Model
{
    using System;
    using System.ComponentModel;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using System.Linq;

    [DataContract(Name = "browserlink", Namespace = "")]
    [Modifier(Order=7, IsEnabled=true)]
    [Serializable()]
    [XmlRoot(Namespace = "", ElementName = "browserlink")]
    public class BrowserLink : Modifier
    {
        private string visualStudioProjectPath;
        private static readonly ILogger logger = LogManager.CreateCurrentClassLogger();
        
        private BrowserLink()
            : this(null)
        {            
        }

        public BrowserLink(UrlRule parent)
            : base(parent)
        {
            visualStudioProjectPath = "";
        }

        [DataMember(Name = "visualstudioprojectpath", IsRequired = false), DefaultValue("")]
        [XmlAttribute(AttributeName = "visualstudioprojectpath")]
        public string VisualStudioProjectPath
        {
            get { return this.visualStudioProjectPath; }
            set { pC.Update(ref visualStudioProjectPath, value); }
        }

        public override void RequestBefore(Session session)
        {
            base.RequestBefore(session);
            if (Parent.IsEnabled && IsEnabled /*&& this.HasScript*/)
            {
                // In order to be able to inject data in the response we need to buffer it.
                session.bBufferResponse = true;
            }
        }

        public override void ResponseBefore(Session session)
        {
            base.ResponseBefore(session);            
            if (IsEnabled && session.oResponse.headers.ExistsAndContains("Content-Type", "text/html"))
            {
                
                // TODO Return the browser link config that we actually think we will use.... Perhaps it is easier
                //      to let users make a manual choice in the UI!!!!
                // TODO Sometimes we get back config with empty connection strings. Need to log this to tell
                //      users to start a corresponding website in debug to initialize browserlink in VS.
                var browserLinkConnection = new Fiddler.Webdev.BrowserLink.BrowserLinkConfiguration()
                    .GetAllBrowserLinkConnections()
                    .FirstOrDefault() ;
                if (browserLinkConnection == null)
                    logger.Debug( () => "No browserlink connection found.");
                else
                {                    
                    session.utilDecodeResponse();
                    var htmlDoc = new HtmlAgilityPack.HtmlDocument()
                    {
                        OptionCheckSyntax = false,
                        OptionOutputOriginalCase = true
                    };

                    htmlDoc.LoadHtml(session.GetResponseBodyAsString());
                    var bodyTag = htmlDoc.DocumentNode.SelectSingleNode("/html/body");
                    if (bodyTag != null)
                    {
                        var userAgent = session.oRequest.headers["User-Agent"] ?? string.Empty;
                        var browserId = new Fiddler.Webdev.BrowserLink.BrowserIdentifier(userAgent);
                        var nodes = HtmlAgilityPack.HtmlTextNode.CreateNode(
                            "<div>" + browserLinkConnection.GetHtmlScript(new Uri(session.fullUrl), browserId) + "</div>").ChildNodes;
                        bodyTag.AppendChildren(nodes);
                        session.utilSetResponseBody(htmlDoc.DocumentNode.OuterHtml);
                    }                                                                                                   
                }                
            } 
        }                
    }
}
