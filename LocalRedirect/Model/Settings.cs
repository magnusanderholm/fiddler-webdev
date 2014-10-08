namespace Fiddler.LocalRedirect.Config
{
    using Fiddler.LocalRedirect.Model;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.Serialization;

    [DataContract(Name = "settings", Namespace = "")]
    public partial class Settings : IEnumerable<UrlRule>
    {
        private ICollection<UrlRule> urlRules;
        protected NotifyPropertyChanged pC;

        public Settings()
        {
            Initialize();
        }

        /// <remarks/>
        [DataMember(Name = "urlrules", IsRequired=false)]
        public ICollection<UrlRule> UrlRules
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
        }

        [OnDeserializing]
        private void DeserializationInitializer(StreamingContext ctx)
        {
            this.Initialize();
        }
    }
}
