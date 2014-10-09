using System.ComponentModel;
using System.Runtime.Serialization;
namespace Fiddler.LocalRedirect.Config
{
    [DataContract(Name = "browserlink", Namespace = "")]
    public class BrowserLink : ChildSetting
    {
        private string visualStudioProjectPath;
        
        public BrowserLink()
        {
            Initialize();
        }

        public BrowserLink(UrlRule parent)
            : base(parent)
        {
            Initialize();
        }

        [DataMember(Name = "visualstudioprojectpath", IsRequired = false), DefaultValue("")]
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
