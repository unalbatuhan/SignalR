using System;
using System.IO;
using System.Web.Configuration;
using Microsoft.AspNet.SignalR.Hubs;

namespace Microsoft.AspNet.SignalR.Tests.Common.Hubs
{
    [HubName("groupChat")]
    public class ChatWithGroups : Hub
    {
        public void Send(string group, string message)
        {
            Clients.Group(group).send(message);
        }

        public void Join(string group)
        {
            Groups.Add(Context.ConnectionId, group);
        }

        public void Leave(string group)
        {
            Groups.Remove(Context.ConnectionId, group);
        }

        public void TriggerAppDomainRestart()
        {
            var config = WebConfigurationManager.OpenWebConfiguration("~");
            string path = config.FilePath.ToLower().Replace("web.config", "");
            string file = Path.Combine(path, @"bin\test.dll");

            if (File.Exists(file))
            {
                File.Delete(file);
            }
            else
            {
                File.WriteAllText(file, "");
            }
        }
    }
}
