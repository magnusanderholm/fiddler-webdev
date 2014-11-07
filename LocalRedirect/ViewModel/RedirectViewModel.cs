namespace Fiddler.LocalRedirect.ViewModel
{
    using Fiddler.LocalRedirect.Model;

    public class RedirectViewModel : Fiddler.LocalRedirect.ViewModel.IRedirectViewModel
    {
        public RedirectViewModel(ISettingsStorage settingsStorage)
        {                                    
            SettingsStorage = settingsStorage;            
        }

        public ISettingsStorage SettingsStorage { get; private set; }                                
    }
}
