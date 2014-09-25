using Fiddler.VSAutoResponder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;


// [assembly: Fiddler.RequiredVersion("4.4.9.3")]


public class VisualStudioResponder : Fiddler.IFiddlerExtension
{
    private readonly Socket udpSocket = new Socket(
        AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

    private readonly CancellationTokenSource serverThread =
        new CancellationTokenSource();
    private Task serverTask;    

    public VisualStudioResponder()
    {

    }

    public void OnBeforeUnload()
    {
        try
        {
            serverThread.Cancel();
            udpSocket.Dispose();
        }
        catch(Exception e)
        {
            Fiddler.FiddlerApplication.Log.LogFormat("Unexpected exception: {0}", e);
        }
        serverTask.Wait();
    }

    public void OnLoad()
    {        
        serverTask = Task.Factory.StartNew(
            ServerThread,
            serverThread.Token, 
            TaskCreationOptions.LongRunning, 
            TaskScheduler.Default);
    }

    public void ServerThread()
    {
        var recievedRules = new Dictionary<string, Fiddler.ResponderRule>();
        var autoResponder = Fiddler.FiddlerApplication.oAutoResponder;
        try
        {
            udpSocket.Bind(Configuration.EndPoint);
            var serializer = new DataContractJsonSerializer(typeof(AutoResponderRule[]));
            int recievedBytes = 0;
            EndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            var buffer = new byte[Configuration.MaxMessageSizeInBytes];

            while (!serverThread.IsCancellationRequested && (recievedBytes = udpSocket.ReceiveFrom(buffer, ref sender)) > 0)
            {
                using (var mS = new MemoryStream(buffer, 0, recievedBytes))
                {
                    var rules = (AutoResponderRule[])serializer.ReadObject(mS);
                    foreach (var rule in rules)
                    {
                        Fiddler.ResponderRule existingRule = null;
                        if (recievedRules.TryGetValue(rule.Key, out existingRule))
                        {
                            autoResponder.RemoveRule(existingRule);
                            recievedRules.Remove(rule.Key);
                        }
                        var newRule = autoResponder.AddRule(rule.Regex, rule.FullFilePath, true);
                        recievedRules.Add(rule.Key, newRule);
                    }
                }
            }

        }
        catch(Exception e)
        {
            Fiddler.FiddlerApplication.Log.LogFormat("Unexpected exception: {0}", e);
        }

        // Unload all rules we have received.
        foreach (var rule in recievedRules.Values)
            autoResponder.RemoveRule(rule);

        // BUG fix. SaveRules() is called by a fiddler UI control BEFORE OnBeforeUnload is called. Therefore
        // the removed rules are never reflected in the default config file.
        // therefore we need to save the default config file again (when rules have been removed).
        autoResponder.SaveRules(Fiddler.CONFIG.GetPath("AutoResponderDefaultRules"));        
    }
}

