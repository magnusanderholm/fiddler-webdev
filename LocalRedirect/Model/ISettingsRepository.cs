namespace Fiddler.LocalRedirect.Model
{
    using System;
    using System.IO;

    public interface ISettingsRepository
    {
        FileInfo CurrentStorage { get; set; }
        IMostRecentlyUsed<FileInfo> Mru { get; }
        Settings Settings { get; }        
    }
}
