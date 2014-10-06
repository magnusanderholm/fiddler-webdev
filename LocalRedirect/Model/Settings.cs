using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Collections.Specialized;

namespace Fiddler.LocalRedirect.Model
{
    [DataContract(Name="settings", Namespace="")]
    public class Settings
    {
        private ObservableItemCollection<UrlRule> urlRules;

        public Settings()
        {                        
        }                
        

        [DataMember(Name="urlrules")]
        public ObservableItemCollection<UrlRule> UrlRules
        {
            get
            {
                urlRules = urlRules ?? new ObservableItemCollection<UrlRule>();
                return urlRules;
            }
            set
            {
                var _urlRules = UrlRules;
                _urlRules.Clear();
                if (value != null)
                {
                    foreach (var val in value)
                        _urlRules.Add(val);
                }
            }
        }
    }
}
