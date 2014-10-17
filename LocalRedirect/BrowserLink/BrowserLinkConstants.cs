using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fiddler.LocalRedirect.BrowserLink
{
    public static class BrowserLinkConstants
    {
        public const string ElevatedIndexFileName = "Global\\PageInspector.Artery";
        public const string NonElevatedIndexFileName = "PageInspector.Artery";
        public const string RequestSignalSuffix = ".RequestSignal";
        public const string ReadySignalSuffix = ".ReadySignal";
        public static readonly string[] IndexFileNames = new string[]
		{
			"Global\\PageInspector.Artery",
			"PageInspector.Artery"
		};
    }
}
