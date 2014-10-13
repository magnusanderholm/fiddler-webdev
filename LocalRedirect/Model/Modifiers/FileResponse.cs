namespace Fiddler.LocalRedirect.Model
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.Serialization;

    [DataContract(Name = "fileresponse", Namespace = "")]
    [Modifier(Order=0, IsEnabled=false)]
    public class FileResponse : ChildSetting
    {
        private string baseDirectory;

        private FileResponse()
        {
            Initialize();
        }

        public FileResponse(UrlRule parent)
            : base(parent)
        {
            Initialize();
        }

        [DataMember(Name = "directorypath", IsRequired = false), DefaultValue("")]
        public string DirectoryPath
        {
            get { return this.baseDirectory; }
            set { pC.Update(ref baseDirectory, value); }
        }

        public override void RequestBefore(Session session)
        {
            base.RequestBefore(session);
            if (Parent.IsEnabled && IsEnabled /*&& this.HasScript*/)
            {
                // In order to be able to inject data in the response we need to buffer it.
                session.bBufferResponse = true;
                // TODO Hmm can we shortcircuit the request here and make sure
                //      we return an answer immediatly??? That way we won't even
                //     need to make the roundtrip to the server.... Look at how AutoResponder does this
                //     right now.
            }
        }

        public override void ResponseBefore(Session session)
        {
            base.ResponseBefore(session);
            if (IsEnabled)
            {
                // TODO Look in locally stored files and simply replace the response with them.
                // TODO If basedirectory does not exist. Log it.
                var path = new Uri(session.PathAndQuery).LocalPath;
                path = Path.Combine(baseDirectory, path);
                if (File.Exists(path))
                {
                    // TODO Must treat everything as binary (can be img's etc).
                    // TODO Log when we do not find the file...
                    var content = File.ReadAllText(path);
                    session.utilDecodeResponse();
                    session.utilSetResponseBody(content);
                }                
            } 
        }        

        private void Initialize()
        {
            baseDirectory = "";
        }

        [OnDeserializing]
        private void DeserializationInitializer(StreamingContext ctx)
        {
            this.Initialize();
        }
    }
}
