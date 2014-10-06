using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Fiddler.LocalRedirect.Model
{
    [DataContract(Name = "settingbase", Namespace = "")]            
    [KnownType(typeof(ChildSetting))]
    [KnownType(typeof(UrlRule))]
    [KnownType(typeof(BrowserLink))]
    [KnownType(typeof(HeaderScript))]
    [KnownType(typeof(Redirect))]
    public abstract class SettingBase : INotifyPropertyChanged
    {
        private bool isEnabled = false;
                                
        [DataMember(Name = "isenabled", IsRequired=true, EmitDefaultValue = true), DefaultValue(false)]
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { Update(ref isEnabled, value, "IsEnabled"); }
        }        
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs pCe)
        {
            var h = PropertyChanged;
            if (h != null)
                h(this, pCe);
        }

        protected void Update<T>(ref T var, T value, string name, params string[] extraNames)
        {
            if (!object.Equals(var, value))
            {
                var = value;
                OnPropertyChanged(new PropertyChangedEventArgs(name));
                foreach (var n in extraNames)
                    OnPropertyChanged(new PropertyChangedEventArgs(n));
            }
        }
    }
}
