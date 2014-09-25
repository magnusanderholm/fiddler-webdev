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
            // TODO Need to make Settings and Redirect instances threadsafe somehow.
            //      best would probably be if we use a copy of Settings in the RedirectEngine
            //      and only update it once in a while
            var bindingList = new BindingList<Redirect>();            
            bindingList.ListChanged += OnListChanged;
            Redirects = bindingList;
            bindingList.Add(new Redirect() { Url = new Uri("https://veidekkeintra.blob.core.windows.net"), IsEnabled = true, UseMinified = true });
        }

        private void OnListChanged(object sender, ListChangedEventArgs e)
        {                        
            //  Kick of Save task and save settings to disk. As Json or xml???

            // TODO If we are adding items make sure we register their propertychanged event
            //      so we get notifications right away. Hmm perhaps we shall use some kind of timer 
            //      before we save so we actually dont just save "garbage" data and stall the UI thread because
            //      of all the I/O.            
        }        
        
        public IEnumerable<Redirect> Redirects { get; private set; }        
    }
}
