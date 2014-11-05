using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Serialization;
using System.IO;
using System.Text;

namespace VSAutoResponder.Tests
{
 

    [TestClass]
    public class SerializationTests
    {
        private string sString =
@"<?xml version=""1.0""?>
<S xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <Value>S</Value>
</S>";
      

        [TestMethod]
        public void TestMethod1()
        {
            var s = new XmlSerializer(typeof(S));
            using(var ms = new MemoryStream())                        
            {
                //s.Serialize(ms, new S() { Value = "S" });                
                //ms.Position = 0;
                //var tmp = Encoding.UTF8.GetString(ms.ToArray());
                var data = Encoding.UTF8.GetBytes(sString);
                ms.Write(data, 0, data.Length);
                ms.Position = 0;
                var sCopy = (S)s.Deserialize(ms);
                Assert.AreEqual(10, S.Integer);
                Assert.AreEqual("S", sCopy.Value);
            }
            
        }
    }
}
