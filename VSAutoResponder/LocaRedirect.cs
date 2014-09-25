using Fiddler;
using Fiddler.VSAutoResponder.Model;
using System.Windows.Forms;


[assembly: Fiddler.RequiredVersion("4.4.9.3")]


public class DevRedirector : Fiddler.IAutoTamper2
{        
    private readonly RedirectEngine redirectEngine = new RedirectEngine(new Fiddler.VSAutoResponder.Model.Settings());
    public DevRedirector()
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

        view.Settings = redirectEngine.Settings;        
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
        // TODO Check if we are decrypting SSL or not. If not do nothing

        // Look at configuration. If there is a mapping between host -> localhost 
        // check if the file actually exists (hmm can we really do that no cause it could be a MVC auto generated file).
        // would really like to query site first if possible..... should be possible if we can use a http request that just checks for
        // existance (like HEAD)

        // TODO Use RedirectEngineInstance to determine if we shall redirect or not and to what url we shall redirect
        //      in that case.

        //Session.bypassGateway = true;                   // Prevent this request from going through an upstream proxy
        //oSession["x-overrideHost"] = "128.123.133.123";  // DNS name or IP address of target server

        //if (string.Compare(oSession.host, "veidekkeintra.blob.core.windows.net", true) == 0)
        //{
        //    if (!oSession.HTTPMethodIs("CONNECT")) // Allow CONNECTS to go to intended host otherwise the CONNECT will fail.
        //    {
        //        // if site exists on localhost set the following
        //        oSession.bypassGateway = true;                   // Prevent this request from going through an upstream proxy
        //        oSession["x-overrideHost"] = "127.0.0.1"; 
                
        //        // oSession.host = "dev.blob";
        //        oSession.oRequest.headers.UriScheme = "http"; // TODO Get this from binding information. NO we always use http locally. simplifies things a lot.
        //        oSession.url = oSession.url.Replace(".min.css", ".css");
        //        oSession.url = oSession.url.Replace(".min.js", ".js");                
        //        // TODO Write code to replace all .min scripts with unminified.
        //    }                            
        //}
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

