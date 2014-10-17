namespace Fiddler.LocalRedirect.Model
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
        {
            Initialize();
        }

        public BrowserLink(UrlRule parent)
            : base(parent)
        {
            Initialize();
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
            if (IsEnabled)
            {
                var browserLinkConnection = new Fiddler.LocalRedirect.BrowserLink.BrowserLinkConfiguration()
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

                    var nodes = HtmlAgilityPack.HtmlTextNode.CreateNode(
                        "<div>" + browserLinkConnection.GetHtmlScript(session.isHTTPS) + "</div>").ChildNodes;
                    htmlDoc.DocumentNode.SelectSingleNode("/html/body").AppendChildren(nodes);
                    
                    session.utilSetResponseBody(htmlDoc.DocumentNode.OuterHtml);
                }                
            } 
        }        

        private void Initialize()
        {
            visualStudioProjectPath = "";
        }

        [OnDeserializing]
        private void DeserializationInitializer(StreamingContext ctx)
        {
            this.Initialize();
        }
    }
}
