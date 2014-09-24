using Fiddler;
using Fiddler.VSAutoResponder.Model;
using System.Windows.Forms;


[assembly: Fiddler.RequiredVersion("4.4.9.3")]


public class DevRedirector : Fiddler.IAutoTamper2
{
    private HostFile hostsFile = null;
    public DevRedirector()
    {
    }

    public void OnBeforeUnload()
    {
        hostsFile.Dispose();
    }

    public void OnLoad()
    {
        hostsFile = new HostFile(); // Triggers admin alert.
        var oPage = new TabPage("LocalRedirect");
        oPage.ImageIndex = (int)Fiddler.SessionIcons.Timeline;
        var view = new Fiddler.VSAutoResponder.View.LocalRedirectSettings();        
        oPage.Controls.Add(view);        
        view.Dock = DockStyle.Fill;
        FiddlerApplication.UI.tabsViews.TabPages.Add(oPage);

        view.Settings = new Fiddler.VSAutoResponder.Model.Settings();
    }

    public void OnPeekAtResponseHeaders(Fiddler.Session oSession)
    {
    }

    public void AutoTamperRequestAfter(Fiddler.Session oSession)
    {        
    }

    public void AutoTamperRequestBefore(Fiddler.Session oSession)
    {
        // TODO Check if we are decrypting SSL or not. If not do nothing

        // Look at configuration. If there is a mapping between host -> localhost 
        // check if the file actually exists (hmm can we really do that no cause it could be a MVC auto generated file).
        // would really like to query site first if possible..... should be possible if we can use a http request that just checks for
        // existance (like HEAD)

        if (string.Compare(oSession.host, "veidekkeintra.blob.core.windows.net", true) == 0)
        {
            if (!oSession.HTTPMethodIs("CONNECT")) // Allow CONNECTS to go to intended host otherwise the CONNECT will fail.
            {
                oSession.host = "dev.blob";
                oSession.oRequest.headers.UriScheme = "http";
                oSession.url = oSession.url.Replace(".min.css", ".css");
                oSession.url = oSession.url.Replace(".min.js", ".js");                
                // TODO Write code to replace all .min scripts with unminified.
            }                            
        }
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

