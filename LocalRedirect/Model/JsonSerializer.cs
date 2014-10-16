namespace Fiddler.LocalRedirect.Model
{
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public class JsonSerializer<T>
    {
        // TODO Could make use of the "extras" parameter to avoid using XmlInclude everywhere.....
        private readonly DataContractJsonSerializer serializer;

        public JsonSerializer()
        {
            serializer = new DataContractJsonSerializer(typeof(T), "root", null, int.MaxValue, false, null, false);              
        }
        
        public void Serialize(T t, Stream s)
        {
            serializer.WriteObject(s, t);
        }

        public string Serialize(T t)
        {
            using (var mS = new MemoryStream())
            using (var sR = new StreamReader(mS))
            {
                serializer.WriteObject(mS, t);
                mS.Position = 0;
                return sR.ReadToEnd();
            }            
        }           
    }
}
