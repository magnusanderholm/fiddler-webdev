namespace Fiddler.LocalRedirect.Model
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    [DataContract(Name="setting", Namespace="")]
    [KnownType(typeof(UrlRule))]
    [KnownType(typeof(Redirect))]
    [KnownType(typeof(BrowserLink))]
    [KnownType(typeof(HeaderScript))]
    [KnownType(typeof(ForceUnminified))]
    [KnownType(typeof(JavascriptCombiner))]
    [KnownType(typeof(CSSCombiner))]
    [KnownType(typeof(ForceSharepointDebugJavascript))]
    [KnownType(typeof(OverrideDNS))]    
    public abstract class Setting  : INotifyPropertyChanged
    {
        private bool isEnabled;
        public NotifyPropertyChanged pC;
        
        public Setting()
        {
            Initialize();
        }

        
        /// <remarks/>
        [DataMember(Name = "isenabled", IsRequired = false), DefaultValue(false)]
        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { pC.Update(ref isEnabled, value); }            
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs pCe)
        {
            var h = PropertyChanged;
            if (h != null)
                h(this, pCe);
        }        

        private void Initialize()
        {
            pC = new NotifyPropertyChanged(OnPropertyChanged);
            isEnabled = false;
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
            this.pC.Enabled = true;
        }
    }
}
