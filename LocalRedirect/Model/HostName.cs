namespace Fiddler.LocalRedirect.Model
{
    using System;

    public class HostName
    {
        public HostName(string hostName)
        {
            var tmp = new Uri("http://" + hostName);
            Host = tmp.Host;
            Port = tmp.Port;
            Name = hostName;
        }

        public string Host { get; private set; }

        public int Port { get; private set; }

        public string Name { get; private set; }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            var hostName = obj as HostName;
            if (hostName == null)
                return false;

            return string.Compare(this.Name, hostName.Name, true) == 0;
        }
    }
}
