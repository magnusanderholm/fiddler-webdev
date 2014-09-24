using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VSAutoResponder.Msbuild;

namespace VSAutoResponder.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

            //var server = new VisualStudioResponder();
            //server.OnLoad();
            //System.Threading.Thread.Sleep(500);
            var client = new VSAutoResponderClient();
            for (int i = 0; i < 10;  ++i)
                client.Add(new Fiddler.VSAutoResponder.AutoResponderRule() { Regex = ".*/foo" + i.ToString(), FullFilePath = @"c:\temp" + i .ToString()});

            int j = 0;
            //System.Threading.Thread.Sleep(50000);
            //server.OnBeforeUnload();
        }
    }
}
