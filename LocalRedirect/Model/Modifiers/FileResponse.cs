namespace Fiddler.LocalRedirect.Model
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.Serialization;

    [DataContract(Name = "fileresponse", Namespace = "")]
    [Modifier(Order=0, IsEnabled=true)]
    public class FileResponse : ChildSetting
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
        public string DirectoryPath
        {
            get 
            { 
                return this.baseDirectory; 
            }
            set 
            {
                if (value != null && !Path.IsPathRooted(value))
                    throw new ArgumentException("Not an absolute path", "value");

                if (!string.IsNullOrEmpty(value))
                {  
                    // Make sure that the path looks valid.
                    var dir = new DirectoryInfo(value);                                        
                }
                
                pC.Update(ref baseDirectory, value); 
            }
        }

        public override void RequestBefore(Session session)
        {
            base.RequestBefore(session);
            if (IsEnabled /*&& this.HasScript*/)
            {
                var localFile = new FileInfo(Path.Combine(baseDirectory, new Uri(session.PathAndQuery).LocalPath));
                if (localFile.Exists)
                    session.oFlags["x-replywithfile"] = localFile.FullName;
            }
        }                      
    }
}