using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Fiddler.LocalRedirect.Model
{
    [DataContract(Name = "redirect", Namespace="")]    
    public class Redirect : INotifyPropertyChanged
    {
        private string url = string.Empty;
        private bool isEnabled = false;
        private bool useMinified = false;  

        [DataMember(Name = "url", IsRequired = true, EmitDefaultValue = true), DefaultValue("")]
        public string Url 
        {
            get { return url; }
            set { Update(ref url, "Url", (value ?? string.Empty).ToLower());}
        }

        public string Host
        {
            get { return IsValid ? new Uri(Url, UriKind.Absolute).Host : string.Empty; }
        }

        public bool IsValid
        {
            get { return Uri.IsWellFormedUriString(Url, UriKind.Absolute); }
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
