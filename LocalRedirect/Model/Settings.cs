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
    public partial class Settings : IEnumerable<UrlRule>, INotifyPropertyChanged
    {
        private ObservableCollection<UrlRule> urlRules;
        private NotifyPropertyChanged pC;
        private ObserveChange changeObserver;

        public Settings()
        {
            Initialize();
            changeObserver.Observe(urlRules);            
            changeObserver.Observe(this);
        }

        public IChange Observer
        {
            get { return changeObserver; }
        }

        public UrlRule AddUrlRule()
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
            ObserveRuleAndChildrenForChanges(rule);

            return rule;
        }

        public void ClearUrlRules()
        {
           urlRules.Clear();
        }

        // TODO Can't we fallback on the class IEnumerable inteface instead????
        //      Would look a lot cleaner

        /// <remarks/>
        [DataMember(Name = "urlrules", IsRequired=false)]
        public IEnumerable<UrlRule> UrlRules
        {
            get { return this.urlRules; }
            //set { pC.Update(ref urlRules, value); }
            set { } // Needed for serialization purposes. Don't call
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs pCe)
        {            
            var h = PropertyChanged;
            if (h != null)
                h(this, pCe);
        }        
        

        public IEnumerator<UrlRule> GetEnumerator()
        {
            return UrlRules.AsEnumerable().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void Initialize()
        {
            pC = new NotifyPropertyChanged(OnPropertyChanged);
            urlRules = new ObservableCollection<UrlRule>();
            changeObserver = new ObserveChange();                        
        }
        

        [OnDeserializing]
        private void OnDeserializing(StreamingContext ctx)
        {
            this.Initialize();
            this.pC.Enabled = false;
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext ctx)
        {
            foreach (var urlRule in urlRules) { 
                urlRule.Parent = this;
                ObserveRuleAndChildrenForChanges(urlRule);
            }

            changeObserver.Observe(urlRules);
            changeObserver.Observe(this);
            this.pC.Enabled = true;            
        }

        private void ObserveRuleAndChildrenForChanges(UrlRule rule)
        {
            changeObserver.Observe(rule);
            changeObserver.Observe(rule.Children);
            foreach (var c in rule.Children)
                changeObserver.Observe(c);
        }
    }
}
