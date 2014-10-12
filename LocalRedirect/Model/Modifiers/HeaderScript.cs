namespace Fiddler.LocalRedirect.Model
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract(Name = "headerscript", Namespace = "")]
    [Modifier(Order = 3, IsEnabled = true)] 
    public class HeaderScript : ChildSetting
    {
        private string htmlFragmentPath;

        private HeaderScript()
        {
            Initialize();            
        }

        public HeaderScript(UrlRule parent)
            :base(parent)
        {
            htmlFragmentPath = string.Empty;
        }

        public HeaderScript(UrlRule parent, FileInfo fI)
            : this(parent)
        {                        
            htmlFragmentPath = fI.FullName;
        }

        [DataMember(Name = "htmlfragmentpath", IsRequired = false), DefaultValue("")]
        public string HtmlFragmentPath
        {
            get { return this.htmlFragmentPath; }
            set { pC.Update(ref htmlFragmentPath, value).Extra("HasScript", "HtmlFragment"); }
        }
        
        public bool HasScript
        {
            get
            {                
                bool hasHeaderScript = false;
                if (!string.IsNullOrWhiteSpace(HtmlFragmentPath))
                {
                    var headerScriptFile = new FileInfo(HtmlFragmentPath);
                    hasHeaderScript = headerScriptFile.Exists && headerScriptFile.Length > 0;
                }

                return hasHeaderScript;
            }
        }
        
        public string HtmlFragment
        {
            get
            {
                // TODO Use cache to reread file at regular intervals instead.
                return HasScript
                    ? File.ReadAllText(new FileInfo(HtmlFragmentPath).FullName)
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
                session.utilReplaceInResponse("</head>", "<!-- Injected by fiddler -->" + HtmlFragment + "</head>");
            }
        }

        private void Initialize()
        {
            htmlFragmentPath = string.Empty;            
        }

        [OnDeserializing]
        private void DeserializationInitializer(StreamingContext ctx)
        {
            this.Initialize();
        }
    }
}
