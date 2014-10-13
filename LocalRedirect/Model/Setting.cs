namespace Fiddler.LocalRedirect.Model
{
    using System;
    using System.ComponentModel;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract(Name="setting", Namespace="")]
    [KnownType(typeof(UrlRule)), XmlInclude(typeof(UrlRule))]
    [KnownType(typeof(Redirect)), XmlInclude(typeof(Redirect))]
    [KnownType(typeof(BrowserLink)), XmlInclude(typeof(BrowserLink))]
    [KnownType(typeof(HeaderScript)), XmlInclude(typeof(HeaderScript))]
    [KnownType(typeof(ForceUnminified)), XmlInclude(typeof(ForceUnminified))]
    [KnownType(typeof(JavascriptCombiner)), XmlInclude(typeof(JavascriptCombiner))]
    [KnownType(typeof(CSSCombiner)), XmlInclude(typeof(CSSCombiner))]
    [KnownType(typeof(ForceSharepointDebugJavascript)), XmlInclude(typeof(ForceSharepointDebugJavascript))]
    [KnownType(typeof(OverrideDNS)), XmlInclude(typeof(OverrideDNS))]
    [KnownType(typeof(DisableCache)), XmlInclude(typeof(DisableCache))]
    [KnownType(typeof(FileResponse)), XmlInclude(typeof(FileResponse))]
    [KnownType(typeof(ChildSetting)), XmlInclude(typeof(ChildSetting))]
    [Serializable()]
    [XmlRoot(Namespace = "", ElementName = "setting")]
    public abstract class Setting : INotifyPropertyChanged, ISessionModifier
    {
        private bool isEnabled;
        protected NotifyPropertyChanged pC;
        
        public Setting()
        {
            Initialize();
        }

        
        /// <remarks/>
        [DataMember(Name = "isenabled", IsRequired = false), DefaultValue(false)]
        [XmlAttribute(AttributeName = "isenabled")]
        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { pC.Update(ref isEnabled, value); }            
        }
        
        
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void PeekAtResponseHeaders(Session session)
        {
        }

        public virtual void RequestAfter(Session session)
        {
        }

        public virtual void RequestBefore(Session session)
        {
        }

        public virtual void ResponseAfter(Session session)
        {
        }

        public virtual void ResponseBefore(Session session)
        {
        }

        public virtual void BeforeReturningError(Session session)
        {
        }


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
