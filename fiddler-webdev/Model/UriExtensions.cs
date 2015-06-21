namespace System
{

    public static class UriExtensions
    {
        public static bool IsPartialMatch(this Uri url, Uri u)
        {
            return 
                u != null &&
                string.Compare(
                    url.GetComponents(UriComponents.SchemeAndServer, UriFormat.SafeUnescaped),
                    u.GetComponents(UriComponents.SchemeAndServer,UriFormat.SafeUnescaped)) == 0 &&
                url.PathAndQuery.StartsWith(u.PathAndQuery, StringComparison.InvariantCultureIgnoreCase);
        }        
    }
}
