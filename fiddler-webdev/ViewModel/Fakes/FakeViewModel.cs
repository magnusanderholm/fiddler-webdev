namespace Fiddler.Webdev.ViewModel.Fakes
{
    using Fiddler.Webdev.Model;
    using System.Collections.Generic;
    using System.IO;

    public class FakeViewModel : IRedirectViewModel
    {

        public FakeViewModel()
        {
            SettingsStorage = new FakeSettingsStorage();             
            var mruCollection = new List<FileInfo>();
            mruCollection.Add(new FileInfo("c:\f1.config"));
            mruCollection.Add(new FileInfo("c:\f2.config"));
            mruCollection.Add(new FileInfo("c:\f3.config"));
            Mru = new MostRecentlyUsed<FileInfo>(mruCollection, 10, null);
        }

        public ISettingsStorage SettingsStorage { get; private set; }


        public IEnumerable<FileInfo> Mru { get; private set;}
    }
}
