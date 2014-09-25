using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Fiddler.LocalRedirect.Model
{
    [DataContract(Name="settings", Namespace="")]
    public class Settings
    {
        private BindingList<Redirect> redirects;

        public Settings()
        {                        
        }                
        

        [DataMember(Name="redirects")]
        public ICollection<Redirect> Redirects
        {
            get
            {
                if (redirects == null)
                {
                    redirects = new BindingList<Redirect>();
                    redirects.ListChanged += (s, e) => OnSettingsChanged(EventArgs.Empty);                                       
                }                
                return redirects;
            }
            set
            {
                if (redirects != null)
                    redirects.RaiseListChangedEvents = false;
                Redirects.Clear();
                if (value != null)
                {                    
                    foreach (var val in value)
                        Redirects.Add(val);
                }
                redirects.RaiseListChangedEvents = true;
                redirects.ResetBindings();
            }
        }

        public event EventHandler<EventArgs> Changed;

        protected virtual void OnSettingsChanged(EventArgs e)
        {
            var h = Changed;
            if (h != null)
                h(this, e);
        } 
    }
}
