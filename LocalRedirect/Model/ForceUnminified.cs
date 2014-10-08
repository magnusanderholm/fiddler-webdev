namespace Fiddler.LocalRedirect.Config
{
    using System;
    using System.ComponentModel;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract(Name="forceunminified", Namespace="")]
    public class ForceUnminified : ChildSetting
    {                
        public ForceUnminified()
        {
        }
        

        public ForceUnminified(UrlRule parent)
            : base(parent)
        {                                 
        }        
                                       
        public override void RequestBefore(Session session)
        {
            base.RequestBefore(session);
            if (IsEnabled)
            {
                session.url = session.url.Replace(".min.css", ".css");
                session.url = session.url.Replace(".min.js", ".js");                
            }
        }                        
    }
}
