namespace Fiddler.LocalRedirect.ViewModel
{
    using Fiddler.LocalRedirect.Model;
    using System.Collections.Generic;

    public class RedirectViewModel
    {        

        public RedirectViewModel(SettingsRepository settingsRepository)
        {                                    
            SettingsRepository = settingsRepository;
            UrlRules = SettingsRepository.Settings.UrlRules;
        }

        public SettingsRepository SettingsRepository { get; private set; }        
                
        public IEnumerable<Model.UrlRule> UrlRules { get; private set; }                       
    }
}
