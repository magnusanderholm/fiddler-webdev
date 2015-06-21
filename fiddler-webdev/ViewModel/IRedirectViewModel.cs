namespace Fiddler.Webdev.ViewModel
{
    using Fiddler.Webdev.Model;
    using System.Collections.Generic;
    using System.IO;

    public interface IRedirectViewModel
    {
        ISettingsStorage SettingsStorage { get; }

        IEnumerable<FileInfo> Mru { get;}
    }
}
