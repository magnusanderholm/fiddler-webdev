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
        private readonly Settings settings = Settings.CreateDefault();
        // private readonly FileInfo settingsFile;

        public SettingsRepository()
        {                       
            // this.settingsFile = settingsFile;
                                    
            // this.settings = LoadSettingsFromFile(settingsFile);            
            //this.settings.Matches.CollectionChanged += (s, e) => Persist();
            //this.settings.Matches.ItemPropertyChanged += (s, e) => Persist();
        }
        

        public Settings Settings { get { return settings; } }

        public void Save(FileInfo fI)
        {
            SaveSettingsToFile(fI, Settings);
            OnChanged(EventArgs.Empty);
        }

        public event EventHandler<EventArgs> Changed;

        protected virtual void OnChanged(EventArgs e)
        {
            var h = Changed;
            if (h != null)
                h(this, e);
        }


        public Settings Open(FileInfo fI)
        {
            var newSettings = LoadSettingsFromFile(fI);
            settings.UrlRules.Clear();
            foreach (var urlRule in newSettings.UrlRules)
                settings.UrlRules.Add(urlRule);
            this.OnChanged(EventArgs.Empty);
            return settings;
        }

        public Settings CopySettings()
        {
            return settingsSerializer.DeepCopy(settings);
        }
        
        
        private Settings LoadSettingsFromFile(FileInfo settingsFile)
        {            
            using (var s = settingsFile.OpenRead())
            {
                var settings = settingsSerializer.Deserialize(s);
                foreach (var urlRule in settings)
                {
                    foreach (var child in urlRule)
                        child.Parent = urlRule;
                }
                return settings;
            }                                        
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
