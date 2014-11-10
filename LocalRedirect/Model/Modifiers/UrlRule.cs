namespace Fiddler.LocalRedirect.Model
{
    using Fiddler.LocalRedirect.Model;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract(Name = "urlrule", Namespace = "")]
    [Serializable()]
    [XmlRoot(Namespace = "", ElementName="urlrule")]
    public class UrlRule : ModifierBase
    {
        private ObservableCollection<Modifier> children;
        private string urlString;
        private Settings parent;                
        private string color;

        private UrlRule()
            : this(null)
        {            
        }

        public UrlRule(Settings settings)
            : base()
        {
            urlString = string.Empty;            
            children = new SortedObservableCollection<Modifier>();
            children.CollectionChanged += OnChildrenCollectionChanged;
            color = null;         
            Parent = settings;
        }
        
        [XmlIgnore()]
        public Settings Parent
        {
            get { return parent; }
            set  { pC.Update(ref parent, value);}
        }

        [DataMember(Name = "modifiers", IsRequired = true)]
        [XmlArray("modifiers", IsNullable = false), XmlArrayItem(IsNullable=false,ElementName="modifier", Type=typeof(Modifier))]
        public ObservableCollection<Modifier> Modifiers {
            get { return this.children;}            
        }

        [DataMember(Name = "url", IsRequired = true)]
        [XmlAttribute(AttributeName = "url")]
        public string UrlString {
            get { return this.urlString;}
            set 
            {                
                var val = value;
                if (!string.IsNullOrEmpty(val))
                {
                    val = new Uri(value, UriKind.Absolute).ToString();
                    if (!(val.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase) || 
                        val.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase)))
                        throw new FormatException("Url must start with http:// or https://");
                }

                pC.Update(ref urlString, val);
                if (pC.IsChanged)
                {
                    Url = new Uri(urlString, UriKind.Absolute);
                    pC.Extra("Url");
                }
            }
        }

        [XmlIgnore()]
        public Uri Url { get; private set; }

        public bool IsMatch(Uri sessionUrl, bool isHttpConnect)
        {
            return 
                IsValid && IsEnabled && 
                (!isHttpConnect
                    ? sessionUrl.IsPartialMatch(Url)
                    : Url.Scheme == "https" && sessionUrl.Scheme == "http" && string.Compare(
                        Url.GetComponents(UriComponents.HostAndPort, UriFormat.SafeUnescaped),
                        sessionUrl.GetComponents(UriComponents.HostAndPort, UriFormat.Unescaped), true) == 0);
        }

        

        [DataMember(Name = "color", IsRequired = false, EmitDefaultValue=false), DefaultValue(null)]
        [XmlAttribute(AttributeName = "color")]
        public string HtmlColor
        {
            get { return this.color; }
            set
            {
                var val = value;

                if (val != null)
                {
                    // Ensure we have valid colors.
                    var tmp = ColorTranslator.FromHtml(val);
                    val = ColorTranslator.ToHtml(tmp);
                }

                pC.Update(ref color, val).Extra("Color");
            }
        }
        
        [XmlIgnore()]
        public Color Color
        {
            get { return ColorTranslator.FromHtml(HtmlColor); }
            set
            {
                HtmlColor = value != null
                    ? ColorTranslator.ToHtml(value)
                    : null;                                                           
            }
        }


        [XmlIgnore()]
        public bool IsValid { get { return !string.IsNullOrWhiteSpace(UrlString); } }

        
        public override void RequestBefore(Session session)
        {
            base.RequestBefore(session);
            if (IsEnabled)            
                session.oFlags["ui-backcolor"] = HtmlColor;            
        }
        
        private void OnChildrenCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Modifier c in e.NewItems)
                    c.Parent = this;
            }
        }        
    }
}
