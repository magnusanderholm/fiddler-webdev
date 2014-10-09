namespace Fiddler.LocalRedirect.Config
{
    using Fiddler.LocalRedirect.Model;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Runtime.Serialization;

    [DataContract(Name = "urlrule", Namespace = "")]
    public class UrlRule : Setting, IEnumerable<ChildSetting>
    {
        private ObservableCollection<ChildSetting> children;
        private string url;
        private Settings parent;

        public UrlRule()
            : this(null)
        {         
        }

        public UrlRule(Settings settings)
            : base()
        {
            Initialize();
            Parent = settings;
        }
        
        public Settings Parent
        {
            get { return parent; }
            set
            {
                pC.Update(ref parent, value);
                pC.Parent = parent != null
                 ? parent.pC
                 : null;
            }
        }

        [DataMember(Name = "children", IsRequired = true)]
        public ObservableCollection<ChildSetting> Children {
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
            pC.Register(children);
        }

        [OnDeserializing]
        private void OnDeserializing(StreamingContext ctx)
        {
            this.Initialize();
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext ctx)
        {
            foreach (var c in children)
                c.Parent = this;
        }
    }
}
