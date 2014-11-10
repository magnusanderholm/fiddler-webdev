namespace Fiddler.LocalRedirect.ViewModel
{
    using Fiddler.LocalRedirect.Model;
    using System.Collections.Generic;
    using System.IO;

    public class RedirectViewModel : Fiddler.LocalRedirect.ViewModel.IRedirectViewModel
    {
        public RedirectViewModel(ISettingsStorage settingsStorage, IEnumerable<FileInfo> mru)
        {                                    
            SettingsStorage = settingsStorage;
            Mru = mru;
        }

        public IEnumerable<FileInfo> Mru { get; private set; }

        public ISettingsStorage SettingsStorage { get; private set; }                                
    }
}
