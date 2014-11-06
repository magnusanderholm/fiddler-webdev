using Fiddler;
using Fiddler.LocalRedirect.Embedded;
using Fiddler.LocalRedirect.Model;
using Fiddler.LocalRedirect.ViewModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;


[assembly: Fiddler.RequiredVersion("4.4.9.3")]


public class LocalRedirect : Fiddler.IAutoTamper2
{
    private readonly SettingsRepository settingsRepository =
        new SettingsRepository(new FileInfo(Path.Combine(Fiddler.CONFIG.GetPath("Root"), "localredirect.xml")));        
    private readonly UrlRuleSelector urlMatcher = new UrlRuleSelector();
    private readonly RedirectViewModel viewModel;
    private static readonly EmbeddedAssemblyLoader assemblyLoader;
    private static readonly ILogger logger = LogManager.CreateCurrentClassLogger();
    private static readonly IEventBus eventBus = EventBusManager.Get();

    static LocalRedirect()
    {
        assemblyLoader = new EmbeddedAssemblyLoader();               
    }
    
    public LocalRedirect()
    {
        //settingsRepository.Settings.Observer.Changed += (s, e) => urlMatcher.AssignSettings(settingsRepository.Settings);
        //urlMatcher.AssignSettings(settingsRepository.Settings);
        viewModel = new RedirectViewModel(settingsRepository);
        
        eventBus.SubscribeTo<Settings, object>(OnSettingsChanged);
        eventBus.SubscribeTo<ModifierBase, object>(OnSettingsChanged);
    }

    private void OnSettingsChanged(object sender, object msg)
    {
        Settings settings = sender as Settings;
        if (settings == null)        
            settings = sender is UrlRule ? (sender as UrlRule).Parent : (sender as Modifier).Parent.Parent;

        urlMatcher.AssignSettings(settings);
        settingsRepository.Save(settings);
    }

    private void OnWebSocketsMessage(object sender, WebSocketMessageEventArgs e)
    {
        logger.Info(() => "Web socket message received.");        
    }

    public void OnBeforeUnload()
    {        
    }

    public void OnLoad()
    {
        FiddlerApplication.OnWebSocketMessage += OnWebSocketsMessage;
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

