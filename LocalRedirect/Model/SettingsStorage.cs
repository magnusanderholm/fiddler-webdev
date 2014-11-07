namespace Fiddler.LocalRedirect.Model
{
    using Fiddler.LocalRedirect.Model;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;

    public class SettingsStorage : INotifyPropertyChanged, Fiddler.LocalRedirect.Model.ISettingsStorage
    {
        private readonly SerializerEx<Settings> settingsSerializer = new SerializerEx<Settings>();
        private Settings settings;                
        private FileInfo currentStorage = new FileInfo(Guid.NewGuid().ToString("N"));
        private NotifyPropertyChanged pC;

        private static readonly ILogger logger = LogManager.CreateCurrentClassLogger();
        private static readonly IEventBus eventBus = EventBusManager.Get();

        public SettingsStorage(FileInfo defaultSettingsFile)
        {
            if (defaultSettingsFile == null)
                throw new ArgumentNullException("defaultSettingsFile");
            
            pC = new NotifyPropertyChanged(OnPropertyChanged);
            Mru = new MostRecentlyUsedFiles(10);
            if (!Mru.Any(f => string.Compare(f.FullName, defaultSettingsFile.FullName) == 0))
                Mru.Touch(defaultSettingsFile);

            CurrentStorage = Mru.First();                    
        }        

        public IMostRecentlyUsed<FileInfo> Mru { get; private set; }
        
        public Settings Settings { get { return settings; } }

        public FileInfo CurrentStorage
        {
            get { return currentStorage; }
            set 
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                if (string.Compare(value.FullName, currentStorage.FullName) != 0)
                {
                    // Look on disk if file exists. If so load it.
                    // TODO Create NEW messagebus and feed it to newSettings. Resubscribe on the new messagebus. This should solve
                    //      the serialization/deserialization issue.
                    Settings newSettings = value.Exists ? LoadSettingsFromFile(value) : new Settings();      
                    var notifyChanged = pC.Update(ref currentStorage, value);
                    if (notifyChanged.IsChanged)
                    {
                        Mru.Touch(CurrentStorage); 
                        settings = newSettings;
                        notifyChanged.Extra("Settings");
                        
                        // Now we shall subscribe to any changes made in settings
                        settings.Events.SubscribeTo<object, NotifyCollectionChangedEventArgs>((s, e) => SaveSettingsToFile(CurrentStorage, Settings));
                        settings.Events.SubscribeTo<object, NotifyCollectionChangedEventArgs>((s, e) => SaveSettingsToFile(CurrentStorage, Settings));
                    }
                }
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs pCe)
        {
            var h = PropertyChanged;
            if (h != null)
                h(this, pCe);
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
