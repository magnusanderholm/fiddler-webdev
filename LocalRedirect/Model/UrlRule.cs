namespace Fiddler.LocalRedirect.Model
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
        public string Url {
            get { return this.url;}
            set 
            {                
                var val = value;
                if (!string.IsNullOrEmpty(val))
                    val = new Uri(value, UriKind.Absolute).ToString();

                pC.Update(ref url, val).Extra("Scheme");
            }
        }

        public bool IsValid { get { return !string.IsNullOrEmpty(Url); } }
        
        public string Scheme
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Url)
                  ? new Uri(Url).Scheme.ToUpper()
                  : "NONE";
            }
        }        
        
        
        [OnDeserializing]
        private void OnInitializing(StreamingContext ctx)
        {
            url = string.Empty;
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
