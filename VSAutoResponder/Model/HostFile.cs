using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Fiddler.VSAutoResponder.Model
{
    public class HostFile : IDisposable
    {
        public HostFile()
        {
            // Hmm can we spawn a administrative process for thist that we can reuse????
            // that would be the simplest thing. Perhaps we can compile a temp exe
            // that is then launched???? Then we communicate with it using pipes or something similiar.
        }

        public void Add(string host, string ip)
        {
        }

        public void Remove(string host)
        {
        }

        public void Dispose()
        {
            // TODO Remove all added host entries.
        }
    }
}
