﻿namespace Fiddler.LocalRedirect.Model
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract(Name = "disablecache", Namespace = "")]
    [Modifier(Order = -1, IsEnabled = true)] 
    [Serializable()]
    [XmlRoot(Namespace = "", ElementName = "disablecache")]
    public class DisableCache : ChildSetting
    {
        private DisableCache()
        {            
        }

        public DisableCache(UrlRule parent)
            : base(parent)
        {
            IsEnabled = true;                        
        }

        public override void ResponseBefore(Session session)
        {
            base.ResponseBefore(session);
            if (IsEnabled)            
            {
                session.oResponse.headers.Remove("Expires");
                session.oResponse["Cache-Control"] = "no-cache";
            }            
        }

        public override void RequestBefore(Session session)
        {
            base.RequestBefore(session);
            if (IsEnabled)
            {
                session.oRequest.headers.Remove("If-None-Match");
                session.oRequest.headers.Remove("If-Modified-Since");
                session.oRequest["Pragma"] = "no-cache";
            }            
        }
                                               
    }
}
