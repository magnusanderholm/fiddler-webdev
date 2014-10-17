namespace Fiddler.LocalRedirect.Model
{
    using System;
    using System.ComponentModel;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract(Name="javascriptcombiner", Namespace="")]
    [Modifier(Order = 0, IsEnabled=false)] 
    [Serializable()]
    [XmlRoot(Namespace = "", ElementName = "javascriptcombiner")]
    public class JavascriptCombiner : Modifier
    {
        private JavascriptCombiner()
        {
            Initialize();
        }

        public JavascriptCombiner(UrlRule parent)
            : base(parent)
        {        
            // TODO This "should" be fairly simple
            // 1. Load page in html agility pack on response
            // 2. Find all javascript in head
            // 2.1 Optional. Find all javascript in page below head and place at bottom instead. (minified and combined of course)
            // 3. Keep track of prev loaded javascript url using a in mem/disk dictionary (could be a nosql db)
            // 4. If not in dictionary place it there and redownload contents into dictionary.            
            // 6. If at least on javascript did not exist. Create new SHA1 based on all urls in load.
            // 7. Regenerate a new combined javascript. Store on disk with SHA1 name.
            // 8. put link to SHA1.js in response.
            // 9. when client asks for SHA1.js set pretty high cache value on response so clien't doesn't try to redownload it.
            // Above algorithm has flaw in that it does not take into account
            // that the browser may actually download a javascript even if its
            // url hasn't changed because it wasn't in the cache or it expired. How do we handle that? Proxy must replicate
            // the caching functionality of the browser. But that won't help either because the proxy cannot know
            // if the client.
            // Hmm proxy could be active and continuosly poll all javascript url's??? That would not work if some of the scripts
            // are secured though cause the proxy would not be able to impersonate a user.
            // Only see one way and that is that the proxy has to query the server for all url's every time. Could potentially
            // save some time by looking at 

            // NO what happens if the server gives client different javascripts depending on who the user is. Same names but different 
            // content. No the only thing that could work is if the proxy always downloads content per response and never
            // tries to cache anything. That would however introduce a latency in the response that probably would be too large
        }        
                                       
        [OnDeserializing]
        private void DeserializationInitializer(StreamingContext ctx)
        {
            this.Initialize();
        }
        

        private void Initialize()
        {

        }
    }
}
