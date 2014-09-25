using Fiddler;
using Fiddler.VSAutoResponder.Model;
using System.IO;
using System.Windows.Forms;


[assembly: Fiddler.RequiredVersion("4.4.9.3")]


public class LocalRedirect : Fiddler.IAutoTamper2
{        
    private readonly RedirectEngine redirectEngine =  new RedirectEngine();
    private readonly SettingsRepository settingsRepository = 
        new SettingsRepository(new FileInfo(Path.Combine(Fiddler.CONFIG.GetPath("Root"), "localredirect.xml")));
    
    public LocalRedirect()
    {
        
    }

    public void OnBeforeUnload()
    {        
    }

    public void OnLoad()
    {        
        var oPage = new TabPage("Redirector");
        oPage.ImageIndex = (int)Fiddler.SessionIcons.Redirect;
        var view = new Fiddler.VSAutoResponder.View.LocalRedirectSettings();        
        oPage.Controls.Add(view);
        oPage.Padding = new Padding(0);
        view.Dock = DockStyle.Fill;
        FiddlerApplication.UI.tabsViews.TabPages.Add(oPage);

        view.Settings = settingsRepository.Settings;

        settingsRepository.Settings.Changed += (s, e) => AssingSettingsToRedirectEngine();
        AssingSettingsToRedirectEngine();                       
    }

    private void AssingSettingsToRedirectEngine()
    {
        redirectEngine.Settings = settingsRepository.CopySettings();
    }


    public void OnPeekAtResponseHeaders(Fiddler.Session oSession)
    {
    }

    public void AutoTamperRequestAfter(Fiddler.Session oSession)
    {        
    }

    public void AutoTamperRequestBefore(Fiddler.Session oSession)
    {
        redirectEngine.TryRedirect(oSession);
    }

    public void AutoTamperResponseAfter(Fiddler.Session oSession)
    {

    }

    public void AutoTamperResponseBefore(Fiddler.Session oSession)
    {

    }

    public void OnBeforeReturningError(Fiddler.Session oSession)
    {

    }
}

