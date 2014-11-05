namespace Fiddler.LocalRedirect.Model
{
    using System;
    using System.ComponentModel;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [Serializable()]    
    [XmlRoot(Namespace = "", ElementName = "modifierbase")]        
    [DataContract(Name = "modifierbase", Namespace = "")]
    [KnownType(typeof(UrlRule)), XmlInclude(typeof(UrlRule))]
    [KnownType(typeof(Redirect)), XmlInclude(typeof(Redirect))]
    [KnownType(typeof(BrowserLink)), XmlInclude(typeof(BrowserLink))]
    [KnownType(typeof(InjectFragment)), XmlInclude(typeof(InjectFragment))]
    [KnownType(typeof(ForceUnminified)), XmlInclude(typeof(ForceUnminified))]
    [KnownType(typeof(JavascriptCombiner)), XmlInclude(typeof(JavascriptCombiner))]
    [KnownType(typeof(CSSCombiner)), XmlInclude(typeof(CSSCombiner))]
    [KnownType(typeof(ForceSharepointDebugJavascript)), XmlInclude(typeof(ForceSharepointDebugJavascript))]
    [KnownType(typeof(OverrideDNS)), XmlInclude(typeof(OverrideDNS))]
    [KnownType(typeof(DisableCache)), XmlInclude(typeof(DisableCache))]
    [KnownType(typeof(FileResponse)), XmlInclude(typeof(FileResponse))]
    [KnownType(typeof(Modifier)), XmlInclude(typeof(Modifier))]
    public abstract class ModifierBase : INotifyPropertyChanged, ISessionModifier
    {
        private bool isEnabled;
        protected NotifyPropertyChanged pC;
        protected static readonly IEventBus eventBus = EventBusManager.Get();
        private static readonly StreamingContext emptyStreamingContext = new StreamingContext();
        
        public ModifierBase()
        {
            OnDeserializing(emptyStreamingContext);
            OnDeserialized(emptyStreamingContext);
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

        [OnDeserializing]
        private void OnDeserializing(StreamingContext ctx)
        {
            pC = new NotifyPropertyChanged(OnPropertyChanged);
            this.pC.Enabled = false;            
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext ctx)
        {
            this.pC.Enabled = true;
            this.PublishPropertyChangedOnEventBus(eventBus);
        }
    }
}
