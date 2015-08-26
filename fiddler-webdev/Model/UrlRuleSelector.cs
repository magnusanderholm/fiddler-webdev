namespace Fiddler.Webdev.Model
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class UrlRuleSelector
    {        
        private readonly ConditionalWeakTable<Fiddler.Session, SessionModifier> map =
            new ConditionalWeakTable<Session, SessionModifier>();

        private static readonly SerializerEx<Settings> settingsSerializer = new SerializerEx<Settings>();        
        private Settings settings = null;
        

        public void AssignSettings(Settings settings)
        {
            if (settings != null)
            {
                lock (settingsSerializer)
                {
                    var settingsCopy = settingsSerializer.DeepCopy(settings);
                    this.settings = settingsCopy;
                }
            }
        }
                        
        public SessionModifier Get(Session oSession)
        {            
            // Method will be called from many threads so all accessed data must be thredsafe.
            // we therefore get the local copy of the current settings. 
            Settings _settings = null;
            lock (settingsSerializer)            
                _settings = settings;                        

            var redirect = map.GetValue(oSession, s => 
            {
                oSession.OnCompleteTransaction -= OnCompleteTransaction;
                oSession.OnCompleteTransaction += OnCompleteTransaction;
                var sessionModifier = SessionModifier.Empty;
                
                if (_settings != null)
                {
                    
                    var sessionUrl = new Uri(s.fullUrl.ToLower());                    
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
            Fiddler.Session session = (Fiddler.Session)sender;
            map.Remove(session);
            session.OnCompleteTransaction -= OnCompleteTransaction;
        }
    }
}
