namespace Fiddler.LocalRedirect.Model
{
    using System;
    using System.IO;

    public interface ISettingsStorage
    {
        FileInfo CurrentStorage { get; set; }
        IMostRecentlyUsed<FileInfo> Mru { get; }
        Settings Settings { get; }        
    }
}
