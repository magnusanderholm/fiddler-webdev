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
        private IEventBus eventBus;

        public SettingsStorage(FileInfo defaultSettingsFile, IEventBus eventBus)
        {
            if (defaultSettingsFile == null)
                throw new ArgumentNullException("defaultSettingsFile");
            this.eventBus = eventBus;

            pC = new NotifyPropertyChanged(OnPropertyChanged);
            Mru = new MostRecentlyUsedFiles(10);
            if (!Mru.Any(f => string.Compare(f.FullName, defaultSettingsFile.FullName) == 0))
                Mru.Touch(defaultSettingsFile);

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


                    // Now we shall subscribe to any changes made in settings                        
                    // Traverse the settings tree and hookup the subscriptions on our new eventbus. Since we will be subscribing
                    // to INotifyPropertyChanged events and on NotifyCollecitonChangedEventArgs we only need to listen for such messages
                    // when they arrive we just make sure that we also subscribe on all those new items as well. That way the tree can grow
                    // dynamically and we do not really care about what new objects that removed/entered into the tree.
                    //settings.Events.SubscribeTo<object, NotifyCollectionChangedEventArgs>((s, e) => SaveSettingsToFile(CurrentStorage, Settings));
                    //settings.Events.SubscribeTo<object, NotifyCollectionChangedEventArgs>((s, e) => SaveSettingsToFile(CurrentStorage, Settings));
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
                        AttemptToPublishChangesForObjectOnEventBus(n);

                    // We are now listening to the tree as it is right this moment. Set up subscriptions. If we get a change in 
                    // one of the observable collections then we may need to react on that and make sure that the new item also publishes on the eventbus

                    eventBus.SubscribeTo<object, PropertyChangedEventArgs>((s, e) => OnSettingsChange(s, e));
                    eventBus.SubscribeTo<object, NotifyCollectionChangedEventArgs>((s, e) => OnSettingsChange(s, e));

                    // we have new settings. Publish it so all interested parties
                    // can read it.
                    eventBus.Publish(this, Settings);
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

        private void AttemptToPublishChangesForObjectOnEventBus(object n)
        {
            var np = n as INotifyPropertyChanged;
            var nc = n as INotifyCollectionChanged;
            if (nc != null)
                eventBus.PublishChanges(nc);
            if (np != null)
                eventBus.PublishChanges(np);            
        }

        private void OnSettingsChange(object sender, EventArgs e)
        {
            AttemptToPublishChangesForObjectOnEventBus(sender);
            SaveSettingsToFile(CurrentStorage, Settings);
            // Ok settings have changed so we publish a message about that for any listeners
            eventBus.Publish(this, Settings);
        }
    }
}
