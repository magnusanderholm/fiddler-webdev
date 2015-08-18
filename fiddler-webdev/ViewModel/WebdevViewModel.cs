namespace Fiddler.Webdev.ViewModel
{
    using Fiddler.Webdev.Model;
    using System.Collections.Generic;
    using System.IO;

    public class WebdevViewModel : Fiddler.Webdev.ViewModel.IWebdevViewModel
    {
        public WebdevViewModel(ISettingsStorage settingsStorage, IEnumerable<FileInfo> mru)
        {                                    
            SettingsStorage = settingsStorage;
            Mru = mru;
        }

        public IEnumerable<FileInfo> Mru { get; private set; }

        public ISettingsStorage SettingsStorage { get; private set; }                                
    }
}
