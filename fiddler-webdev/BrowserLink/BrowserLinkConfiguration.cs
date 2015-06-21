namespace Fiddler.Webdev.BrowserLink
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.MemoryMappedFiles;
    using System.Linq;
    using System.Security.Permissions;
    using System.Text;

    public class BrowserLinkConfiguration
    {
        private enum ConneectionStringIndex { HTTP = 0, HTTPS = 1, Path = 3};
        public IEnumerable<BrowserLinkConnection> GetAllBrowserLinkConnections()
        {
            foreach (var instanceFileName in GetAllInstanceFileNames())
            {
                var list = ReadAllLinesFrom(instanceFileName);                
                if (list.Count() > 2)
                {
                    string connectionString = list.ElementAt((int)ConneectionStringIndex.HTTP);
                    string sslConnectionString = list.ElementAt((int)ConneectionStringIndex.HTTPS);
                    string requestSignalName = instanceFileName + ".RequestSignal";
                    string readySignalName = instanceFileName + ".ReadySignal";                    
                    yield return new BrowserLinkConnection(connectionString, sslConnectionString, requestSignalName, readySignalName, list.Skip(2).ToArray());                    
                }
            }                        
        }
        

        
        private IEnumerable<string> GetAllInstanceFileNames()
        {
            IEnumerable<string> enumerable = Enumerable.Empty<string>();
            string[] indexFileNames = BrowserLinkConstants.IndexFileNames;
            for (int i = 0; i < indexFileNames.Length; i++)
            {
                string fileName = indexFileNames[i];
                enumerable = enumerable.Concat(ReadAllLinesFrom(fileName));
            }
            return enumerable;
        }

        [SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
        private IEnumerable<string> ReadAllLinesFrom(string fileName)
        {            
            if (MappedFileExists(fileName))
            {
                MemoryMappedFile memoryMappedFile = null;
                MemoryMappedViewStream memoryMappedViewStream = null;
                try
                {
                    memoryMappedFile = MemoryMappedFile.OpenExisting(fileName, MemoryMappedFileRights.Read);
                    memoryMappedViewStream = memoryMappedFile.CreateViewStream(0L, 0L, MemoryMappedFileAccess.Read);
                    return ReadAllLinesFrom(memoryMappedViewStream);
                }
                catch (FileNotFoundException)
                {
                }
                finally
                {
                    if (memoryMappedViewStream != null)
                    {
                        memoryMappedViewStream.Dispose();
                    }
                    if (memoryMappedFile != null)
                    {
                        memoryMappedFile.Dispose();
                    }
                }
            }
            return new List<string>();
        }

        private IEnumerable<string> ReadAllLinesFrom(MemoryMappedViewStream stream)
        {
            List<string> list = new List<string>();
            StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
            while (streamReader.Peek() > 0)
            {
                list.Add(streamReader.ReadLine());
            }
            return list;
        }
        private bool MappedFileExists(string fileName)
        {
            IntPtr intPtr = NativeMethods.OpenFileMapping(4u, false, fileName);
            if (intPtr == IntPtr.Zero)
            {
                return false;
            }
            NativeMethods.CloseHandle(intPtr);
            return true;
        }
    }
}
