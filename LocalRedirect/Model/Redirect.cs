using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;

namespace Fiddler.LocalRedirect.Model
{
    [DataContract(Name = "redirect", Namespace="")]    
    public class Redirect : INotifyPropertyChanged
    {
        private string fromUrl = string.Empty;
        private HostName toHost = new HostName("localhost:80");
        private bool isEnabled = false;
        private bool useMinified = false;        
        private string headerScriptPath = string.Empty;
        private bool browserLinkEnabled = false;
        private bool isHeaderScriptEnabled = false;

        public readonly static Redirect Empty = new Redirect();

        public Redirect()
        {
        }
        
        [DataMember(Name = "fromUrl", IsRequired = true, EmitDefaultValue = true), DefaultValue("")]
        public string FromUrl 
        {            
            get { return fromUrl; }
            set { Update(ref fromUrl, (value ?? string.Empty).ToLower(), "FromUrl", "FromScheme"); }
        }

        public string FromScheme
        {
            get
            {
                return !string.IsNullOrWhiteSpace(FromUrl)
                  ? new Uri(FromUrl).Scheme.ToUpper()
                  : "NONE";
            }
        }

        // host[:<port>]
        [DataMember(Name = "tohost", IsRequired = true, EmitDefaultValue = true), DefaultValue("localhost")]
        public string ToHost
        {
            // TODO Throw exception if host name contains invalid values.
            get { return toHost.ToString(); }
            set { Update<HostName>(ref toHost, new HostName(value), "ToHost", "ToPort"); }
        }                

        [DataMember(Name = "headerscriptpath", IsRequired = true, EmitDefaultValue = true), DefaultValue("")]
        public string HeaderScriptPath
        {
            get { return headerScriptPath; }
            set { Update(ref headerScriptPath, value ?? string.Empty, "HeaderScriptPath"); }
        }

        [DataMember(Name = "isheaderscriptenabled", IsRequired = false, EmitDefaultValue = true), DefaultValue("")]
        public bool IsHeaderScriptEnabled
        {
            get { return isHeaderScriptEnabled; }
            set { Update(ref isHeaderScriptEnabled, value, "IsHeaderScriptEnabled"); }
        }

        public bool HasHeaderScript
        {
            get
            {
                bool hasHeaderScript = false;
                if (!string.IsNullOrWhiteSpace(HeaderScriptPath))
                {
                    var headerScriptFile = new FileInfo(HeaderScriptPath);
                    hasHeaderScript =  headerScriptFile.Exists && headerScriptFile.Length > 0;
                }

                return hasHeaderScript;
            }
        }

        public string HeaderScript
        {
            get 
            {
                // TODO Use cache to reread file at regular intervals instead.
                return HasHeaderScript
                    ? File.ReadAllText(new FileInfo(HeaderScriptPath).FullName)
                    : String.Empty;
            }
        }

        [DataMember(Name = "browserlinkenabled", EmitDefaultValue = true), DefaultValue(false)]
        public bool BrowserLinkEnabled
        {
            get { return browserLinkEnabled; }
            set { Update(ref browserLinkEnabled, value, "BrowserLinkEnabled"); }
        }

        public bool CanRedirect
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
            set { Update(ref isEnabled, value, "IsEnabled"); }
        }

        [DataMember(Name="useminified", EmitDefaultValue=true), DefaultValue(false)]
        public bool ForceUnminified 
        {
            get { return useMinified; }
            set { Update(ref useMinified, value, "UseMinified"); }
        }        
        
        public event PropertyChangedEventHandler PropertyChanged;
                

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs pCe)
        {
            var h = PropertyChanged;
            if (h != null)
                h(this, pCe);
        }

        private void Update<T>(ref T var, T value, string name, params string[] extraNames)
        {
            if (!object.Equals(var, value))
            {
                var = value;
                OnPropertyChanged(new PropertyChangedEventArgs(name));
                foreach(var n in extraNames)
                    OnPropertyChanged(new PropertyChangedEventArgs(n));
            }
        }
        
    }
}
