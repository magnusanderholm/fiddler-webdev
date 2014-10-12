namespace Fiddler.LocalRedirect.Config
{
    using Fiddler.LocalRedirect.Model;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.Serialization;

    [DataContract(Name = "settings", Namespace = "")]
    public partial class Settings : INotifyPropertyChanged
    {
        private ObservableCollection<UrlRule> urlRules;
        private NotifyPropertyChanged pC;
        private ObserveChange changeObserver;
        private static readonly StreamingContext emptyStreamingContext = new StreamingContext();

        public Settings()
        {            
            OnInitializing(emptyStreamingContext);
            OnInitialized(emptyStreamingContext);
        }

        public IChange Observer
        {
            get { return changeObserver; }
        }

        public void ReplaceUrlRulesWith(IEnumerable<UrlRule> rules)
        {
            urlRules.Clear();
            foreach (var r in rules)            
                urlRules.Add(r);            
        }
        
        public UrlRule CreateUrlRule()
        {
            // TODO Create this list by looking at class attributes instead. If we write it 
            // propery we do not need to think about adding new sessionmodifiers. It will just happen automatically
            var rule = new UrlRule(this);
            rule.Children.Add(new Redirect(rule, "localhost:80", false));
            rule.Children.Add(new ForceUnminified(rule));
            rule.Children.Add(new ForceSharepointDebugJavascript(rule));
            rule.Children.Add(new HeaderScript(rule));
            rule.Children.Add(new BrowserLink(rule));
            rule.Children.Add(new JavascriptCombiner(rule));
            rule.Children.Add(new CSSCombiner(rule));                       
            urlRules.Add(rule);           
            return rule;
        }

        public void ClearUrlRules()
        {
           urlRules.Clear();
        }
        
        /// <remarks/>
        [DataMember(Name = "urlrules", IsRequired=false)]
        public ObservableCollection<UrlRule> UrlRules
        {
            get { return this.urlRules; }            
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs pCe)
        {            
            var h = PropertyChanged;
            if (h != null)
                h(this, pCe);
        }        
        
        
        [OnDeserializing]
        private void OnInitializing(StreamingContext ctx)
        {
            pC = new NotifyPropertyChanged(OnPropertyChanged);
            urlRules = new ObservableCollection<UrlRule>();
            changeObserver = new ObserveChange();           
            this.pC.Enabled = false;
        }

        [OnDeserialized]
        private void OnInitialized(StreamingContext ctx)
        {
            foreach (var urlRule in urlRules) { 
                urlRule.Parent = this;
                SetParentAndObserveHiearchyChanges(urlRule);
            }

            changeObserver.Observe(urlRules);
            changeObserver.Observe(this);
            this.pC.Enabled = true;
            urlRules.CollectionChanged += OnUrlRulesCollectionChanged;
        }

        private void OnUrlRulesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (UrlRule r in e.NewItems)
                    SetParentAndObserveHiearchyChanges(r);
            }
        }

        private void SetParentAndObserveHiearchyChanges(UrlRule rule)
        {
            rule.Parent = this;
            changeObserver.Observe(rule);
            changeObserver.Observe(rule.Children);
            foreach (var c in rule.Children)
                changeObserver.Observe(c);
        }
    }
}
