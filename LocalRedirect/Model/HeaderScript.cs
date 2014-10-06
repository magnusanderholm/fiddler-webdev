namespace Fiddler.LocalRedirect.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;

    [DataContract(Name = "headerscript", Namespace = "")]
    public class HeaderScript : ChildSetting
    {
        private string headerScriptPath = string.Empty;
        
        [DataMember(Name = "headerscriptpath", IsRequired = true, EmitDefaultValue = true), DefaultValue("")]
        public string HeaderScriptPath
        {
            get { return headerScriptPath; }
            set { Update(ref headerScriptPath, value ?? string.Empty, "HeaderScriptPath"); }
        }
        
        public bool HasScript
        {
            get
            {
                bool hasHeaderScript = false;
                if (!string.IsNullOrWhiteSpace(HeaderScriptPath))
                {
                    var headerScriptFile = new FileInfo(HeaderScriptPath);
                    hasHeaderScript = headerScriptFile.Exists && headerScriptFile.Length > 0;
                }

                return hasHeaderScript;
            }
        }

        public string Script
        {
            get
            {
                // TODO Use cache to reread file at regular intervals instead.
                return HasScript
                    ? File.ReadAllText(new FileInfo(HeaderScriptPath).FullName)
                    : String.Empty;
            }
        }
    }
}
