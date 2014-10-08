namespace Fiddler.LocalRedirect.Config
{
    using Fiddler.LocalRedirect.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    [DataContract(Name = "urlrule", Namespace = "")]
    public class UrlRule : Setting, IEnumerable<ChildSetting>
    {
        private ICollection<ChildSetting> children;
        private string url;

        public UrlRule()
        {
            Initialize();
        }
        public static UrlRule CreateDefault()
        {
            var rule = new UrlRule();
            var children = new List<ChildSetting>();
            children.Add(new Redirect(rule, "localhost:80", false));
            children.Add(new ForceUnminified(rule)); 
            children.Add(new HeaderScript(rule));
            children.Add(new BrowserLink(rule));
            children.Add(new JavascriptCombiner(rule));
            children.Add(new CSSCombiner(rule));
            rule.Children = children.ToArray();
            return rule;
        }

        [DataMember(Name = "children", IsRequired = true)]
        public ICollection<ChildSetting> Children {
            get { return this.children;}
            set { pC.Update(ref children, value);}
        }

        [DataMember(Name = "url", IsRequired = true)]
        public string Url {
            get { return this.url;}
            set { pC.Update(ref url, value).Extra("Scheme");}
        }

        
        public string Scheme
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Url)
                  ? new Uri(Url).Scheme.ToUpper()
                  : "NONE";
            }
        }        

        public IEnumerator<ChildSetting> GetEnumerator()
        {
            return Children.AsEnumerable().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void Initialize()
        {
            url = "";
            children = new ObservableItemCollection<ChildSetting>();
        }

        [OnDeserializing]
        private void DeserializationInitializer(StreamingContext ctx)
        {
            this.Initialize();
        }
    }
}
