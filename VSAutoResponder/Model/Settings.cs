using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Fiddler.VSAutoResponder.Model
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
                    // redirects.Add(new Redirect() { Url = new Uri("https://veidekkeintra.blob.core.windows.net"), IsEnabled = true, UseMinified = true });
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
