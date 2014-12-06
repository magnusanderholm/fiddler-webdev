namespace Fiddler.LocalRedirect.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;

    public class SettingsStorage : INotifyPropertyChanged, ISettingsStorage
    {
        private readonly SerializerEx<Settings> settingsSerializer = new SerializerEx<Settings>();
        private FileInfo currentStorage = new FileInfo(Guid.NewGuid().ToString("N"));
        private NotifyPropertyChanged pC;

        private static readonly ILogger logger = LogManager.CreateCurrentClassLogger();        

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
                    // Load settings from file if possible. Otherwise try to fallback to current settings and finally
                    // empty settings.
                    Settings = TryLoadSettingsFromFile(CurrentStorage, Settings ?? new Settings());
                    notifyChanged.Extra("Settings");

                    // Always save the settings once to the new location to ensure we actually have stored the data on disk
                    // at least once.
                    SaveSettingsToFile(CurrentStorage, Settings);

                    // Most recently used update.
                    Mru.Touch(CurrentStorage);


                    // Now we shall subscribe to any changes made to any object in this.Settings.                         
                    // Traverse the settings tree and hookup the subscriptions on our new eventbus. Since we will be subscribing
                    // to INotifyPropertyChanged events and on NotifyCollecitonChangedEventArgs we only need to listen for such messages
                    // when they arrive we just make sure that we also subscribe on all those new items as well. That way the tree can grow
                    // dynamically and we do not really care about what new objects that removed/entered into the tree.                    
                    ListenForChangesInSettings();                    
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

        protected virtual void OnSettingsChanged(EventArgs e)
        {
            ListenForChangesInSettings();
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
            return _settings;
        }

        private void SaveSettingsToFile(FileInfo settingsFile, Settings settings)
        {
            using (var fS = settingsFile.OpenWrite())
            {
                fS.SetLength(0);
                settingsSerializer.Serialize(settings, fS);
            }
        }

        private void ListenForChangesInSettings()
        {
            var flattenedTree = new List<object>();
            flattenedTree.Add(Settings);
            flattenedTree.Add(Settings.UrlRules);
            flattenedTree.AddRange(Settings.UrlRules);

            foreach (var u in Settings.UrlRules)
            {
                flattenedTree.Add(u.Modifiers);
                flattenedTree.AddRange(u.Modifiers);
            }

            foreach (var n in flattenedTree)
            {
                var np = n as INotifyPropertyChanged;
                var nc = n as INotifyCollectionChanged;
                if (nc != null)
                {
                    nc.CollectionChanged -= OnCollectionChanged;
                    nc.CollectionChanged += OnCollectionChanged;
                }
                if (np != null)
                {
                    np.PropertyChanged -= OnPropertyChanged;
                    np.PropertyChanged += OnPropertyChanged;
                }
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnSettingsChanged(e);
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnSettingsChanged(e);
        }
    }
}
