namespace Fiddler.LocalRedirect.ViewModel
{
    using Fiddler.LocalRedirect.Model;
    using System.Collections.Generic;

    public interface IRedirectViewModel
    {
        ISettingsStorage SettingsStorage { get; }        
    }
}
