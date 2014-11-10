namespace Fiddler.LocalRedirect.Model
{
    using System;
    using System.IO;

    public interface ISettingsStorage
    {
        FileInfo CurrentStorage { get; set; }       
        Settings Settings { get; }        
    }
}
