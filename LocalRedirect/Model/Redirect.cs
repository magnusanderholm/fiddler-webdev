using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;

namespace Fiddler.LocalRedirect.Model
{
    [DataContract(Name = "redirect", Namespace="")]    
    public class Redirect : ChildSetting
    {        
        private HostName toHost = new HostName("localhost:80");        
        private bool useMinified = false;                                

        public readonly static Redirect Empty = new Redirect();

        public Redirect()
        {
        }
        

        // host[:<port>]
        [DataMember(Name = "tohost", IsRequired = true, EmitDefaultValue = true), DefaultValue("localhost")]
        public string ToHost
        {
            // TODO Throw exception if host name contains invalid values.
            get { return toHost.ToString(); }
            set { Update<HostName>(ref toHost, new HostName(value), "ToHost", "ToPort"); }
        }                
       

        public bool CanRedirect
        {
            get 
            { 
                return 
                    (UrlRule.Url.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase) ||
                    UrlRule.Url.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase)) &&
                    !string.IsNullOrEmpty(ToHost); 
            }
        }

        [DataMember(Name="useminified", EmitDefaultValue=true), DefaultValue(false)]
        public bool ForceUnminified 
        {
            get { return useMinified; }
            set { Update(ref useMinified, value, "UseMinified"); }
        }        
                        
    }
}
