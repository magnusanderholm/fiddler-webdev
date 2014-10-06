using Fiddler.LocalRedirect.Config;
using Fiddler.LocalRedirect.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Fiddler.LocalRedirect.ViewModel
{
    public class RedirectViewModel
    {        

        public RedirectViewModel(Settings settings)
        {
            var _col = new ObservableItemCollection<Config.UrlRule>();
            UrlRules = _col;
            foreach (var urlRule in settings.UrlRules)
                UrlRules.Add(urlRule);
            _col.CollectionChanged += OnCollectionChanged;
            Settings = settings;
        }

        public Settings Settings { get; private set; }

        private void OnCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Settings.UrlRules = UrlRules.ToArray();
        }
        
        // TODO Ensure that we cannot add duplicates!!!! Porably best to put that in the custom ObservableCollection class.
        public ICollection<Config.UrlRule> UrlRules { get; private set; }


        public Config.UrlRule Create()
        {
            return Config.UrlRule.CreateDefault();
        }
        // TODO Add Clear method for example.
    }
}
