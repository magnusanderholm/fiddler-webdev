namespace Fiddler.LocalRedirect.Model
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract(Name = "fileresponse", Namespace = "")]
    [Modifier(Order=3, IsEnabled=true)]
    [Serializable()]
    [XmlRoot(Namespace = "", ElementName = "fileresponse")]
    public class FileResponse : Modifier
    {
        private string baseDirectory;

        private FileResponse()
        {            
        }

        public FileResponse(UrlRule parent)
            : base(parent)
        {         
        }

        [DataMember(Name = "directorypath", IsRequired = false, EmitDefaultValue=false)]
        [XmlAttribute(AttributeName = "directorypath"), DefaultValue(null)]
        public string DirectoryPath
        {
            get 
            { 
                return this.baseDirectory; 
            }
            set 
            {
                var val = (value ?? string.Empty).Trim();
                if (val != string.Empty && !Path.IsPathRooted(val))
                    throw new ArgumentException("Not an absolute path", "value");

                if (val != string.Empty)
                {  
                    // Make sure that the path looks valid.
                    var dir = new DirectoryInfo(value);                                        
                }
                
                pC.Update(ref baseDirectory, val); 
            }
        }

        public override void RequestBefore(Session session)
        {
            base.RequestBefore(session);
            if (IsEnabled /*&& this.HasScript*/)
            {
                var localFile = new FileInfo(Path.Combine(baseDirectory, new Uri(session.fullUrl).LocalPath.TrimStart('/')));
                if (localFile.Exists)
                    session.oFlags["x-replywithfile"] = localFile.FullName;
            }
        }                      
    }
}