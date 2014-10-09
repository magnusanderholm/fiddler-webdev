namespace Fiddler.LocalRedirect.Config
{
    using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

    // TODO Rewrite this plugin to keep a set of 
    //      search and replace rules instead
    //      that way we can modify the file part of a url
    //      instead using regular expressions.
    [DataContract(Name="forcesharepointdebugjavascript", Namespace="")]
    public class ForceSharepointDebugJavascript : ChildSetting
    {
        private static Regex regex = new Regex(@"\.(min\.)?js", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public ForceSharepointDebugJavascript()
        {
        }


        public ForceSharepointDebugJavascript(UrlRule parent)
            : base(parent)
        {                                 
        }        
                                       
        public override void RequestBefore(Session session)
        {
            base.RequestBefore(session);
            if (Parent.IsEnabled && IsEnabled)
            {                
                session.url = regex.Replace( session.url, ".debug.js");                
            }
        }                        
    }
}
