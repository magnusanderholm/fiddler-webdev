using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Fiddler.VSAutoResponder.Model
{
    public class Settings 
    {
        public Settings()
        {
            var bindingList = new BindingList<Redirect>();            
            bindingList.ListChanged += OnListChanged;
            Redirects = bindingList;
        }

        private void OnListChanged(object sender, ListChangedEventArgs e)
        {                        
            //  Kick of Save task and save settings to disk. As Json or xml???

            // TODO If we are adding items make sure we register their propertychanged event
            //      so we get notifications right away. Hmm perhaps we shall use some kind of timer 
            //      before we save so we actually dont just save "garbage" data and stall the UI thread because
            //      of all the I/O.

            // Save any settings to the HostsFile as well. Should probably be done in the same way as above.
        }

        public HostFile Hosts { get; private set; }

        // TODO Changing a redirect will trigger a change in the hosts file....
        public IEnumerable<Redirect> Redirects { get; private set; }        
    }
}
