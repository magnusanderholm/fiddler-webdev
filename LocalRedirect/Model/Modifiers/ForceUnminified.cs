namespace Fiddler.LocalRedirect.Model
{
    using System;
    using System.ComponentModel;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    // TODO Rewrite this plugin to keep a set of 
    //      search and replace rules instead
    //      that way we can modify the file part of a url
    //      instead using regular expressions.
    [DataContract(Name="forceunminified", Namespace="")]
    [Modifier(Order = 2, IsEnabled = true)] 
    [Serializable()]
    [XmlRoot(Namespace = "", ElementName = "forceunminified")]
    public class ForceUnminified : Modifier
    {
        private ForceUnminified()
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
                // TODO Convert to a URL instead. Read PathAndQuery and then
                //      the file part. This is what we need to perform replace on.
                //      We shall keep a list of standard regular expressions that might come in handy.
                session.url = session.url.Replace(".min.css", ".css");
                session.url = session.url.Replace(".min.js", ".js");                
            }
        }                        
    }
}
