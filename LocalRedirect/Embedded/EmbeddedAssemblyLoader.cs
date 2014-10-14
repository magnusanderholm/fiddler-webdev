namespace Fiddler.LocalRedirect.Embedded
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public class EmbeddedAssemblyLoader
    {
        public EmbeddedAssemblyLoader()
        {
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
                using (var mS = new MemoryStream((int)s.Length))
                {
                    s.CopyTo(mS);
                    mS.Position = 0;
                    loadedAssembly = Assembly.Load(mS.ToArray());
                }            
            }
                
            return loadedAssembly;
        }
    }
}
