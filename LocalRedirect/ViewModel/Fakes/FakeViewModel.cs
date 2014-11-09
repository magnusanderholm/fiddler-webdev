namespace Fiddler.LocalRedirect.ViewModel.Fakes
{
    using Fiddler.LocalRedirect.Model;

    public class FakeViewModel
    {

        public FakeViewModel()
        {
            SettingsRepository = new FakeSettingsRepository();            
        }

        public ISettingsStorage SettingsRepository { get; private set; }                                
    }
}
