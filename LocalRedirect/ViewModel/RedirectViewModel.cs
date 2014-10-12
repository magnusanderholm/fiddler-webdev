using Fiddler.LocalRedirect.Config;
using Fiddler.LocalRedirect.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Markup;

namespace Fiddler.LocalRedirect.ViewModel
{
    public class RedirectViewModel
    {        

        public RedirectViewModel(SettingsRepository settingsRepository)
        {                                    
            SettingsRepository = settingsRepository;
            UrlRules = SettingsRepository.Settings.UrlRules;
        }

        public SettingsRepository SettingsRepository { get; private set; }        
                
        public IEnumerable<Config.UrlRule> UrlRules { get; private set; }                       
    }
}
