namespace Fiddler.LocalRedirect.Config
{
    using Fiddler.LocalRedirect.Model;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.Serialization;

    [DataContract(Name = "settings", Namespace = "")]
    public partial class Settings : IEnumerable<UrlRule>
    {
        private ObservableItemCollection<UrlRule> urlRules;
        public NotifyPropertyChanged pC;

        public Settings()
        {
            Initialize();            
        }

        public UrlRule CreateUrlRule()
        {
            var rule = new UrlRule(this);
            rule.Children.Add(new Redirect(rule, "localhost:80", false));
            rule.Children.Add(new ForceUnminified(rule));
            rule.Children.Add(new ForceSharepointDebugJavascript(rule));
            rule.Children.Add(new HeaderScript(rule));
            rule.Children.Add(new BrowserLink(rule));
            rule.Children.Add(new JavascriptCombiner(rule));
            rule.Children.Add(new CSSCombiner(rule));            
            return rule;
        }

        /// <remarks/>
        [DataMember(Name = "urlrules", IsRequired=false)]
        public ObservableItemCollection<UrlRule> UrlRules
        {
            get { return this.urlRules; }
            set { pC.Update(ref urlRules, value); }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs pCe)
        {            
            var h = PropertyChanged;
            if (h != null)
                h(this, pCe);
        }

        public static Settings CreateDefault()
        {
            var settings = new Settings();
            return settings;
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
            urlRules = new ObservableItemCollection<UrlRule>();
            pC.Register(urlRules); 
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
            foreach (var urlRule in urlRules)
                urlRule.Parent = this;
            this.pC.Enabled = true;
        }
    }
}
