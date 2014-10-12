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

        public SettingsRepository(FileInfo settingsFile)
        {            
            settings = new Settings();
            settings.Observer.Changed  += OnSettingsChanged;
        }

        private void OnSettingsChanged(object sender, EventArgs e)
        {            
        }
        
        public Settings Settings { get { return settings; } }

        public void Save(FileInfo fI)
        {
            SaveSettingsToFile(fI, Settings);
        }

        public Settings Open(FileInfo fI)
        {
            var newSettings = LoadSettingsFromFile(fI);
            settings.ClearUrlRules();
            foreach (var urlRule in newSettings.UrlRules)
            {
                urlRule.Parent = settings;
                settings.AddUrlRule();
            }
            
            return settings;
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
