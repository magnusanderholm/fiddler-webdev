namespace Fiddler.LocalRedirect.ViewModel.Fakes
{
    using Fiddler.LocalRedirect.Model;

    public class FakeViewModel : IRedirectViewModel
    {

        public FakeViewModel()
        {
            SettingsStorage = new FakeSettingsRepository();            
        }

        public ISettingsStorage SettingsStorage { get; private set; }                                
    }
}
