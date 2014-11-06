namespace Fiddler.LocalRedirect.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract(Name = "settings", Namespace = "")]
    [Serializable()]
    [XmlRoot(Namespace = "", ElementName = "settings")]
    public partial class Settings : INotifyPropertyChanged
    {
        private ObservableCollection<UrlRule> urlRules;
        private NotifyPropertyChanged pC;        
        private static readonly StreamingContext emptyStreamingContext = new StreamingContext();
        private static readonly IEventBus eventBus = EventBusManager.Get();
        

        public Settings()
        {            
            OnInitializing(emptyStreamingContext);
            OnInitialized(emptyStreamingContext);
        }
        

        // TODO Put in extension method of ObservableCollection<UrlRule>
        public void ReplaceUrlRulesWith(IEnumerable<UrlRule> rules)
        {
            urlRules.Clear();
            foreach (var r in rules)            
                urlRules.Add(r);            
        }

        [XmlIgnore()]
        public ModifierFactory UrlRuleFactory { get; private set;}       
                
        /// <remarks/>
        [DataMember(Name = "urlrules", IsRequired=false)]
        [XmlArray(ElementName = "urlrules"), XmlArrayItem(ElementName="urlrule",IsNullable=false, Type= typeof(UrlRule))]
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
            UrlRuleFactory = new ModifierFactory(this);
            pC = new NotifyPropertyChanged(OnPropertyChanged);
            urlRules = new ObservableCollection<UrlRule>();
            this.pC.Enabled = false;
        }

        [OnDeserialized]
        private void OnInitialized(StreamingContext ctx)
        {
            // When we get here the children has not yet been initialized. They will
            // be deserialized later on. And added to this object using the normal methods
            // ie property setters and collection add.

            // However we do NOT want to allow them to send events on the eventBus
            // until the entire hiearchy has been deserialized.

            // A. Before serialization we need to ensure that eventbus and propertynotifier is turned off
            // B. After serialization is complete we need to ensure that 
            
            this.pC.Enabled = true;
            urlRules.CollectionChanged += OnUrlRulesCollectionChanged;
            
            this.PublishPropertyChangedOnEventBus(eventBus);            
            urlRules.PublishCollectionChangedOnEventBus(eventBus);
            urlRules.PublishPropertyChangedOnEventBus(eventBus);
        }

        private void OnUrlRulesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (UrlRule r in e.NewItems)
                {
                    r.Parent = this;
                    r.PublishPropertyChangedOnEventBus(eventBus);                    
                }
            }
        }        
    }
}
