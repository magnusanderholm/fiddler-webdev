namespace Fiddler.LocalRedirect.Embedded
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public static class EmbeddedResourceExtensions
    {
        public static IEnumerable<EmbeddedResource> GetEmbeddedResources(this Assembly assembly)
        {
            return assembly.GetManifestResourceNames().Select(name => new EmbeddedResource(name, assembly));
        }
    }

    public class EmbeddedResource
    {
        public EmbeddedResource(string name, Assembly assembly)
        {
            Name = name;
            Assembly = assembly;
        }

        public string Name { get; private set; }

        public Assembly Assembly { get; private set; }

        public Stream Open()
        {            
            return Assembly.GetManifestResourceStream(Name);
        }
    }
}
