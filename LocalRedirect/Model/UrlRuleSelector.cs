namespace Fiddler.LocalRedirect.Model
{
    using Fiddler.LocalRedirect.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;    
    
    public class UrlRuleSelector
    {
        // TODO Can probably replace with a thread storage dictionary instead to avoid locking.
        private readonly ConcurrentDictionary<Fiddler.Session, SessionModifier> map =
            new ConcurrentDictionary<Session, SessionModifier>();

        private static readonly SerializerEx<Settings> settingsSerializer = new SerializerEx<Settings>();
        private Settings settings = null;

        public UrlRuleSelector()
        {
        }                

        public void AssignSettings(Settings settings)
        {            
            lock (settingsSerializer)
            {
                var settingsCopy = settingsSerializer.DeepCopy(settings);
                this.settings = settingsCopy;                
            }            
        }
                        
        public SessionModifier Get(Session oSession)
        {            
            // Method will be called from many threads so all accessed data must be thredsafe.
            // we therefore get the local copy of the current settings. 
            Settings _settings = null;
            lock (settingsSerializer)            
                _settings = settings;                        

            var redirect = map.GetOrAdd(oSession, s => 
            {
                oSession.OnCompleteTransaction += OnCompleteTransaction;
                var sessionModifier = SessionModifier.Empty;
                
                if (_settings != null)
                {
                    // Find best matching redirect rule (ie the one that is longest and matches the url in oSession).                     
                    var sessionUrl = new Uri(s.fullUrl.ToLower());

                    // Notice that we treat HTTPS CONNECT a little bit different. To match a HTTPS CONNECT we know that 
                    // we must have a matching urlrule for a https://* url. Now the CONNECT comes in the form of http://<host>:443.
                    // To get a match on that against our current rules we simply replace http:// with https:// and see if 
                    // we get a match. If we do get a match we fake the tunnel.
                    bool isHttpsConnect = s.HTTPMethodIs("CONNECT") && sessionUrl.Scheme == "http";
                                                                                    
                    // TODO If Redirects are ordered to begin with we can avoid some overhead here.
                    var orderedRedirects = _settings.UrlRules.Where(r => r.IsValid && r.IsEnabled).OrderByDescending(r => r.UrlString.Length).ToArray();
                    var bestMatch = orderedRedirects.FirstOrDefault(r => r.IsMatch(sessionUrl, isHttpsConnect));
                    if (bestMatch != null) 
                    {
                        var modifiers = new List<ISessionModifier>();
                        modifiers.Add(bestMatch);
                        if (isHttpsConnect)                        
                            modifiers.Add(new FakeHTTPSTunnel());                        
                        else                        
                            modifiers.AddRange(bestMatch.Modifiers);
                                                                        
                        sessionModifier = new SessionModifier(s, modifiers);
                    }                        
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
