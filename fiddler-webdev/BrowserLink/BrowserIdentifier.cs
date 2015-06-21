namespace Fiddler.Webdev.BrowserLink
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Configuration;

    public class BrowserIdentifier
    {
        private static readonly Regex OperaRegex = new Regex("OPR/\\d+\\.\\d+\\.\\d+\\.\\d+", RegexOptions.Compiled);
        private static readonly Regex IEMobileAgentRegex = new Regex("IEMobile/[\\d\\.]+", RegexOptions.Compiled);
       
        const string BrowserName_IEMobile = "Internet Explorer (Windows Phone)";
        const string BrowserName_InternetExplorer = "Internet Explorer";
        const string BrowserName_MobileFormat = "{0} ({1})";
        const string BrowserName_Opera = "Opera";
        const string BrowserName_Unknown = "Unknown Browser";


        public BrowserIdentifier(string userAgent)
        {
            //userAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.10) " +
            //    "Gecko/20100914 Firefox/3.6.10";
            var browser = new HttpBrowserCapabilities
            {
                Capabilities = new Hashtable { { string.Empty, userAgent } }
            };
            var factory = new BrowserCapabilitiesFactory();
            factory.ConfigureBrowserCapabilities(new NameValueCollection(), browser);
            Id = FormatBrowserName(browser, userAgent);
        }

        public string Id { get; private set; }


        private static string FormatBrowserName(HttpBrowserCapabilities browser, string userAgent)
        {
            if (string.IsNullOrWhiteSpace(browser.Browser))
                return BrowserName_Unknown;
            if (userAgent != null && IEMobileAgentRegex.Match(userAgent).Success)            
                return BrowserName_IEMobile;            
            if (browser.IsMobileDevice)            
                return string.Format(BrowserName_MobileFormat, browser.Browser, browser.MobileDeviceModel);            
            if (browser.Browser.Equals("IE") || browser.Browser.Equals("InternetExplorer"))            
                return BrowserName_InternetExplorer;            
            if (userAgent != null && OperaRegex.Match(userAgent).Success)            
                return BrowserName_Opera;            
            return browser.Browser;
        }
    }
        
}
