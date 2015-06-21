namespace Fiddler.Webdev.Model
{
    using System;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;

    public class SettingsStorage : INotifyPropertyChanged, ISettingsStorage
    {
        private readonly SerializerEx<Settings> settingsSerializer = new SerializerEx<Settings>();
        private FileInfo currentStorage = new FileInfo(Guid.NewGuid().ToString("N"));
        private NotifyPropertyChanged pC;

        private static readonly ILogger log = LogManager.CreateCurrentClassLogger();        

        public SettingsStorage(IMostRecentlyUsed<FileInfo> mru)
        {            
            if (mru == null)
                throw new ArgumentNullException("mru");
            if (!mru.Any())
                throw new ArgumentException("Must contain at least one element!", "mru");
            
            this.pC = new NotifyPropertyChanged(OnPropertyChanged);
            this.Mru = mru;

            CurrentStorage = Mru.First();
        }

        public IMostRecentlyUsed<FileInfo> Mru { get; private set; }

        public Settings Settings { get; private set; }

        public FileInfo CurrentStorage
        {
            get { return currentStorage; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                var notifyChanged = pC.Update(
                    ref currentStorage,
                    value,
                    (s0, s1) => string.Compare(value.FullName, currentStorage.FullName) == 0);
                
                if (notifyChanged.IsChanged)
                {
                    CurrentStorage.Refresh(); // Make sure we are looking at a fresh file descriptor.

                    // Load settings from file if possible. Otherwise try to fallback to current settings and finally
                    // empty settings.
                    Settings = TryLoadSettingsFromFile(CurrentStorage, Settings ?? new Settings());
                    notifyChanged.Extra("Settings");

                    // If file does not exist then we loaded default settings. Resave it to disk so we
                    // always have a file on disk. (Otherwise we'll get issues with MRU)
                    if (!CurrentStorage.Exists)                    
                        SaveSettingsToFile(CurrentStorage, Settings);

                    // Most recently used update.
                    Mru.Touch(CurrentStorage);

                    // Now we shall subscribe to any changes made to any object in this.Settings                                                        
                    RegisterOnSettingsPropertyAndCollectionChangedEvents(this.Settings);                    
                }
            }
        }
       
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler SettingsChanged;
        
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs pCe)
        {
            var h = PropertyChanged;
            if (h != null)
                h(this, pCe);
        }

        protected virtual void OnSettingsChanged(object sender, EventArgs e)
        {
            // Since a item in Settings changed (may be a INotifyPropertyChanged or INotifyCollectionChanged item)
            // we need to traverse the tree and listen to any potional new objects PropertyChanged or Collectionchanged events.
            RegisterOnSettingsPropertyAndCollectionChangedEvents(Settings); 
            SaveSettingsToFile(CurrentStorage, Settings);
            var h = SettingsChanged;
            if (h != null)
                h(this, e);
        }

        private Settings TryLoadSettingsFromFile(FileInfo settingsFile, Settings defaultValue)
        {
            Settings _settings = defaultValue;
            if (settingsFile.Exists)
            {
                // A settings file does exist. Just load it as usual (if possible). 
                // TODO What happens if we get an error???
                using (var s = settingsFile.OpenRead())
                    _settings = settingsSerializer.Deserialize(s);
            }
            log.Debug(() => "Restored settings from " + settingsFile.FullName);
            return _settings;
        }

        private void SaveSettingsToFile(FileInfo settingsFile, Settings settings)
        {
            using (var fS = settingsFile.OpenWrite())
            {
                fS.SetLength(0);
                settingsSerializer.Serialize(settings, fS);
            }
            CurrentStorage.Refresh();                        
            log.Debug(() => "Saved settings to " + settingsFile.FullName);
        }

        private void RegisterOnSettingsPropertyAndCollectionChangedEvents(Settings settings)
        {
            foreach (var item in settings.AsFlattenedEnumerable())
            {
                var nP = item as INotifyPropertyChanged;
                var nC = item as INotifyCollectionChanged;
                
                if (nC != null)
                {
                    nC.CollectionChanged -= OnSettingsChanged;
                    nC.CollectionChanged += OnSettingsChanged;
                }

                if (nP != null)
                {
                    nP.PropertyChanged -= OnSettingsChanged;
                    nP.PropertyChanged += OnSettingsChanged;
                }
            }
        }  
    }
}
