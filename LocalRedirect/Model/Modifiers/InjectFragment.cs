namespace Fiddler.LocalRedirect.Model
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using System.Linq;

    [DataContract(Name = "injectfragment", Namespace = "")]
    [Modifier(Order = 6, IsEnabled = true)] 
    [Serializable()]
    [XmlRoot(Namespace = "", ElementName = "injectfragment")]
    public class InjectFragment : Modifier
    {
        private string path;
        private string htmlSelector;
        private const string defaultHtmlSelector = "/html/head";

        private InjectFragment()
            : this(null)
        {            
        }

        public InjectFragment(UrlRule parent)
            :base(parent)
        {
            path = null;
            htmlSelector = defaultHtmlSelector;
        }

        public InjectFragment(UrlRule parent, FileInfo fI)
            : this(parent)
        {                        
            path = fI.FullName;
        }

        [DataMember(Name = "path", IsRequired = false, EmitDefaultValue = false), DefaultValue(null)]
        [XmlAttribute(AttributeName = "path")]
        public string Path
        {
            get 
            { 
                return this.path; 
            }
            set 
            {
                var val = (value ?? string.Empty).Trim();
                if (val != string.Empty && !System.IO.Path.IsPathRooted(val))
                    throw new ArgumentException("Not an absolute path", "value");

                if (val != string.Empty)
                {
                    var file = new FileInfo(value);
                }
                
                pC.Update(ref path, val).Extra("HasScript", "HtmlFragment"); 
            }
        }

        [DataMember(Name = "htmlselector", IsRequired = false, EmitDefaultValue = true), DefaultValue(defaultHtmlSelector)]
        [XmlAttribute(AttributeName = "htmlselector")]
        public string HtmlSelector
        {
            get { return this.htmlSelector; }
            set  { pC.Update(ref htmlSelector, value); }
        }

        [XmlIgnore()]
        public bool HasScript
        {
            get { return Path != null &&  File.Exists(Path); }
        }
        
        [XmlIgnore()]
        public string HtmlFragment
        {
            get
            {
                // TODO Use cache to reread file at regular intervals instead.
                return HasScript
                    ? File.ReadAllText(Path)
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
    }
}
