using Fiddler.LocalRedirect.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Fiddler.LocalRedirect.ViewModel
{
    public class RedirectViewModel
    {        
        public RedirectViewModel(Settings settings)
        {
            Redirects = settings.Redirects;
        }
        
        // TODO Ensure that we cannot add duplicates!!!! Porably best to put that in the custom ObservableCollection class.
        public ICollection<Redirect> Redirects { get; private set; }


        // TODO Add Clear method for example.
    }
}
