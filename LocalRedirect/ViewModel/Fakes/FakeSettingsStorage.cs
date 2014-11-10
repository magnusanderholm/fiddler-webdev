namespace Fiddler.LocalRedirect.ViewModel.Fakes
{
    using Fiddler.LocalRedirect.Model;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;


    class FakeSettingsStorage : ISettingsStorage
    {
        
        public FakeSettingsStorage()
        {
            CurrentStorage = new FileInfo(@"c:\temp\dummy.txt");
            Mru = new FakeMostRecentlyUsed();
            Settings = new Settings();
            var scheme = new string[2] {"http", "https"};
            for (int i = 0; i < 10; ++i)
            {
                var urlRule = Settings.UrlRuleFactory.Create();
                urlRule.IsEnabled = i % 2 == 0;
                urlRule.UrlString = new Uri( scheme[i % 2] + "://dummy/" + i.ToString()).ToString();
                Settings.UrlRules.Add(urlRule);
            }                            
        }

        public System.IO.FileInfo CurrentStorage { get; set; }

        public IMostRecentlyUsed<FileInfo> Mru { get; private set; }

        public Settings Settings { get; private set; }


        public void Save(FileInfo fI)
        {            
        }

        public Settings Open(FileInfo fI)
        {
            return Settings;
        }

        class FakeMostRecentlyUsed : IMostRecentlyUsed<FileInfo>
        {
            private FileInfo[] files = new FileInfo[] { new FileInfo(@"c:\temp\1.conf"), new FileInfo(@"c:\temp\2.conf") };
            public void Touch(FileInfo item)
            {                
            }

            public IEnumerator<FileInfo> GetEnumerator()
            {
                return files.AsEnumerable<FileInfo>().GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return files.GetEnumerator();
            }            
        }
    }
}
