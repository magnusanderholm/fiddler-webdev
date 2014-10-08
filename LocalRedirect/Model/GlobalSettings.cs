using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Fiddler.LocalRedirect.Model
{
    [DataContract(Name = "globalsettings", Namespace = "")]
    public class GlobalSettings : INotifyPropertyChanged
    {
        protected NotifyPropertyChanged pC;
        private ICollection<FileInfo> recentlyUsedConfigurations;            

        public GlobalSettings()
        {
            Initialize();
        }

        [DataMember(Name = "recentlyusedconfigurations", IsRequired = false), DefaultValue(false)]
        public ICollection<FileInfo> RecentlyUsedConfigurations
        {
            get { return this.recentlyUsedConfigurations; }
            set { pC.Update(ref recentlyUsedConfigurations, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs pCe)
        {
            var h = PropertyChanged;
            if (h != null)
                h(this, pCe);
        }

        private void Initialize()
        {
            pC = new NotifyPropertyChanged(OnPropertyChanged);
            recentlyUsedConfigurations = new ObservableCollection<FileInfo>();
        }

        [OnDeserializing]
        private void DeserializationInitializer(StreamingContext ctx)
        {
            this.Initialize();
        }

    }
}
