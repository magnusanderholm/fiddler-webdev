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
        private ObservableItemCollection<Redirect> redirects;

        public Settings()
        {                        
        }                
        

        [DataMember(Name="redirects")]
        public ObservableItemCollection<Redirect> Redirects
        {
            get
            {
                redirects = redirects ?? new ObservableItemCollection<Redirect>();
                return redirects;
            }
            set
            {
                var _redirects = Redirects;
                _redirects.Clear();
                if (value != null)
                {
                    foreach (var val in value)
                        _redirects.Add(val);
                }
            }
        }
    }
}
