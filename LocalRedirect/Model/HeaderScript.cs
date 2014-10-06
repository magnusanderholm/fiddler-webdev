namespace Fiddler.LocalRedirect.Config
{
    using System;
    using System.IO;
    using System.Xml.Serialization;
    
    public partial class HeaderScript : ChildSetting
    {
        private string headerScriptPath = string.Empty;

        [XmlIgnore()]
        public bool HasScript
        {
            get
            {                
                bool hasHeaderScript = false;
                if (!string.IsNullOrWhiteSpace(Path))
                {
                    var headerScriptFile = new FileInfo(Path);
                    hasHeaderScript = headerScriptFile.Exists && headerScriptFile.Length > 0;
                }

                return hasHeaderScript;
            }
        }

        [XmlIgnore()]
        public string Script
        {
            get
            {
                // TODO Use cache to reread file at regular intervals instead.
                return HasScript
                    ? File.ReadAllText(new FileInfo(Path).FullName)
                    : String.Empty;
            }
        }

        public override void RequestBefore(Session session)
        {
            base.RequestBefore(session);
            if (IsEnabled && HasScript)
            {
                // In order to be able to inject data in the response we need to buffer it.
                session.bBufferResponse = true;
            }
        }

        public override void ResponseBefore(Session session)
        {
            base.ResponseBefore(session);
            if (IsEnabled && HasScript && session.oResponse.headers.ExistsAndContains("Content-Type", "text/html"))
            {
                // TODO UseHtmlAgility pack for this
                session.utilDecodeResponse();
                session.utilReplaceInResponse("</head>", "<!-- Injected by fiddler -->" + Script + "</head>");
            }
        }
    }
}
