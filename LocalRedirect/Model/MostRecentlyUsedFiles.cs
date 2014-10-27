using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Fiddler.LocalRedirect.Model
{    
    public class MostRecentlyUsedFiles : MostRecentlyUsed<FileInfo>
    {
        const string registryPath = "Software\\FiddlerExtensions";
        const string mruKey = "mru";        
        private static string separator = Environment.NewLine;


        public MostRecentlyUsedFiles(int maxItems)
            : base(registryPath, mruKey, maxItems, Serialize, Deserialize, (f0, f1) => string.Compare(f0.FullName, f1.FullName, true))
        {
            // How can we ensure that we ALWAYS have the default file as the last item. It cannot be removed.
        }        

        private static IEnumerable<FileInfo> Deserialize(string fileInfoData)
        {
            return fileInfoData.Split(separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .Distinct(StringComparer.InvariantCultureIgnoreCase)
                .Select(p => new FileInfo(p))
                .Where(f => f.Exists);
        }

        private static string Serialize(IEnumerable<FileInfo> files)
        {
            return string.Join(separator, files.Select(f => f.FullName).ToArray());                
        }
    }
}
