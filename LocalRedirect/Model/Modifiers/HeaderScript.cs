namespace Fiddler.LocalRedirect.Model
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using System.Linq;

    [DataContract(Name = "headerscript", Namespace = "")]
    [Modifier(Order = 3, IsEnabled = true)] 
    [Serializable()]
    [XmlRoot(Namespace = "", ElementName = "headerscript")]
    public class HeaderScript : Modifier
    {
        private string htmlFragmentPath;
        private string htmlSelector;
        private const string defaultHtmlSelector = "/html/head";

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
                if (value != null)
                {
                    // Make sure that the path looks valid (not that it exists). Also convert
                    // to a absolute path
                    var tmp = new FileInfo(value);
                    val = tmp.FullName;
                }
                pC.Update(ref htmlFragmentPath, val).Extra("HasScript", "HtmlFragment"); 
            }
        }

        [DataMember(Name = "htmlselector", IsRequired = false, EmitDefaultValue = true), DefaultValue(defaultHtmlSelector)]
        [XmlAttribute(AttributeName = "htmlselector")]
        public string HtmlSelector
        {
            get { return this.htmlSelector; }
            set { pC.Update(ref htmlFragmentPath, value); }
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
            if (IsEnabled & HasScript && session.oResponse.headers.ExistsAndContains("Content-Type", "text/html") && !string.IsNullOrWhiteSpace(htmlSelector))
            {                
                session.utilDecodeResponse();
                var htmlDoc = new HtmlAgilityPack.HtmlDocument()
                {
                     OptionCheckSyntax = false,
                     OptionOutputOriginalCase = true
                };
   
                htmlDoc.LoadHtml(session.GetResponseBodyAsString());
                var head = htmlDoc.DocumentNode.SelectSingleNode(htmlSelector);
                if (head != null) { 
                    head.AppendChild(HtmlAgilityPack.HtmlNode.CreateNode("<!-- Injected by fiddler -->"));
                    head.AppendChild(HtmlAgilityPack.HtmlNode.CreateNode(HtmlFragment));
                }
                session.utilSetResponseBody(htmlDoc.DocumentNode.OuterHtml);
            }
        }

        private void Initialize()
        {
            htmlFragmentPath = null;
            htmlSelector = defaultHtmlSelector;
        }

        [OnDeserializing]
        private void DeserializationInitializer(StreamingContext ctx)
        {
            this.Initialize();
        }
    }
}
