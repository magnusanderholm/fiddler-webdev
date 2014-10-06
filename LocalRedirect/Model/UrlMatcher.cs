namespace Fiddler.LocalRedirect.Model
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Collections.Concurrent;


    // TODO Need to rewrite this
    //      1. We have a Setting class
    //      2.    Below the Setting class we have several more settings
    public class UrlMatcher
    {
        private readonly ConcurrentDictionary<Fiddler.Session, SessionModifier> map =
            new ConcurrentDictionary<Session, SessionModifier>();

        public UrlMatcher()
        {
        }


        private Settings settings;
        private object _lock = new object();

        public Settings Settings
        {
            get
            {
                lock (_lock)
                    return settings;

            }

            set
            {
                lock (_lock)
                    settings = value;
            }

        }        
        
        public SessionModifier Get(Session oSession)
        {            
            var redirect = map.GetOrAdd(oSession, s => 
            {
                oSession.OnCompleteTransaction += OnCompleteTransaction;
                var sessionModifier = SessionModifier.Empty;
                if (Settings != null && !s.HTTPMethodIs("CONNECT"))
                {
                    // Find best matching redirect rule (ie the one that is longest and matches the url in oSession). 
                    var settings = Settings;
                    var sessionUrl = s.fullUrl.ToLower();

                    // TODO If Redirects are ordered to begin with we can avoid some overhead here.
                    var orderedRedirects = settings.Redirects.OrderByDescending(r => r.FromUrl.Length).ToArray();
                    var bestMatch = orderedRedirects.FirstOrDefault(r => sessionUrl.StartsWith(r.FromUrl));
                    if (bestMatch != null)                    
                        sessionModifier = new SessionModifier(s, bestMatch);
                }

                return sessionModifier; 
            });
            
            return redirect;
        }

        private void OnCompleteTransaction(object sender, EventArgs e)
        {
            SessionModifier tmp = null;
            Fiddler.Session session = (Fiddler.Session)sender;
            map.TryRemove((Session)sender, out tmp);
            session.OnCompleteTransaction -= OnCompleteTransaction;
        }
    }
}
