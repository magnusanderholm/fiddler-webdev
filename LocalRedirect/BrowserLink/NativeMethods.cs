namespace Fiddler.LocalRedirect.BrowserLink
{
    using System;
    using System.Runtime.InteropServices;

    internal static class NativeMethods
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern IntPtr OpenFileMapping(uint dwDesiredAccess, bool bInheritHandle, string lpName);
        [DllImport("kernel32.dll")]
        internal static extern bool CloseHandle(IntPtr hHandle);
        [DllImport("urlmon.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        internal static extern int FindMimeFromData(IntPtr pBC, [MarshalAs(UnmanagedType.LPWStr)] string pwzUrl, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1, SizeParamIndex = 3)] byte[] pBuffer, int cbSize, [MarshalAs(UnmanagedType.LPWStr)] string pwzMimeProposed, int dwMimeFlags, out IntPtr ppwzMimeOut, int dwReserved);
    }
}
