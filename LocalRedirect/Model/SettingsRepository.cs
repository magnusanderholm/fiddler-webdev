namespace Fiddler.LocalRedirect.Model
{
    using Fiddler.LocalRedirect.Model;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;

    public class SettingsRepository : INotifyPropertyChanged, Fiddler.LocalRedirect.Model.ISettingsRepository
    {
        private readonly SerializerEx<Settings> settingsSerializer = new SerializerEx<Settings>();
        private readonly Settings settings;                
        private FileInfo currentSettingsFile;
        private NotifyPropertyChanged pC;

        private static readonly ILogger logger = LogManager.CreateCurrentClassLogger();
        public SettingsRepository(FileInfo defaultSettingsFile)
        {
            if (defaultSettingsFile == null)
                throw new ArgumentNullException("defaultSettingsFile");
            pC = new NotifyPropertyChanged(OnPropertyChanged);
            Mru = new MostRecentlyUsedFiles(10);
            if (!Mru.Any(f => string.Compare(f.FullName, defaultSettingsFile.FullName) == 0))
                Mru.Touch(defaultSettingsFile);

            currentSettingsFile = Mru.First();
            settings = new Settings();
            
            if (!defaultSettingsFile.Exists || defaultSettingsFile.Length == 0)
            {
                SaveSettingsToFile(defaultSettingsFile, settings);
            }
            else
            {
                try
                {
                    settings = LoadSettingsFromFile(defaultSettingsFile);
                }
                catch (Exception e)
                {
                    logger.Error(() => string.Format("{0} has invalid format.", currentSettingsFile.FullName), e);
                }
            }
                            
            settings.Observer.Changed  += OnSettingsChanged;

        }


        public IMostRecentlyUsed<FileInfo> Mru { get; private set; }
        
        public Settings Settings { get { return settings; } }

        public FileInfo CurrentFile
        {
            get { return currentSettingsFile; }
            set 
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                if (string.Compare(value.FullName, currentSettingsFile.FullName) != 0)
                    pC.Update(ref currentSettingsFile, value);
            }
        }

        public void Save(FileInfo fI)
        {            
            SaveSettingsToFile(fI, Settings);
            CurrentFile = fI;
        }

        public Settings Open(FileInfo fI)
        {
            try
            {
                var newSettings = LoadSettingsFromFile(fI);
                settings.ReplaceUrlRulesWith(newSettings.UrlRules);
                Mru.Touch(fI);
                CurrentFile = fI;                
            }
            catch (Exception e)
            {
                logger.Error(() => string.Format("{0} has invalid format.", CurrentFile.FullName), e);
            }
            
            return settings;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs pCe)
        {
            var h = PropertyChanged;
            if (h != null)
                h(this, pCe);
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

            Mru.Touch(settingsFile);
        }

        
    }
}
