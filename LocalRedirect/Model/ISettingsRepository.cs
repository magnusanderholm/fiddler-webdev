using System;
using System.IO;
namespace Fiddler.LocalRedirect.Model
{
    public interface ISettingsRepository
    {
        System.IO.FileInfo CurrentFile { get; set; }
        IMostRecentlyUsed<FileInfo> Mru { get; }
        Settings Settings { get; }
        void Save(FileInfo fI);
        Settings Open(FileInfo fI);
    }
}
