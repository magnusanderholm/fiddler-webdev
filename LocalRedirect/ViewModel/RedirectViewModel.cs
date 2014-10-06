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
            Redirects = settings.Matches;
        }
        
        // TODO Ensure that we cannot add duplicates!!!! Porably best to put that in the custom ObservableCollection class.
        public ICollection<SettingBase> Redirects { get; private set; }

        public Model.Redirect Create()
        {
            var redirect = new Model.Redirect();
            var lastRedirect = Redirects.LastOrDefault();
            if (lastRedirect != null)
            {                
                redirect.ToHost = lastRedirect.ToHost;
            }
            return redirect;
        }
        // TODO Add Clear method for example.
    }
}
