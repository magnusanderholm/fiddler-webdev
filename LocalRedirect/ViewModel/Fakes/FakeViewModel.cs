namespace Fiddler.LocalRedirect.ViewModel.Fakes
{
    using Fiddler.LocalRedirect.Model;
    using System.Collections.Generic;

    public class FakeViewModel
    {

        public FakeViewModel()
        {
            SettingsRepository = new FakeSettingsRepository();
            UrlRules = SettingsRepository.Settings.UrlRules;
        }

        public ISettingsRepository SettingsRepository { get; private set; }        
                
        public IEnumerable<Model.UrlRule> UrlRules { get; private set; }                       
    }
}
