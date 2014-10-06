using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Fiddler.LocalRedirect.Model
{
    [DataContract(Name = "urlrule", Namespace = "")]
    public class UrlRule : SettingBase
    {
        private string url = string.Empty;
        private ObservableItemCollection<ChildSetting> settings;

        [DataMember(Name = "url", IsRequired = true, EmitDefaultValue = true), DefaultValue("")]
        public string Url
        {
            get { return url; }
            set { Update(ref url, (value ?? string.Empty).ToLower(), "Url", "Scheme"); }
        }

        [DataMember(Name = "settings", IsRequired = false, EmitDefaultValue = false)]
        public ICollection<ChildSetting> Settings
        {
            get 
            { 
                // Setup default child settings collection.
                return (settings = settings ?? new ObservableItemCollection<Settings>()); 
            }
            set
            {

            }
        }

        public string Scheme
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Url)
                  ? new Uri(Url).Scheme.ToUpper()
                  : "NONE";
            }
        }  
    }
}
