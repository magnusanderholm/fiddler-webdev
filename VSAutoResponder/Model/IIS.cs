using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fiddler.VSAutoResponder.Model
{
    public class IIS
    {
        public IEnumerable<Site> GetSites(Uri url)
        {
            using (var mgr = ServerManager.OpenRemote("localhost"))
            {
                return
                    (from s in mgr.Sites
                     let bindings =
                         (from b in s.Bindings
                          where string.Compare(url.Host, b.Host, true) == 0 && string.Compare(url.Scheme, b.Protocol, true) == 0 && url.Port == b.EndPoint.Port
                          select new Uri(b.Protocol + "://" + b.Host + ":" + b.EndPoint.Port)).ToArray()
                     where bindings.Length > 0
                     select new Site(s.Name, null, bindings)).ToArray();
            }
        }

        //public IEnumerable<Site> GetSites(Uri url)
        //{
        //    System.Diagnostics.Debugger.Break();
        //    // Get the sites section from the AppPool.config 
        //    Microsoft.Web.Administration.ConfigurationSection sitesSection =
        //        Microsoft.Web.Administration.WebConfigurationManager.GetSection(null, null, "system.applicationHost/sites");

        //    foreach (Microsoft.Web.Administration.ConfigurationElement site in sitesSection.GetCollection())
        //    {
        //        // For each binding see if they are http based and return the port and protocol 
        //        foreach (Microsoft.Web.Administration.ConfigurationElement binding in site.GetCollection("bindings"))
        //        {
        //            string protocol = (string)binding["protocol"];
        //            string bindingInfo = (string)binding["bindingInformation"];
        //            yield return new Site("foo", null, null);
        //            //if (protocol.StartsWith("http", StringComparison.OrdinalIgnoreCase))
        //            //{
        //            //    string[] parts = bindingInfo.Split(':');
        //            //    if (parts.Length == 3)
        //            //    {
        //            //        string port = parts[1];
        //            //        yield return new KeyValuePair<string, string>(protocol, port);
        //            //    }
        //            //}
        //        }                
        //    }
        //}
    }
}
