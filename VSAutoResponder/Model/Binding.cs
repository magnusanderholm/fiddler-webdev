using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Fiddler.VSAutoResponder.Model
{
    public class Site
    {
        public Site(string name, DirectoryInfo directory, IEnumerable<Uri> bindings)
        {
            Name = name;
            Directory = directory;
            Bindings = bindings;
        }

        public IEnumerable<Uri> Bindings { get; private set; }

        public string Name { get; private set; }

        public DirectoryInfo Directory { get; private set; }
    }
}
