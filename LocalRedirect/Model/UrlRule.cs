namespace Fiddler.LocalRedirect.Config
{
    using Fiddler.LocalRedirect.Model;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Runtime.Serialization;

    [DataContract(Name = "urlrule", Namespace = "")]
    public class UrlRule : Setting
    {
        private ObservableCollection<ChildSetting> children;
        private string url;
        private Settings parent;
        private static readonly StreamingContext emptyStreamingContext = new StreamingContext();

        public UrlRule()
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
        

        private void Initialize()
        {
            
        }

        [OnDeserializing]
        private void OnInitializing(StreamingContext ctx)
        {
            url = "";
            children = new ObservableCollection<ChildSetting>();                        
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
