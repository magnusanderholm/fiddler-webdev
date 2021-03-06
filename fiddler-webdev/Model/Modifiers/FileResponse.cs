﻿namespace Fiddler.Webdev.Model
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
        private static readonly ILogger log = LogManager.CreateCurrentClassLogger();

        private FileResponse()
            : this(null)
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
                // Cut of parent path so we get a path relative the parent
                // this is then used with baseDir to find the local file.
                var path = new Uri(session.fullUrl)
                    .LocalPath.Remove(0, this.Parent.Url.LocalPath.Length)
                    .TrimStart('/');                
                var localFile = new FileInfo(Path.Combine(baseDirectory, path));
                if (localFile.Exists)
                    session.oFlags["x-replywithfile"] = localFile.FullName;
                else
                    log.Error(() => string.Format("'{0}' does not exist", localFile.FullName));
            }
        }    
    }
}