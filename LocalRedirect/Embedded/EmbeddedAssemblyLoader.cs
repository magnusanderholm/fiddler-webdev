namespace Fiddler.LocalRedirect.Embedded
{
    using System;
    using System.Linq;
    using System.Reflection;

    public class EmbeddedAssemblyLoader
    {
        public EmbeddedAssemblyLoader()
        {
            // TODO Rewrite so we load all assemblies immediatly and then we only
            //      look in AppDomain.CurrentDomain.GetAssemblies() and return the correct one
            //      in OnAssemblyResolve(). Will probably give us a little performance boost if
            //      we need to load lots of assemblies.
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
        }                

        private Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {            
            Assembly loadedAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name); ;
            if (loadedAssembly != null)
                return loadedAssembly;
            var assemblyName = args.Name.Split(',')[0] + ".dll";
            var dll = GetType()
                .Assembly
                .GetEmbeddedResources()
                .FirstOrDefault(e => e.Name.EndsWith(assemblyName, StringComparison.InvariantCultureIgnoreCase));
            
            if (dll != null)
            {
                using (var s = dll.Open())                
                {
                    var dllContent = new byte[s.Length];
                    s.Read(dllContent, 0, dllContent.Length);
                    loadedAssembly = Assembly.Load(dllContent);
                }            
            }
                
            return loadedAssembly;
        }
    }
}
