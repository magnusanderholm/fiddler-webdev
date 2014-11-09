namespace Fiddler.LocalRedirect.Model
{
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public class SerializerEx<T>
    {
        // TODO Could make use of the "extras" parameter to avoid using XmlInclude everywhere.....
        private readonly XmlSerializer serializer = new XmlSerializer(typeof(T));
        
        public void Serialize(T t, Stream s)
        {
            using(var w = XmlWriter.Create(s, new XmlWriterSettings() { Encoding = Encoding.UTF8, Indent = true }))
                serializer.Serialize(w, t);
        }
        
        public T Deserialize(Stream s)
        {            
            return (T)serializer.Deserialize(s);
        }

        public T DeepCopy(T t)
        {
            using (var mS = new MemoryStream())
            {
                serializer.Serialize(mS, t);
                mS.Position = 0;
                return (T)serializer.Deserialize(mS);
            }
        }
    }
}
