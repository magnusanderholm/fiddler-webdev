namespace Fiddler.LocalRedirect.ViewModel
{
    using Fiddler.LocalRedirect.Model;
    using System.Collections.Generic;

    public class RedirectViewModel : Fiddler.LocalRedirect.ViewModel.IRedirectViewModel
    {
        public RedirectViewModel(ISettingsRepository settingsRepository)
        {                                    
            SettingsRepository = settingsRepository;
            UrlRules = SettingsRepository.Settings.UrlRules;
        }

        public ISettingsRepository SettingsRepository { get; private set; }        
                
        public IEnumerable<Model.UrlRule> UrlRules { get; private set; }

        // TODO Viewmodel simply sends a message to SettingsRepository telling it where future files shall be saved.
        //      Viewmodel keeps track of file reference
    }
}
