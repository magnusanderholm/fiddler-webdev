namespace Fiddler.LocalRedirect.ViewModel
{
    using Fiddler.LocalRedirect.Model;
    using System.Collections.Generic;
using System.IO;

    public class RedirectViewModel : Fiddler.LocalRedirect.ViewModel.IRedirectViewModel
    {
        public RedirectViewModel(ISettingsRepository settingsRepository)
        {                                    
            SettingsRepository = settingsRepository;
            UrlRules = SettingsRepository.Settings.UrlRules;
        }

        public ISettingsRepository SettingsRepository { get; private set; }        
                
        public IEnumerable<Model.UrlRule> UrlRules { get; private set; }                
    }
}
