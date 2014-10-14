using Fiddler;
using Fiddler.LocalRedirect.Embedded;
using Fiddler.LocalRedirect.Model;
using Fiddler.LocalRedirect.ViewModel;
using System.IO;
using System.Windows.Forms;


[assembly: Fiddler.RequiredVersion("4.4.9.3")]


public class LocalRedirect : Fiddler.IAutoTamper2
{
    private readonly SettingsRepository settingsRepository =
        new SettingsRepository(new FileInfo(Path.Combine(Fiddler.CONFIG.GetPath("Root"), "localredirect.xml")));        
    private readonly UrlRuleSelector urlMatcher = new UrlRuleSelector();
    private readonly RedirectViewModel viewModel;

    static LocalRedirect()
    {
        var assemblyLoader = new EmbeddedAssemblyLoader();
        var loadedAssemblies = assemblyLoader.LoadEmbeddedAssemblies();
        int i = 0;
    }
    
    public LocalRedirect()
    {
        settingsRepository.Settings.Observer.Changed += (s, e) => urlMatcher.AssignSettings(settingsRepository.Settings);
        urlMatcher.AssignSettings(settingsRepository.Settings);
        viewModel = new RedirectViewModel(settingsRepository);
    }

    public void OnBeforeUnload()
    {        
    }

    public void OnLoad()
    {
        var view = new Fiddler.LocalRedirect.View.LocalRedirectHost() { ViewModel = viewModel };
           
        var oPage = new TabPage("Redirector");
        oPage.ImageIndex = (int)Fiddler.SessionIcons.Redirect;        
        oPage.Controls.Add(view);
        oPage.Padding = new Padding(0);
        view.Dock = DockStyle.Fill;        
        FiddlerApplication.UI.tabsViews.TabPages.Add(oPage);        
    }        

    public void OnPeekAtResponseHeaders(Fiddler.Session oSession)
    {
        urlMatcher.Get(oSession).PeekAtResponseHeaders();        
    }

    public void AutoTamperRequestAfter(Fiddler.Session oSession)
    {
        urlMatcher.Get(oSession).RequestAfter();        
    }

    public void AutoTamperRequestBefore(Fiddler.Session oSession)
    {        
        urlMatcher.Get(oSession).RequestBefore();                
    }

    public void AutoTamperResponseAfter(Fiddler.Session oSession)
    {
        urlMatcher.Get(oSession).ResponseAfter();        
    }

    public void AutoTamperResponseBefore(Fiddler.Session oSession)
    {
        urlMatcher.Get(oSession).ResponseBefore();        
    }

    public void OnBeforeReturningError(Fiddler.Session oSession)
    {
        // urlMatcher.Get(oSession).BeforeReturningError();        
    }
}

