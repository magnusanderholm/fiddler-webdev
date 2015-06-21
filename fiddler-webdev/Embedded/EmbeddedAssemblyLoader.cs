namespace Fiddler.Webdev.Embedded
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class EmbeddedAssemblyLoader : IEnumerable<Assembly>
    {
        private readonly IList<Assembly> embeddedAssemblies = new List<Assembly>();
        
        public EmbeddedAssemblyLoader(string @namespace)
        {            
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
            
            // Load all dll embedded resources that exists in the given @namespace (or subnamespaces)            
            foreach (var res in GetType().Assembly.GetEmbeddedResources())
            {
                if (res.Name.StartsWith(@namespace) && res.Name.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase))
                {
                    using (var s = res.Open())
                    {
                        var dllContent = new byte[s.Length];
                        s.Read(dllContent, 0, dllContent.Length);
                        embeddedAssemblies.Add(Assembly.Load(dllContent));
                    }
                }
            }
        }

        private Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {            
            return embeddedAssemblies.FirstOrDefault(a => a.FullName == args.Name);            
        }

        public IEnumerator<Assembly> GetEnumerator()
        {
            return embeddedAssemblies.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
