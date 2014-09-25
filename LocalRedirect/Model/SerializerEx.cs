using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;

namespace Fiddler.VSAutoResponder.Model
{
    public class SerializerEx<T>
    {
        private readonly DataContractSerializer serializer = new DataContractSerializer(typeof(T));
        
        public void Serialize(T t, Stream s)
        {
            using(var w = XmlWriter.Create(s, new XmlWriterSettings() { Encoding = Encoding.UTF8, Indent = true }))
                serializer.WriteObject(w, t);
        }
        
        public T Deserialize(Stream s)
        {            
            return (T)serializer.ReadObject(s);
        }

        public T DeepCopy(T t)
        {
            using (var mS = new MemoryStream())
            {
                serializer.WriteObject(mS, t);
                mS.Position = 0;
                return (T)serializer.ReadObject(mS);
            }
        }
    }
}
