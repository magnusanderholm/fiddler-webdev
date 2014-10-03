using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Fiddler.LocalRedirect.Model
{
    [DataContract(Name = "redirect", Namespace="")]    
    public class Redirect : INotifyPropertyChanged
    {
        private string fromUrl = string.Empty;
        private string toHost = "localhost";
        private bool isEnabled = false;
        private bool useMinified = false;
        private int toPort = 80;
        private string headerScriptPath = string.Empty;
        private bool browserLinkEnabled = false;

        public Redirect()
        {
        }
        
        [DataMember(Name = "fromUrl", IsRequired = true, EmitDefaultValue = true), DefaultValue("")]
        public string FromUrl 
        {            
            get { return fromUrl; }
            set { Update(ref fromUrl, "FromUrl", (value ?? string.Empty).ToLower()); }
        }

        [DataMember(Name = "tohost", IsRequired = true, EmitDefaultValue = true), DefaultValue("localhost")]
        public string ToHost
        {
            // TODO Throw exception if host name contains invalid values.
            get { return toHost; }
            set { Update(ref toHost, "ToHost", (value ?? string.Empty).ToLower()); }
        }

        [DataMember(Name = "toport", IsRequired = true, EmitDefaultValue = true), DefaultValue(80)]
        public int ToPort
        {
            // TODO Throw exceptions if port range is invalid.
            get { return toPort; }
            set { Update(ref toPort, "ToPort", value); }
        }


        [DataMember(Name = "headerscriptpath", IsRequired = true, EmitDefaultValue = true), DefaultValue("")]
        public string HeaderScriptPath
        {
            get { return headerScriptPath; }
            set { Update(ref headerScriptPath, "HeaderScriptPath", value ?? string.Empty); }
        }

        public bool HasHeaderScript
        {
            get
            {

                var headerScriptFile = new FileInfo(HeaderScriptPath);
                return headerScriptFile.Exists && headerScriptFile.Length > 0;                
            }
        }

        public string HeaderScript
        {
            get 
            {
                return HasHeaderScript
                    ? File.ReadAllText(new FileInfo(HeaderScriptPath).FullName)
                    : String.Empty;
            }
        }

        [DataMember(Name = "browserlinkenabled", EmitDefaultValue = true), DefaultValue(false)]
        public bool BrowserLinkEnabled
        {
            get { return browserLinkEnabled; }
            set { Update(ref browserLinkEnabled, "BrowserLinkEnabled", value); }
        }

        public bool IsValid
        {
            get 
            { 
                return 
                    (FromUrl.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase) || 
                    FromUrl.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase)) &&
                    !string.IsNullOrEmpty(ToHost); 
            }
        }

        [DataMember(Name = "isenabled", EmitDefaultValue=true), DefaultValue(false)]
        public bool IsEnabled 
        {
            get { return isEnabled; }
            set { Update(ref isEnabled, "IsEnabled", value); }
        }

        [DataMember(Name="useminified", EmitDefaultValue=true), DefaultValue(false)]
        public bool UseMinified 
        {
            get { return useMinified; }
            set { Update(ref useMinified, "UseMinified", value); }
        }        
        
        public event PropertyChangedEventHandler PropertyChanged;
                

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs pCe)
        {
            var h = PropertyChanged;
            if (h != null)
                h(this, pCe);
        }

        private void Update<T>(ref T var, string name, T value)
        {
            if (!object.Equals(var, value))
            {
                var = value;
                OnPropertyChanged(new PropertyChangedEventArgs(name));
            }
        }
        
    }
}
