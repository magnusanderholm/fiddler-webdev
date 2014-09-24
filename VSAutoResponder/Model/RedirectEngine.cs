using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fiddler.VSAutoResponder.Model
{
    public class RedirectEngine
    {
        public RedirectEngine()
        {
        }

        private Settings Settings {get; set;}

        public bool TryRedirect(string absoluteUrl, out Uri redirectedUrl)
        {
            return TryRedirect(new Uri(absoluteUrl, UriKind.Absolute), out redirectedUrl);
        }

        public bool TryRedirect(Uri absoluteUrl, out Uri redirectedUrl)
        {
            redirectedUrl = new Uri("dummy");
            // TODO Make actual webcall to see if we can redirect or not. Cache the result for quick lookup.
            return false;
        }
    }
}
