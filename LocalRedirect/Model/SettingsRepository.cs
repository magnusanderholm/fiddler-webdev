﻿using System;
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
        private readonly Settings settings = new Settings();
        private readonly FileInfo settingsFile;

        public SettingsRepository(FileInfo settingsFile)
        {                       
            this.settingsFile = settingsFile;
            
            if (!settingsFile.Exists) // Ensure we always have a settings file
                SaveSettingsToFile(settingsFile, settings);
            
            this.settings = LoadSettingsFromFile(settingsFile);
            this.settings.Redirects.CollectionChanged += (s, e) => Persist();
            this.settings.Redirects.ItemPropertyChanged += (s, e) => Persist();
        }
        

        public Settings Settings { get { return settings; } }

        public void Persist()
        {
            SaveSettingsToFile(settingsFile, Settings);
        }

        public Settings CopySettings()
        {
            return settingsSerializer.DeepCopy(settings);
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
