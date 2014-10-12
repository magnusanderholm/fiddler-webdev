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
        
        // TODO Ensure that we cannot add duplicates!!!! Porably best to put that in the custom ObservableCollection class.
        public IEnumerable<Config.UrlRule> UrlRules { get; private set; }

        public Config.UrlRule Create()
        {
            return SettingsRepository.Settings.AddUrlRule();
        }

        public FileInfo CurrentSettingsFile { get;set; }

        public void SaveSettings()
        {
            // TODO Throw if CurrentSettingsFile is not set.
            // Save current settings to file. This means that we need
            SettingsRepository.Save(CurrentSettingsFile);
        }
        // TODO Add Clear method for example.        

        public void OpenSettings(FileInfo fI)
        {
            CurrentSettingsFile = fI;
            SettingsRepository.Open(CurrentSettingsFile);
        }
    }
}
