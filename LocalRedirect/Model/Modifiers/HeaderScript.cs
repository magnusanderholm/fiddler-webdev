namespace Fiddler.LocalRedirect.Model
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract(Name = "headerscript", Namespace = "")]
    [Modifier(Order = 3, IsEnabled = true)] 
    [Serializable()]
    [XmlRoot(Namespace = "", ElementName = "headerscript")]
    public class HeaderScript : ChildSetting
    {
        private string htmlFragmentPath;

        private HeaderScript()
            : this(null)
        {            
        }

        public HeaderScript(UrlRule parent)
            :base(parent)
        {
            Initialize();
        }

        public HeaderScript(UrlRule parent, FileInfo fI)
            : this(parent)
        {                        
            htmlFragmentPath = fI.FullName;
        }

        [DataMember(Name = "htmlfragmentpath", IsRequired = false, EmitDefaultValue=false), DefaultValue(null)]
        [XmlAttribute(AttributeName = "htmlfragmentpath")]
        public string HtmlFragmentPath
        {
            get 
            { 
                return this.htmlFragmentPath; 
            }
            set 
            {
                var val = value;
                if (value == null)
                {
                    // Make sure that the path looks valid (not that it exists). Also convert
                    // to a absolute path
                    var tmp = new FileInfo(value);
                    val = tmp.FullName;
                }
                pC.Update(ref htmlFragmentPath, val).Extra("HasScript", "HtmlFragment"); 
            }
        }
        [XmlIgnore()]
        public bool HasScript
        {
            get { return HtmlFragmentPath != null &&  File.Exists(HtmlFragmentPath); }
        }
        
        [XmlIgnore()]
        public string HtmlFragment
        {
            get
            {
                // TODO Use cache to reread file at regular intervals instead.
                return HasScript
                    ? File.ReadAllText(HtmlFragmentPath)
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
            if (IsEnabled & HasScript && session.oResponse.headers.ExistsAndContains("Content-Type", "text/html"))
            {
                // TODO UseHtmlAgility pack for this
                session.utilDecodeResponse();
                session.utilReplaceInResponse("</head>", "<!-- Injected by fiddler -->" + HtmlFragment + "</head>");
            }
        }

        private void Initialize()
        {
            htmlFragmentPath = null;            
        }

        [OnDeserializing]
        private void DeserializationInitializer(StreamingContext ctx)
        {
            this.Initialize();
        }
    }
}
