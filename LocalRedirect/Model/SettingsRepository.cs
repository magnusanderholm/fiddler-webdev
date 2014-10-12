using Fiddler.LocalRedirect.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Fiddler.LocalRedirect.Model
{
    public class SettingsRepository
    {
        private readonly SerializerEx<Settings> settingsSerializer = new SerializerEx<Settings>();
        private readonly Settings settings;                
        private FileInfo currentSettingsFile;
        public SettingsRepository(FileInfo defaultSettingsFile)
        {
            if (defaultSettingsFile == null)
                throw new ArgumentNullException("defaultSettingsFile");

            currentSettingsFile = defaultSettingsFile;
            settings = new Settings();
            if (!defaultSettingsFile.Exists)
                SaveSettingsToFile(defaultSettingsFile, settings);
            else
                settings = LoadSettingsFromFile(defaultSettingsFile);
            
            settings.Observer.Changed  += OnSettingsChanged;
        }
        
        public Settings Settings { get { return settings; } }

        public FileInfo CurrentFile
        {
            get { return currentSettingsFile; }
            set 
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                if (string.Compare(value.FullName, currentSettingsFile.FullName) != 0)
                    currentSettingsFile = value; 
            }
        }

        public void Save(FileInfo fI)
        {
            CurrentFile = fI;
            SaveSettingsToFile(CurrentFile, Settings);
        }

        public Settings Open(FileInfo fI)
        {
            CurrentFile = fI;
            var newSettings = LoadSettingsFromFile(CurrentFile);
            settings.ReplaceUrlRulesWith(newSettings.UrlRules);                        
            return settings;
        }

        private void OnSettingsChanged(object sender, EventArgs e)
        {
            SaveSettingsToFile(currentSettingsFile, settings);
        }

        private Settings LoadSettingsFromFile(FileInfo settingsFile)
        {            
            using (var s = settingsFile.OpenRead())            
                return settingsSerializer.Deserialize(s);
        }

        private void SaveSettingsToFile(FileInfo settingsFile, Settings settings)
        {
            using (var fS = settingsFile.OpenWrite())
            {
                fS.SetLength(0);
                settingsSerializer.Serialize(settings, fS);
            }                       
        }        
    }
}
