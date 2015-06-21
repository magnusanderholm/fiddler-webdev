namespace Fiddler.Webdev.Model
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
    public class Settings : INotifyPropertyChanged
    {
        private ObservableCollection<UrlRule> urlRules;
        private NotifyPropertyChanged pC;        
                        
        public Settings()
        {            
            UrlRuleFactory = new ModifierFactory(this);
            pC = new NotifyPropertyChanged(OnPropertyChanged);
            urlRules = new ObservableCollection<UrlRule>();
            urlRules.CollectionChanged += OnUrlRulesCollectionChanged;            
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

        private void OnUrlRulesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (UrlRule r in e.NewItems)
                    r.Parent = this;
            }
        }

        public IEnumerable<object> AsFlattenedEnumerable()
        {                
            yield return this;
            yield return UrlRules;
            foreach (var u in UrlRules)
            {
                yield return u;
                yield return u.Modifiers;
                foreach (var m in u.Modifiers)                
                    yield return m;                
            }                                     
        }        
    }
}
