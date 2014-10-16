namespace Fiddler.LocalRedirect.Model
{
    using System;
    using System.ComponentModel;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using System.Linq;

    [DataContract(Name = "browserlink", Namespace = "")]
    [Modifier(Order=0, IsEnabled=true)]
    [Serializable()]
    [XmlRoot(Namespace = "", ElementName = "browserlink")]
    public class BrowserLink : Modifier
    {
        private string visualStudioProjectPath;
        
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
                if (browserLinkConnection != null)
                {
                    session.utilDecodeResponse();
                    var htmlDoc = new HtmlAgilityPack.HtmlDocument()
                    {
                        OptionCheckSyntax = false,
                        OptionOutputOriginalCase = true
                    };

                    htmlDoc.LoadHtml(session.GetResponseBodyAsString());
                    browserLinkConnection.Attach(htmlDoc);
                    //var head = htmlDoc.DocumentNode.SelectSingleNode("/html/body");
                    //if (head != null)
                    //{
                    //    head.AppendChild(HtmlAgilityPack.HtmlNode.CreateNode("<!-- Injected by fiddler -->"));
                    //    head.AppendChild(HtmlAgilityPack.HtmlNode.CreateNode(browserLinkConnection.HtmlScript));
                    //}
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
