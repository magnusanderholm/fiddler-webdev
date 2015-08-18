using Fiddler;
using Fiddler.Webdev.Embedded;
using Fiddler.Webdev.Model;
using Fiddler.Webdev.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

[assembly: Fiddler.RequiredVersion("4.4.9.3")]


public class WebdevPlugin : Fiddler.IAutoTamper2
{
    private readonly SettingsStorage settingsStorage;        
    private readonly UrlRuleSelector urlMatcher = new UrlRuleSelector();
    private readonly WebdevViewModel viewModel;
    private static readonly EmbeddedAssemblyLoader assemblyLoader;
    private static readonly ILogger logger = LogManager.CreateCurrentClassLogger();    
    private readonly IMostRecentlyUsed<FileInfo> mru;
    private readonly RegistrySetting<FileInfo[]> mruStorage;

    static WebdevPlugin()
    {
        assemblyLoader = new EmbeddedAssemblyLoader(typeof(EmbeddedAssemblyLoader).Namespace);
    }
    
    public WebdevPlugin()
    {
        mruStorage = new RegistrySetting<FileInfo[]>(
            "Software\\FiddlerExtensions",
            "mru",
            new FileInfo[] { new FileInfo(Path.Combine(Fiddler.CONFIG.GetPath("Root"), "webdev.xml")) },
            str => str.Split(';').Select(s => new FileInfo(s)).ToArray(),
            files => string.Join(";", files.Select(f => f.FullName).ToArray()));
        
        var mruCollection = mruStorage.Get().ToObservableCollection();
        this.mru = new MostRecentlyUsed<FileInfo>(mruCollection, 10, null);
        mruCollection.CollectionChanged += (s, e) => mruStorage.Set(((IEnumerable<FileInfo>)s).ToArray());
        settingsStorage = new SettingsStorage(mru);
        settingsStorage.SettingsChanged += OnSettingsChanged;
        urlMatcher.AssignSettings(settingsStorage.Settings);
                
        viewModel = new WebdevViewModel(settingsStorage, mruCollection);                        
    }

    private void OnSettingsChanged(object sender, EventArgs e)
    {
        var settingsStorage = (SettingsStorage)sender;
        urlMatcher.AssignSettings(settingsStorage.Settings);
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
        var view = new Fiddler.Webdev.View.WebdevHost() { ViewModel = viewModel };
           
        var oPage = new TabPage("Webdev");
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

