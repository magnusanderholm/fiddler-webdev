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

    [DataContract(Name = "urlrule", Namespace = "")]
    public class UrlRule : Setting
    {
        private ObservableCollection<ChildSetting> children;
        private string urlString;
        private Settings parent;
        private static readonly StreamingContext emptyStreamingContext = new StreamingContext();
        private string color;

        private UrlRule()
            : this(null)
        {            
        }

        public UrlRule(Settings settings)
            : base()
        {
            OnInitializing(emptyStreamingContext);
            Parent = settings;            
            OnInitialized(emptyStreamingContext);            
        }
        
        public Settings Parent
        {
            get { return parent; }
            set { pC.Update(ref parent, value); }
        }

        [DataMember(Name = "children", IsRequired = true)]
        public ObservableCollection<ChildSetting> Children {
            get { return this.children;}            
        }

        [DataMember(Name = "url", IsRequired = true)]
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



        public bool IsValid { get { return UrlString != null; } }

        
        public override void RequestBefore(Session session)
        {
            base.RequestBefore(session);
            if (IsEnabled)            
                session.oFlags["ui-backcolor"] = HtmlColor;            
        }
        
        [OnDeserializing]
        private void OnInitializing(StreamingContext ctx)
        {
            urlString = string.Empty;
            children = new ObservableCollection<ChildSetting>();
            color = null;         
        }

        [OnDeserialized]
        private void OnInitialized(StreamingContext ctx)
        {
            foreach (var c in children)            
                c.Parent = this;
            children.CollectionChanged += OnChildrenCollectionChanged;
        }

        private void OnChildrenCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (ChildSetting c in e.NewItems)
                    c.Parent = this;
            }
        }        
    }
}
