namespace Fiddler.LocalRedirect.ViewModel
{
    using Fiddler.LocalRedirect.Model;
    using System.Collections.Generic;
    using System.IO;

    public interface IRedirectViewModel
    {
        ISettingsStorage SettingsStorage { get; }

        IEnumerable<FileInfo> Mru { get;}
    }
}
