namespace Fiddler.LocalRedirect.Model
{
    using System;
    using System.ComponentModel;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract(Name = "browserlink", Namespace = "")]
    [Modifier(Order=0, IsEnabled=false)]
    [Serializable()]
    [XmlRoot(Namespace = "", ElementName = "browserlink")]
    public class BrowserLink : Modifier
    {
        private string visualStudioProjectPath;
        
        private BrowserLink()
        {
            Initialize();
        }

        public BrowserLink(UrlRule parent)
            : base(parent)
        {
            Initialize();
        }

        [DataMember(Name = "visualstudioprojectpath", IsRequired = false), DefaultValue("")]
        [XmlAttribute(AttributeName = "visualstudioprojectpath")]
        public string VisualStudioProjectPath
        {
            get { return this.visualStudioProjectPath; }
            set { pC.Update(ref visualStudioProjectPath, value); }
        }

        public override void RequestBefore(Session session)
        {
            base.RequestBefore(session);
            if (Parent.IsEnabled && IsEnabled /*&& this.HasScript*/)
            {
                // In order to be able to inject data in the response we need to buffer it.
                session.bBufferResponse = true;
            }
        }

        public override void ResponseBefore(Session session)
        {
            base.ResponseBefore(session);
            if (IsEnabled)
            {
                // Enable browser link by injecting corresponding scripts. Don't think we need to cache
                // anything as browserlink reads its config via memory mapped files.
                // NOTE Could be that we do not really need to read the browserlink configuration. What if we do a 
                //      web request to the site in question instead? Then we'd get a page where the scripts are already
                //      injected and we can just copy and paste that into all other pages... May have to modifiy guids and so on...
            } 
        }        

        private void Initialize()
        {
            visualStudioProjectPath = "";
        }

        [OnDeserializing]
        private void DeserializationInitializer(StreamingContext ctx)
        {
            this.Initialize();
        }
    }
}
