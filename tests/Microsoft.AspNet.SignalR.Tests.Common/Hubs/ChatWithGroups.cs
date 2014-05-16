using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Newtonsoft.Json;
using System.Web.Configuration;
using System.Configuration;
using System.IO;
using System.Reflection;

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

        public void TriggerReconnect()
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

        public string CreateGroupsToken(string groupName)
        {
            IProtectedData protectedData = GlobalHost.DependencyResolver.Resolve<IProtectedData>();

            string groupsString = Context.ConnectionId + ':' + "[\"groupChat." + groupName + "\"]";
            
            string groupsToken = protectedData.Protect(groupsString, Purposes.Groups);

            Clients.All.send(groupName);

            return groupsToken;
        }
    }
}
